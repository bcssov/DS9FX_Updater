// ***********************************************************************
// Assembly         : DS9FX_Updater
// Author           : Mario
// Created          : 10-14-2017
//
// Last Modified By : Mario
// Last Modified On : 10-14-2017
// ***********************************************************************
// <copyright file="KMDetector.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace DS9FX_Updater
{
    /// <summary>
    /// Class KMDetector.
    /// </summary>
    public class KMDetector
    {
        #region Fields

        /// <summary>
        /// The exclud files
        /// </summary>
        private static readonly List<string> excludFiles = new List<string>()
        {
            "scripts\\mainmenu\\mainmenu.py"
        };

        /// <summary>
        /// The signature file.
        /// </summary>
        private readonly static string signatureFile = Path.Combine(Application.StartupPath, "scripts\\gameinfo.py");

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the exclude files.
        /// </summary>
        /// <value>The exclude files.</value>
        public List<string> ExcludeFiles
        {
            get
            {
                return excludFiles;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Determines whether this instance is km.
        /// </summary>
        /// <returns><c>true</c> if this instance is km; otherwise, <c>false</c>.</returns>
        public bool IsKM()
        {
            // TODO: Potentially detect versions...
            return File.Exists(signatureFile);
        }

        #endregion Methods
    }
}