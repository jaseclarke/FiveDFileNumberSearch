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

        private FieldParser _parser = null;

        private ModelInfo _modelInfo = null;

        private DatabaseHelper _dbHelper = new DatabaseHelper(@"C:\Temp\modeldb.sqlite");

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
        }

        private void ProcesssingComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            DoUpdate.Enabled = true;
            DoUpdate.Text = "Load Model";
            richTextBox1.Clear();
            ShowAllInfo();
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
            ProcessArchive();
        }

        public void ProcessArchive()
        {
            using (FiveDZipFileHandler opener = new FiveDZipFileHandler(InputFileName))
            {
                _parser = new FieldParser(opener.FieldXmlFileName);
                _modelInfo = new ModelInfo {ModelPath = InputFileName};
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

        private void chooseInputFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "5D Files|*.5dz|All Files|*.*",
                Title = "Choose 5D Input File"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                inputFileNameText.Text = ofd.FileName;
            }
        }


        private void DoUpdate_Click(object sender, EventArgs e)
        {
            InputFileName = inputFileNameText.Text;

            if (!File.Exists(inputFileNameText.Text))
            {
                MessageBox.Show($"The Input File {InputFileName} must exist.");
                return;
            }

            DoUpdate.Enabled = false;
            DoUpdate.Text = "Processing";
            richTextBox1.Clear();
            _bw.RunWorkerAsync();
        }

        private void showAllBtn_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            ShowAllInfo();
        }

        private void showFileNumbersBtn_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            //List<string> fileNumbers = _parser.AllWellInfo.Where(wi => wi.FileNumber != string.Empty)
            //    .Select(wi => wi.FileNumber.Trim()).ToList();
            //fileNumbers.Sort();
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
            //string searchText = fileNumberTB.Text.Trim().ToLower();
            //foreach (var wi in _parser.AllWellInfo)
            //{
            //    if (wi.FileNumber.ToLower().Contains(searchText))
            //    {
            //        PrintWellInfo(wi);
            //    }
            //}
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
            var lud = _dbHelper.GetLastUpdateTime();
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
