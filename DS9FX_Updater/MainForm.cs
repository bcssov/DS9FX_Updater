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
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            if (!this.InvokeRequired)
            {
                listBox1.Items.Add(text);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            else
            {
                this.BeginInvoke(new Action(() =>
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
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var directory = folderBrowserDialog1.SelectedPath;
                var files = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
                if (files?.Count() > 0)
                {
                    listBox1.Items.Clear();
                    var generator = new UpdateGenerator(files.ToList());
                    generator.StatusChanged += Generator_StatusChanged;
                    Task.Factory.StartNew(() =>
                    {
                        var results = generator.Generate();
                        generator.StatusChanged -= Generator_StatusChanged;
                        string json = JsonConvert.SerializeObject(results, Formatting.Indented);
                        File.WriteAllText(Path.Combine(Application.StartupPath, "updater_info.json"), json);
                        this.AddToListBoxAndFocus("Saved updater info to updater_info.json");
                    });
                }
            }
        }

        /// <summary>
        /// Generators the status changed.
        /// </summary>
        /// <param name="fileIndex">Index of the file.</param>
        /// <param name="totalFiles">The total files.</param>
        /// <param name="fileName">Name of the file.</param>
        private void Generator_StatusChanged(int fileIndex, int totalFiles, string fileName)
        {
            this.AddToListBoxAndFocus(string.Format("Generating signature of: {0}. File {1} of {2}", fileName, fileIndex, totalFiles));
            if (!this.InvokeRequired)
            {
                if (progressBar1.Maximum != totalFiles)
                {
                    progressBar1.Maximum = totalFiles;
                }
                progressBar1.Value = fileIndex;
            }
            else
            {
                this.BeginInvoke(new Action(() =>
                {
                    if (progressBar1.Maximum != totalFiles)
                    {
                        progressBar1.Maximum = totalFiles;
                    }
                    progressBar1.Value = fileIndex;
                }));
            }
        }

        /// <summary>
        /// Initializes the visibility.
        /// </summary>
        private void InitVisibility()
        {
            button1.Visible = Properties.Settings.Default.DevMode;
        }

        #endregion Methods
    }
}