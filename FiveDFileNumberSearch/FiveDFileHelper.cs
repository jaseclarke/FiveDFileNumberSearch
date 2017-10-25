using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FiveDFileNumberSearch
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

        public List<string> ChangedFiles(DatabaseHelper dbHelper)
        {
            var changedFiles = new List<string>();
            var modelData = dbHelper.GetAllModelDataRecords();

            foreach (var file in FindAllFiveDFiles())
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

            return changedFiles;
        }
    }
}
