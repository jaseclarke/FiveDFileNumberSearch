﻿using System;
using System.IO;
using System.IO.Compression;

namespace FiveDFileNumberSearchLib
{
    public class FiveDZipFileHandler : IDisposable
    {
        public FiveDZipFileHandler(string inputFileName)
        {
            ModelFilePath = inputFileName;
            if (!File.Exists(ModelFilePath))
            {
                throw new FileNotFoundException("5D Model Not Found",ModelFilePath);
            }
            ExtractArchive();
            if (!File.Exists(FieldXmlFileName))
            {
                FieldXmlFileName = string.Empty;
            }
        }

        public string FieldXmlFileName { get; set; }

        public string ModelFilePath { get; set; }

        public void ExtractArchive()
        {
            var modelDirectory = Path.GetDirectoryName(ModelFilePath);

            if (modelDirectory != null)
            {
                FieldXmlFileName = Path.Combine(modelDirectory, Path.GetRandomFileName());
                File.Delete(FieldXmlFileName);

                using (ZipArchive inputZip = ZipFile.Open(ModelFilePath, ZipArchiveMode.Read))
                {
                    var entry = inputZip.GetEntry("Field.xml");
                    entry?.ExtractToFile(FieldXmlFileName);
                }
            }
        }

        public void Dispose()
        {
            if (!string.IsNullOrWhiteSpace(FieldXmlFileName) && File.Exists(FieldXmlFileName))
            {
                File.Delete(FieldXmlFileName);
            }
        }
    }
}
