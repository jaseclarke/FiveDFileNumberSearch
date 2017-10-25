using System;
using System.ComponentModel;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FiveDFileNumberSearch
{

    public partial class Form1 : Form
    {
        private readonly BackgroundWorker _bw = new BackgroundWorker();

        private string InputFileName { get; set; }

        private FieldParser _parser;

        private ModelInfo _modelInfo;

        private DatabaseHelper _dbHelper = null;
        private string _dbPath;

        public struct ChangeMessage
        {
            public string Message { get; set; }
            public Color  MessageColor { get; set; }
        }


        public Form1()
        {
            InitializeComponent();

            _bw.DoWork += ProcessArchive;
            _bw.ProgressChanged += ProcessArchiveProgressMessage;
            _bw.WorkerReportsProgress = true;
            _bw.RunWorkerCompleted += ProcesssingComplete;

            var dbFile = @"c:\temp\db.sqlite";
            if (!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);
            }

            searchFNBtn.Enabled = false;
            SetFormState();
        }

        private void ProcesssingComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            //DoUpdate.Enabled = true;
            //DoUpdate.Text = "Load Model";
            //richTextBox1.Clear();
            //ShowAllInfo();
        }

        private void ProcessArchiveProgressMessage(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is ChangeMessage message)
            {
                richTextBox1.AppendText(message.Message + Environment.NewLine,message.MessageColor);
            }
            else
            {
                richTextBox1.AppendText(e.UserState + Environment.NewLine);
            }
        }

        private void PrintInfo(string message)
        {
            richTextBox1.AppendText(message + Environment.NewLine);
        }

        private void ProcessArchive(object sender, DoWorkEventArgs e)
        {
            ProcessArchive(InputFileName);
        }

        public void ProcessArchive(string inputFilePath)
        {
            using (FiveDZipFileHandler opener = new FiveDZipFileHandler(inputFilePath))
            {
                _parser = new FieldParser(opener.FieldXmlFileName);
                _modelInfo = new ModelInfo {ModelPath = inputFilePath };
                _dbHelper.UpdateModel(_modelInfo,_parser);
            }
        }

        private void ShowAllInfo()
        {
            foreach (var wi in _parser.AllWellInfo)
            {
                PrintWellInfo(wi);
            }
        }

        private void PrintWellInfo(WellInfo wi)
        {
            var entryText = $"Well: {wi.WellName}, File Number: {wi.FileNumber}";
            if (wi.ParentWell != null)
            {
                entryText += $", Parent Well = {wi.ParentWell.WellName}";
            }
            PrintInfo(entryText);
            foreach (var pvi in wi.PlanVersionList)
            {
                var planText =
                    $"\tPlan: {pvi.PlanName}, Version: {pvi.VersionName}, Date: {pvi.CreationDate.ToShortDateString()}";

                if (pvi.IsDefinitivePlan)
                {
                    planText += " (Definitive Plan)";
                }
                if (pvi.IsCurrentPlan)
                {
                    planText += " (Current Plan)";
                }
                PrintInfo(planText);
            }
        }

        private void SetFormState()
        {
            bool dbLoaded = _dbHelper != null;
            bool rootFolderExists = dbLoaded && Directory.Exists(_dbHelper.GetRootFolder());

            setRootFolderBtn.Enabled = dbLoaded;
            ProcessChangesBtn.Enabled = rootFolderExists;
            showFileNumbersBtn.Enabled = dbLoaded;
            fileNumberTB.Enabled = dbLoaded;
            exportDatabaseToolStripMenuItem.Enabled = dbLoaded;
        }

        private void chooseInputFile_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog
            {
                Description = "Select the root folder for 5D file search.",
                ShowNewFolderButton = true
            };

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                rootFolderText.Text = fbd.SelectedPath;
            }
        }

        private void DoUpdate_Click(object sender, EventArgs e)
        {
            //InputFileName = rootFolderText.Text;

            //if (!File.Exists(rootFolderText.Text))
            //{
            //    MessageBox.Show($"The Input File {InputFileName} must exist.");
            //    return;
            //}

            //DoUpdate.Enabled = false;
            //DoUpdate.Text = "Processing";
            //richTextBox1.Clear();
            //_bw.RunWorkerAsync();
        }

        private void showAllBtn_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            ShowAllInfo();
        }

        private void showFileNumbersBtn_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            foreach (string fn in _dbHelper.AllKnownFileNumbers())
            {
                PrintInfo(fn);
            }
        }

        private void searchFNBtn_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();

            var records = _dbHelper.SearchByFileNumber(fileNumberTB.Text.Trim());

            foreach (var record in records)
            {
                PrintModellInfo(record.ModelRecord);
                PrintWellInfo(record.WellInfo);
            }
        }

        private void PrintModellInfo(ModelRecord mr)
        {
            var entryText = $"5D File: {mr.ModelPath}, Last Updated: {mr.LastUpdated.ToShortDateString()}";
            PrintInfo(entryText);
        }

        private void fileNumberTB_TextChanged(object sender, EventArgs e)
        {
            searchFNBtn.Enabled = fileNumberTB.Text != string.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FiveDFileHelper helper = new FiveDFileHelper(_dbHelper.GetRootFolder());

            richTextBox1.Clear();
            var changedFiles = helper.ChangedFiles(_dbHelper);
            if (changedFiles.Count > 0)
            {
                foreach (var fiveDFile in changedFiles)
                {
                    PrintInfo(fiveDFile);
                    ProcessArchive(fiveDFile);
                }
            }
            else
            {
                PrintInfo("No Changes Found.");
            }
            SetDatabaseStatusMessage();
        }

        private void newDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "5D Model Database (*.db)|*.db|All Files (*.*)|*.*",
                OverwritePrompt = true
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(sfd.FileName))
                {
                    File.Delete(sfd.FileName);
                }
                _dbPath = sfd.FileName;
                _dbHelper = new DatabaseHelper(sfd.FileName);
                rootFolderText.Text = _dbHelper.GetRootFolder();
                SetDatabaseStatusMessage();
                SetFormState();
            }
        }

        void SetDatabaseStatusMessage()
        {
            if (_dbHelper == null)
            {
                DatabaseStatusText.Text = "Database Loaded: None";
            }
            else
            {
                DatabaseStatusText.Text =
                    $"Database Loaded: {_dbPath}, Last Modified: {_dbHelper.GetLastUpdateTime():G}";
            }
        }

        private void openDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "5D Model Database (*.db)|*.db|All Files (*.*)|*.*",
                CheckPathExists = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _dbPath = ofd.FileName;
                _dbHelper = new DatabaseHelper(ofd.FileName);
                rootFolderText.Text = _dbHelper.GetRootFolder();
                SetDatabaseStatusMessage();
                PrintModelHistory();
                SetFormState();
            }
        }

        private void PrintModelHistory()
        {
            richTextBox1.Clear();
            PrintInfo("Loaded 5D Files:");

            foreach (var mr in _dbHelper.GetAllModelDataRecords())
            {
                PrintInfo($"File: {mr.ModelPath}, Last Updated: {mr.LastUpdated:G}");
            }
        }

        private void SetRootFolderBtnClick(object sender, EventArgs e)
        {
            if (!Directory.Exists(rootFolderText.Text))
            {
                MessageBox.Show("Root Folder must be a valid directory.");
                return;
            }

            _dbHelper.SetRootFolder(rootFolderText.Text);
            rootFolderText.Text = _dbHelper.GetRootFolder();
            SetDatabaseStatusMessage();
            SetFormState();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void exportDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "5D Model Database (*.db)|*.db|All Files (*.*)|*.*",
                OverwritePrompt = true
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.Copy(_dbPath, sfd.FileName);
                richTextBox1.Clear();
                PrintInfo($"Database Saved to {sfd.FileName}");
            }
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }

    public class ModelInfo
    {
        public string ModelPath { get; set; }

        public DateTime LastModified()
        {
            if (File.Exists(ModelPath))
            {
                return File.GetLastWriteTime(ModelPath);
            }
            return DateTime.MinValue;
        }
    }
}
