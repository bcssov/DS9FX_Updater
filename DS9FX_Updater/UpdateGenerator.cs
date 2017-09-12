// ***********************************************************************
// Assembly         : DS9FX_Updater
// Author           : Mario
// Created          : 09-12-2017
//
// Last Modified By : Mario
// Last Modified On : 09-12-2017
// ***********************************************************************
// <copyright file="UpdateGenerator.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly string[] directories = new string[] { "data", "sfx" };

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateGenerator" /> class.
        /// </summary>
        /// <param name="files">The files.</param>
        public UpdateGenerator(List<string> files)
        {
            this.Files = files;
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
        /// <returns>List&lt;UpdateInfo&gt;.</returns>
        public List<UpdateInfo> Generate()
        {
            List<UpdateInfo> updateInfo = new List<UpdateInfo>();
            if (this.Files != null && this.Files.Count > 0)
            {
                var files = this.Files.Where(p => directories.Any(x => p.ToLowerInvariant().Contains(x)));
                var totalCount = files.Count();
                int index = 0;
                foreach (var item in files)
                {
                    index++;
                    this.StatusChanged?.Invoke(index, totalCount, item);
                    updateInfo.Add(new UpdateInfo()
                    {
                        Checksum = Utils.GetChecksum(item),
                        Path = GetCleanPath(item)
                    });
                }
            }
            return updateInfo;
        }

        /// <summary>
        /// Gets the clean path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        private string GetCleanPath(string path)
        {
            foreach (var item in this.directories)
            {
                if (path.ToLowerInvariant().Contains(item))
                {
                    var index = path.ToLowerInvariant().IndexOf(item);
                    return path.Substring(index);
                }
            }
            return string.Empty;
        }

        #endregion Methods
    }
}