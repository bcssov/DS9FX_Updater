// ***********************************************************************
// Assembly         : DS9FX_Updater
// Author           : Mario
// Created          : 09-12-2017
//
// Last Modified By : Mario
// Last Modified On : 09-13-2017
// ***********************************************************************
// <copyright file="UpdateDownloader.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DS9FX_Updater.Shared;

namespace DS9FX_Updater
{
    /// <summary>
    /// Class UpdateDownloader.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class UpdateDownloader : IDisposable
    {
        #region Fields

        /// <summary>
        /// The core updates URL
        /// </summary>
        private static readonly string coreUpdatesUrl = string.Format("{0}/sov/ds9fx/", Properties.Settings.Default.BaseUpdateUrl);

        /// <summary>
        /// The index URL
        /// </summary>
        private static readonly string indexUrl = string.Format("{0}/sov/ds9fx/{1}", Properties.Settings.Default.BaseUpdateUrl, Shared.UpdateIndexName);

        /// <summary>
        /// The local index URL
        /// </summary>
        private static readonly string localIndexUrl = Path.Combine(Application.StartupPath, Shared.UpdateIndexName);

        /// <summary>
        /// The client
        /// </summary>
        private HttpClient client;

        /// <summary>
        /// The processed
        /// </summary>
        private int processed = 0;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDownloader" /> class.
        /// </summary>
        /// <param name="ignoreScripts">if set to <c>true</c> [ignore scripts].</param>
        public UpdateDownloader(bool ignoreScripts)
        {
            IgnoreScripts = ignoreScripts;
            client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("USER_AGENT", "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.79 Safari/537.36");
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when [status changed].
        /// </summary>
        public event StatusDelegate StatusChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the existing updates.
        /// </summary>
        /// <value>The existing updates.</value>
        public List<UpdateInfo> ExistingUpdates { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [ignore scripts].
        /// </summary>
        /// <value><c>true</c> if [ignore scripts]; otherwise, <c>false</c>.</value>
        public bool IgnoreScripts { get; private set; }

        /// <summary>
        /// Gets the updates.
        /// </summary>
        /// <value>The updates.</value>
        public List<UpdateInfo> Updates { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (client != null)
            {
                client.Dispose();
            }
        }

        /// <summary>
        /// load updates as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task LoadUpdatesAsync()
        {
            string json = await DownloadUpdatesAsync();
            Updates = JsonConvert.DeserializeObject<List<UpdateInfo>>(json);
            if (File.Exists(localIndexUrl))
            {
                ExistingUpdates = JsonConvert.DeserializeObject<List<UpdateInfo>>(File.ReadAllText(localIndexUrl));
            }
        }

        /// <summary>
        /// synchronize as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task SyncAsync()
        {
            var diffs = GetDiffs();
            var totalCount = diffs.Count;
            using (var semaphore = new SemaphoreSlim(Shared.MaxConnections))
            {
                var tasks = diffs.Select(async diff =>
                {
                    await semaphore.WaitAsync();
                    try
                    {                        
                        if (diff.ShouldDelete)
                        {                            
                            if (File.Exists(GetUpdatePath(diff.Path)))
                            {
                                File.Delete(GetUpdatePath(diff.Path));
                            }
                            processed++;
                            StatusChanged?.Invoke(processed, totalCount, diff.Path, ProcessingStatus.Deleted);
                        }
                        else
                        {
                            if (File.Exists(GetUpdatePath(diff.Path)))
                            {
                                var checksum = Utils.GetChecksum(GetUpdatePath(diff.Path));
                                if (diff.Checksum != checksum)
                                {                                    
                                    await SaveUpdateAsync(diff.Path);
                                    processed++;
                                    StatusChanged?.Invoke(processed, totalCount, diff.Path, ProcessingStatus.Downloaded);
                                }
                                else
                                {
                                    processed++;
                                    StatusChanged?.Invoke(processed, totalCount, diff.Path, ProcessingStatus.Skipped);
                                }
                            }
                            else
                            {                                
                                await SaveUpdateAsync(diff.Path);
                                processed++;
                                StatusChanged?.Invoke(processed, totalCount, diff.Path, ProcessingStatus.Downloaded);
                            }
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });
                await Task.WhenAll(tasks);
                string json = JsonConvert.SerializeObject(Updates, Formatting.Indented);
                File.WriteAllText(Path.Combine(Application.StartupPath, Shared.UpdateIndexName), json);
            }            
        }

        /// <summary>
        /// download updates as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.String&gt;.</returns>
        private async Task<string> DownloadUpdatesAsync()
        {
            return await client.GetStringAsync(indexUrl);
        }

        /// <summary>
        /// Gets the diffs.
        /// </summary>
        /// <returns>List&lt;UpdateInfo&gt;.</returns>
        private List<UpdateInfo> GetDiffs()
        {
            if (ExistingUpdates != null)
            {
                List<UpdateInfo> diffs = new List<UpdateInfo>();
                List<UpdateInfo> updates = Updates.ToList();
                List<UpdateInfo> existingUpdates = ExistingUpdates.ToList();
                if (IgnoreScripts)
                {
                    updates = Updates.Where(p => !p.Path.ToLowerInvariant().Contains("scripts\\")).ToList();
                    existingUpdates = ExistingUpdates.Where(p => !p.Path.ToLowerInvariant().Contains("scripts\\")).ToList();
                }
                foreach (var update in updates)
                {
                    var updatePath = update.Path.ToLowerInvariant();
                    var existingUpdate = existingUpdates.FirstOrDefault(p => p.Path.ToLowerInvariant() == updatePath);
                    if (existingUpdate != null)
                    {
                        if (existingUpdate.Checksum != update.Checksum)
                        {
                            diffs.Add(new UpdateInfo()
                            {
                                Checksum = update.Checksum,
                                Path = update.Path
                            });
                        }
                        existingUpdates.Remove(existingUpdate);
                    }
                    else
                    {
                        diffs.Add(new UpdateInfo()
                        {
                            Checksum = update.Checksum,
                            Path = update.Path
                        });
                    }
                }
                foreach (var update in existingUpdates)
                {
                    diffs.Add(new UpdateInfo()
                    {
                        Checksum = update.Checksum,
                        Path = update.Path,
                        ShouldDelete = true
                    });
                }
                return diffs;
            }
            return Updates;
        }

        /// <summary>
        /// Gets the update path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        private string GetUpdatePath(string path)
        {
            return Path.Combine(Application.StartupPath, path);
        }

        /// <summary>
        /// Gets the update URL.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        private string GetUpdateUrl(string path)
        {
            if (path.ToLowerInvariant().EndsWith(".py"))
            {
                // NOTE: Server refuses to serve py files, which is to be expected. Also by changing extension prevent it from being treated as a txt file....
                return string.Format("{0}{1}", coreUpdatesUrl, path.Replace("\\", "/").Replace(".py", ".ds9fx"));
            }
            return string.Format("{0}{1}", coreUpdatesUrl, path.Replace("\\", "/"));
        }

        /// <summary>
        /// save update as an asynchronous operation.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Task.</returns>
        private async Task SaveUpdateAsync(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(GetUpdatePath(path))))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(GetUpdatePath(path)));
            }
            using (var response = await client.GetAsync(GetUpdateUrl(path), HttpCompletionOption.ResponseHeadersRead))
            {
                using (var downloadStream = await response.Content.ReadAsStreamAsync())
                {
                    using (var localStream = File.Open(GetUpdatePath(path), FileMode.Create))
                    {
                        await downloadStream.CopyToAsync(localStream);
                    }
                }
            }
        }

        #endregion Methods
    }
}