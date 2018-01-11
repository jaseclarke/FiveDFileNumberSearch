using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FiveDFileNumberSearchLib
{
    public class FiveDFileHelper
    {
        private readonly string _rootFolder;

        public FiveDFileHelper(string rootFolder)
        {
            _rootFolder = rootFolder;
        }

        public List<string> FindAllFiveDFiles()
        {
            var fiveDFiles = Directory.EnumerateFiles(_rootFolder, "*.5dz").ToList();

            foreach (var dir in Directory.EnumerateDirectories(_rootFolder))
            {
                fiveDFiles.AddRange(Directory.EnumerateFiles(dir, "*.5dz"));
            }

            return fiveDFiles;
        }

        public static bool IsNetworkPath(string filePath)
        {
            bool isNetworkPath = new Uri(filePath).IsUnc;
            if (!isNetworkPath)
            {
                DriveInfo drive = new DriveInfo(Path.GetPathRoot(filePath));
                isNetworkPath = drive.DriveType == DriveType.Network;
            }
            return isNetworkPath;
        }

        public Tuple<List<string>,List<string>> ChangedFiles(DatabaseHelper dbHelper)
        {
            var changedFiles = new List<string>();
            var modelData = dbHelper.GetAllModelDataRecords();

            var allFiveDFiles = FindAllFiveDFiles();

            foreach (var file in allFiveDFiles)
            {
                var foundModelData = modelData.FirstOrDefault(md => md.ModelPath == file);

                bool changed = true;
                if (foundModelData != null)
                {
                    var timeDiffInSeconds = (File.GetLastWriteTime(file) - foundModelData.LastUpdated).TotalSeconds;
                    changed = timeDiffInSeconds > 30;
                }

                if (changed)
                {
                    changedFiles.Add(file);
                }
            }

            var deletedFiles = new List<string>();

            foreach (var md in modelData)
            {
                if (!allFiveDFiles.Contains(md.ModelPath))
                {
                    deletedFiles.Add(md.ModelPath);
                }
            }

            return new Tuple<List<string>,List<string>>(changedFiles,deletedFiles);
        }
    }
}
