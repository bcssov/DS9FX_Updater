// ***********************************************************************
// Assembly         : DS9FX_Updater
// Author           : Mario
// Created          : 09-13-2017
//
// Last Modified By : Mario
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="Logger.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using NLog;
using System;

namespace DS9FX_Updater
{
    /// <summary>
    /// Class Logger.
    /// </summary>
    public class Logger
    {
        #region Fields

        /// <summary>
        /// The log
        /// </summary>
        private static NLog.Logger log = LogManager.GetCurrentClassLogger();

        #endregion Fields

        #region Methods

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Log(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
            }
        }

        /// <summary>
        /// Logs the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void Log(Exception ex)
        {
            if (ex != null)
            {
                log.Error(ex);
            }
        }

        #endregion Methods
    }
}