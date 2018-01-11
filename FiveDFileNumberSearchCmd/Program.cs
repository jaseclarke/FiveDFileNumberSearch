using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using FiveDFileNumberSearchLib;

namespace FiveDFileNumberSearchCmd
{
    class CommandLineOptions
    {
        [Option("db", Required = true)]
        public string DatabasePath { get; set; }

        [Option('q',"quiet", Required = false, DefaultValue = false)]
        public bool Quiet { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
    class Program
    {
        private static DatabaseHelper _dbHelper;
        private static bool _silent = false;
        static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (Parser.Default.ParseArguments(args, options))
            {
                if (!File.Exists(options.DatabasePath))
                {
                    Console.WriteLine($"Database File ${options.DatabasePath} not found!");
                    return;
                }
                _silent = options.Quiet;
                _dbHelper = new DatabaseHelper(options.DatabasePath);
                UpdateDatabase();
            }
        }

        private static void UpdateDatabase()
        {
            FiveDFileHelper helper = new FiveDFileHelper(_dbHelper.GetRootFolder());

            var changes = helper.ChangedFiles(_dbHelper);

            var changedFiles = changes.Item1;
            var deletedFiles = changes.Item2;

            if (changedFiles.Count > 0)
            {
                foreach (var fiveDFile in changedFiles)
                {
                    bool isNetworkFile = false;
                    string tempFile = string.Empty;
                    try
                    {
                        if (FiveDFileHelper.IsNetworkPath(fiveDFile))
                        {
                            tempFile = Path.GetTempFileName();
                            PrintInfo($"Making Local Copy of {fiveDFile}");
                            File.Copy(fiveDFile, tempFile, true);
                            File.SetAttributes(tempFile, ~FileAttributes.ReadOnly);
                            isNetworkFile = true;
                        }
                        PrintInfo($"Processing {fiveDFile}");
                        ProcessArchive(fiveDFile, tempFile);
                    }
                    finally
                    {
                        if (isNetworkFile && File.Exists(tempFile))
                        {
                            File.Delete(tempFile);
                        }
                    }
                }
            }
            if (deletedFiles.Count > 0)
            {
                foreach (var fiveDFile in deletedFiles)
                {
                    PrintInfo($"Deleted File: {fiveDFile}.");
                    _dbHelper.DeleteModel(fiveDFile);
                }
            }
            if (changedFiles.Count == 0 && deletedFiles.Count == 0)
            {
                PrintInfo("No Changes Found.");
            }
        }
        public static void ProcessArchive(string inputFilePath, string tempCopyFileName)
        {
            string archiveFile = string.IsNullOrWhiteSpace(tempCopyFileName) ? inputFilePath : tempCopyFileName;
            using (FiveDZipFileHandler opener = new FiveDZipFileHandler(archiveFile))
            {
                if (string.IsNullOrWhiteSpace(opener.FieldXmlFileName)) return;
                var parser = new FieldParser(opener.FieldXmlFileName);
                var modelInfo = new ModelInfo { ModelPath = inputFilePath };
                _dbHelper.UpdateModel(modelInfo, parser);
            }
        }

        private static void PrintInfo(string message)
        {
            if (!_silent)
            {
                Console.WriteLine(message);
            }
        }
    }
}
