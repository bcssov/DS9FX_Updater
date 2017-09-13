// ***********************************************************************
// Assembly         : DS9FX_Updater
// Author           : Mario
// Created          : 09-12-2017
//
// Last Modified By : Mario
// Last Modified On : 09-12-2017
// ***********************************************************************
// <copyright file="UpdateInfo.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using System;

namespace DS9FX_Updater
{
    /// <summary>
    /// Class UpdateInfo.
    /// </summary>
    public class UpdateInfo
    {
        #region Properties

        /// <summary>
        /// Gets or sets the checksum.
        /// </summary>
        /// <value>The checksum.</value>
        [JsonProperty("checksum")]
        public string Checksum { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [should delete].
        /// </summary>
        /// <value><c>true</c> if [should delete]; otherwise, <c>false</c>.</value>
        [JsonIgnore]
        public bool ShouldDelete { get; set; }

        #endregion Properties
    }
}