// ***********************************************************************
// Assembly         : DS9FX_Updater
// Author           : Mario
// Created          : 09-12-2017
//
// Last Modified By : Mario
// Last Modified On : 09-12-2017
// ***********************************************************************
// <copyright file="Utils.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.Security.Cryptography;

namespace DS9FX_Updater
{
    /// <summary>
    /// Class Utils.
    /// </summary>
    public static class Utils
    {
        #region Methods

        /// <summary>
        /// Gets the checksum buffered.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        public static string GetChecksum(string path)
        {
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var bufferedStream = new BufferedStream(stream, 1024 * 32))
                {
                    var sha = new SHA256Managed();
                    byte[] checksum = sha.ComputeHash(bufferedStream);
                    return BitConverter.ToString(checksum).Replace("-", String.Empty);
                }
            }
        }

        #endregion Methods

    }
}