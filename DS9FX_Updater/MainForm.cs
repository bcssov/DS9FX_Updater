// ***********************************************************************
// Assembly         : DS9FX_Updater
// Author           : Mario
// Created          : 09-12-2017
//
// Last Modified By : Mario
// Last Modified On : 09-12-2017
// ***********************************************************************
// <copyright file="MainForm.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DS9FX_Updater.Shared;

namespace DS9FX_Updater
{
    /// <summary>
    /// Class MainForm.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class MainForm : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm" /> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            InitVisibility();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Adds to ListBox and focus.
        /// </summary>
        /// <param name="text">The text.</param>
        private void AddToListBoxAndFocus(string text)
        {
            if (!InvokeRequired)
            {
                listBox1.Items.Add(text);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            else
            {
                BeginInvoke(new Action(() =>
                {
                    listBox1.Items.Add(text);
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                }));
            }
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            SetStatusLabelVisibility(false);
            switch (Properties.Settings.Default.Mode)
            {
                case Shared.Mode.Developer:
                    Task.Factory.StartNew(() => DownloadUpdatesAsync(true));
                    break;

                case Shared.Mode.Tester:
                    Task.Factory.StartNew(() => DownloadUpdatesAsync(false));
                    break;

                default:
                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    {
                        Task.Factory.StartNew(() => GenerateSignaturesAsync(folderBrowserDialog1.SelectedPath));
                    }
                    break;
            }
        }

        /// <summary>
        /// Clears the listbox.
        /// </summary>
        private void ClearListbox()
        {
            if (!InvokeRequired)
            {
                listBox1.Items.Clear();
            }
            else
            {
                BeginInvoke(new Action(() =>
                {
                    listBox1.Items.Clear();
                }));
            }
        }

        /// <summary>
        /// download updates as an asynchronous operation.
        /// </summary>
        /// <param name="ignoreScripts">if set to <c>true</c> [ignore scripts].</param>
        /// <returns>Task.</returns>
        private async Task DownloadUpdatesAsync(bool ignoreScripts)
        {
            ClearListbox();
            SetButtonStatus(false);
            var updater = new UpdateDownloader(ignoreScripts);
            AddToListBoxAndFocus("Fetching update signatures");
            await updater.LoadUpdatesAsync();
            updater.StatusChanged += Updater_StatusChanged;
            await updater.SyncAsync();
            updater.StatusChanged -= Updater_StatusChanged;
            SetStatusLabelVisibility(true);
            SetButtonStatus(true);
        }

        /// <summary>
        /// generate signatures as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        private void GenerateSignaturesAsync(string directory)
        {
            var files = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
            if (files?.Count() > 0)
            {
                SetButtonStatus(false);
                ClearListbox();
                var generator = new UpdateGenerator(files.ToList());
                generator.StatusChanged += Generator_StatusChanged;
                generator.Generate();
                generator.StatusChanged -= Generator_StatusChanged;
                AddToListBoxAndFocus("Saved updater info to " + Shared.UpdateIndexName + ".");
                SetStatusLabelVisibility(true);
                SetButtonStatus(true);
            }
        }

        /// <summary>
        /// Generators the status changed.
        /// </summary>
        /// <param name="fileIndex">Index of the file.</param>
        /// <param name="totalFiles">The total files.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="status">The status.</param>
        private void Generator_StatusChanged(int fileIndex, int totalFiles, string fileName, ProcessingStatus status)
        {
            AddToListBoxAndFocus(string.Format("Generating signature of: {0}.", fileName));
            SetProgressBar(fileIndex, totalFiles);
        }

        /// <summary>
        /// Initializes the visibility.
        /// </summary>
        private void InitVisibility()
        {
            SetStatusLabelVisibility(false);
            Text = string.Format("{0}: {1}", Text, Properties.Settings.Default.Mode);
            switch (Properties.Settings.Default.Mode)
            {
                case Shared.Mode.Developer:
                    button1.Text = "Download developer updates";
                    break;

                case Shared.Mode.Tester:
                    button1.Text = "Download latest version";
                    break;

                default:
                    button1.Text = "Generate signatures";
                    break;
            }
        }

        /// <summary>
        /// Sets the button status.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        private void SetButtonStatus(bool enabled)
        {
            if (!InvokeRequired)
            {
                button1.Enabled = enabled;
            }
            else
            {
                BeginInvoke(new Action(() =>
                {
                    button1.Enabled = enabled;
                }));
            }
        }

        /// <summary>
        /// Sets the progress bar.
        /// </summary>
        /// <param name="fileIndex">Index of the file.</param>
        /// <param name="totalFiles">The total files.</param>
        private void SetProgressBar(int fileIndex, int totalFiles)
        {
            if (!InvokeRequired)
            {
                if (progressBar1.Maximum != totalFiles)
                {
                    progressBar1.Maximum = totalFiles;
                }
                if (fileIndex <= totalFiles)
                {
                    progressBar1.Value = fileIndex;
                }
            }
            else
            {
                BeginInvoke(new Action(() =>
                {
                    if (progressBar1.Maximum != totalFiles)
                    {
                        progressBar1.Maximum = totalFiles;
                    }
                    if (fileIndex <= totalFiles)
                    {
                        progressBar1.Value = fileIndex;
                    }
                }));
            }
        }

        /// <summary>
        /// Sets the status label visibility.
        /// </summary>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        private void SetStatusLabelVisibility(bool visible)
        {
            if (!InvokeRequired)
            {
                label1.Visible = visible;
            }
            else
            {
                BeginInvoke(new Action(() =>
                {
                    label1.Visible = visible;
                }));
            }
        }

        /// <summary>
        /// Updaters the status changed.
        /// </summary>
        /// <param name="fileIndex">Index of the file.</param>
        /// <param name="totalFiles">The total files.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="status">The status.</param>
        private void Updater_StatusChanged(int fileIndex, int totalFiles, string fileName, ProcessingStatus status)
        {
            if (status == ProcessingStatus.Deleting)
            {
                AddToListBoxAndFocus(string.Format("Removing: {0}.", fileName));
            }
            else if (status == ProcessingStatus.Skipping)
            {
                AddToListBoxAndFocus(string.Format("Skipping: {0}.", fileName));
            }
            else
            {
                AddToListBoxAndFocus(string.Format("Downloading: {0}.", fileName));
            }
            SetProgressBar(fileIndex, totalFiles);
        }

        #endregion Methods
    }
}