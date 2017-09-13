// ***********************************************************************
// Assembly         : DS9FX_Updater
// Author           : Mario
// Created          : 09-13-2017
//
// Last Modified By : Mario
// Last Modified On : 09-13-2017
// ***********************************************************************
// <copyright file="Logger.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.Windows.Forms;

namespace DS9FX_Updater
{
    /// <summary>
    /// Class Logger.
    /// </summary>
    public static class Logger
    {
        #region Fields

        /// <summary>
        /// The location
        /// </summary>
        private static readonly string location = Path.Combine(Application.StartupPath, "UpdaterLogs");

        #endregion Fields

        #region Methods

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Log(string message)
        {
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }
            File.AppendAllText(GetLogName(), string.Format("{0}: {1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message, Environment.NewLine));
        }

        /// <summary>
        /// Gets the name of the log.
        /// </summary>
        /// <returns>System.String.</returns>
        private static string GetLogName()
        {
            return Path.Combine(location, DateTime.Now.ToString("yyyy-MM-dd") + ".log");
        }

        #endregion Methods
    }
}