// ***********************************************************************
// Assembly         : DS9FX_Updater
// Author           : Mario
// Created          : 09-12-2017
//
// Last Modified By : Mario
// Last Modified On : 09-13-2017
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
        #region Fields

        /// <summary>
        /// The maximum connections
        /// </summary>
        public const int MaxConnections = 2;

        /// <summary>
        /// The update index name
        /// </summary>
        public const string UpdateIndexName = "updater_info.json";

        #endregion Fields

        #region Delegates

        /// <summary>
        /// Delegate Status.
        /// </summary>
        /// <param name="fileIndex">Index of the file.</param>
        /// <param name="totalFiles">The total files.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="status">The status.</param>
        public delegate void StatusDelegate(int fileIndex, int totalFiles, string fileName, ProcessingStatus status);

        #endregion Delegates

        #region Enums

        /// <summary>
        /// Enum Modes
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// The generator
            /// </summary>
            Generator,

            /// <summary>
            /// The dev
            /// </summary>
            Developer,

            /// <summary>
            /// The tester
            /// </summary>
            Tester
        }

        /// <summary>
        /// Enum ProcessingStatus
        /// </summary>
        public enum ProcessingStatus
        {
            /// <summary>
            /// The calculated
            /// </summary>
            Calculated,

            /// <summary>
            /// The downloaded
            /// </summary>
            Downloaded,

            /// <summary>
            /// The deleted
            /// </summary>
            Deleted,

            /// <summary>
            /// The skipped
            /// </summary>
            Skipped
        }

        #endregion Enums
    }
}