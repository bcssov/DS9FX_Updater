// ***********************************************************************
// Assembly         : DS9FX_Updater
// Author           : Mario
// Created          : 09-12-2017
//
// Last Modified By : Mario
// Last Modified On : 09-15-2017
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
        /// <param name="e">The e.</param>
        public delegate void StatusDelegate(StatusArgument e);

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

        #region Classes

        /// <summary>
        /// Class StatusArgument.
        /// </summary>
        /// <seealso cref="System.EventArgs" />
        public class StatusArgument : EventArgs
        {
            #region Properties

            /// <summary>
            /// Gets or sets the index of the file.
            /// </summary>
            /// <value>The index of the file.</value>
            public int FileIndex { get; set; }

            /// <summary>
            /// Gets or sets the name of the file.
            /// </summary>
            /// <value>The name of the file.</value>
            public string FileName { get; set; }

            /// <summary>
            /// Gets or sets the status.
            /// </summary>
            /// <value>The status.</value>
            public ProcessingStatus Status { get; set; }

            /// <summary>
            /// Gets or sets the total files.
            /// </summary>
            /// <value>The total files.</value>
            public int TotalFiles { get; set; }

            #endregion Properties
        }

        #endregion Classes
    }
}