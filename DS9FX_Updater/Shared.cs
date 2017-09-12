// ***********************************************************************
// Assembly         : DS9FX_Updater
// Author           : Mario
// Created          : 09-12-2017
//
// Last Modified By : Mario
// Last Modified On : 09-12-2017
// ***********************************************************************
// <copyright file="Shared.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace DS9FX_Updater
{
    /// <summary>
    /// Class Shared.
    /// </summary>
    public class Shared
    {
        #region Delegates

        /// <summary>
        /// Delegate Status.
        /// </summary>
        /// <param name="fileIndex">Index of the file.</param>
        /// <param name="totalFiles">The total files.</param>
        /// <param name="fileName">Name of the file.</param>
        public delegate void StatusDelegate(int fileIndex, int totalFiles, string fileName);

        #endregion Delegates
    }
}