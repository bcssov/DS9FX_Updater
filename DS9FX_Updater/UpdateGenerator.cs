// ***********************************************************************
// Assembly         : DS9FX_Updater
// Author           : Mario
// Created          : 09-12-2017
//
// Last Modified By : Mario
// Last Modified On : 09-13-2017
// ***********************************************************************
// <copyright file="UpdateGenerator.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static DS9FX_Updater.Shared;

namespace DS9FX_Updater
{
    /// <summary>
    /// Class UpdateGenerator.
    /// </summary>
    public class UpdateGenerator
    {
        #region Fields

        /// <summary>
        /// The directories
        /// </summary>
        private readonly string[] directories = new string[] { "\\data\\", "\\scripts\\", "\\sfx\\" };

        /// <summary>
        /// The processed
        /// </summary>
        private int processed = 0;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateGenerator" /> class.
        /// </summary>
        /// <param name="files">The files.</param>
        public UpdateGenerator(List<string> files)
        {
            Files = files;
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
        /// Gets the files.
        /// </summary>
        /// <value>The files.</value>
        public List<string> Files { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Generates this instance.
        /// </summary>
        public void Generate()
        {
            List<UpdateInfo> updateInfo = new List<UpdateInfo>();
            if (Files?.Count > 0)
            {
                var files = Files.Where(p => directories.Any(x => p.ToLowerInvariant().Contains(x)));
                var totalCount = files.Count();

                foreach (var file in files)
                {
                    var checksum = Utils.GetChecksum(file);
                    processed++;
                    StatusChanged?.Invoke(processed, totalCount, file, ProcessingStatus.Calculating);
                    updateInfo.Add(new UpdateInfo()
                    {
                        Checksum = checksum,
                        Path = GetCleanPath(file)
                    });
                }
            }
            string json = JsonConvert.SerializeObject(updateInfo, Formatting.Indented);
            File.WriteAllText(Path.Combine(Application.StartupPath, Shared.UpdateIndexName), json);
        }

        /// <summary>
        /// Gets the clean path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        private string GetCleanPath(string path)
        {
            foreach (var directory in directories)
            {
                if (path.ToLowerInvariant().Contains(directory))
                {
                    var index = path.ToLowerInvariant().IndexOf(directory);
                    return path.Substring(index + 1);
                }
            }
            return string.Empty;
        }

        #endregion Methods
    }
}