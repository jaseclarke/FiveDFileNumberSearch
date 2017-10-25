using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace FiveDFileNumberSearch
{
    public class FileNumberRecord
    {
        public ModelRecord ModelRecord { get; set; }
        public int WellID { get; set; }
        public WellInfo WellInfo { get; set; }
    }

    public class ModelRecord
    {
        public int ID { get; set; }
        public string ModelPath { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class WellRecord
    {
        public int ID { get; set; }
        public int ModelID { get; set; }
        public string WellName { get; set; }
        public string FileNumber { get; set; }
    }

    public class DatabaseHelper
    {
        private readonly string _dbFilePath;

        public DatabaseHelper(string filePath)
        {
            _dbFilePath = filePath;
            InitialiseDatabase();
        }

        private void InitialiseDatabase()
        {
            if (!File.Exists(_dbFilePath))
            {
                CreateDatabase();
                SetLastUpdatedTime();
            }
        }

        private string ConnectionString => $"Data Source={_dbFilePath};";

        private void SetLastUpdatedTime()
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "REPLACE INTO [Settings] VALUES (NULL,$key,$value)";

                    cmd.Parameters.AddWithValue("$key", "LastUpdateTime");
                    cmd.Parameters.AddWithValue("$value", DateTime.Now.ToString("O"));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SetRootFolder(string rootFolder)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "REPLACE INTO [Settings] VALUES (NULL,$key,$value)";

                    cmd.Parameters.AddWithValue("$key", "RootFolder");
                    cmd.Parameters.AddWithValue("$value", rootFolder);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string GetRootFolder()
        {
            string rootFolder = string.Empty;
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT * FROM [Settings] WHERE [Key]=$key";

                    cmd.Parameters.AddWithValue("$key", "RootFolder");

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            rootFolder = rdr["Value"].ToString();
                        }
                    }
                }
            }
            return rootFolder;
        }


        public ModelRecord GetModelByPath(ModelInfo info)
        {
            ModelRecord record = null;
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT * FROM [Models] WHERE [FilePath]=$path";

                    cmd.Parameters.AddWithValue("$path", info.ModelPath);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            record = new ModelRecord
                            {
                                ID = Convert.ToInt32(rdr["ID"]),
                                ModelPath = rdr["FilePath"].ToString(),
                                LastUpdated = DateTime.Parse(rdr["LastModified"].ToString())
                            };
                        }
                    }
                }
            }
            return record;
        }

        public ModelRecord GetModelByID(int modelID)
        {
            ModelRecord record = null;
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT * FROM [Models] WHERE [ID]=$modelID";

                    cmd.Parameters.AddWithValue("$modelID", modelID);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            record = new ModelRecord
                            {
                                ID = Convert.ToInt32(rdr["ID"]),
                                ModelPath = rdr["FilePath"].ToString(),
                                LastUpdated = DateTime.Parse(rdr["LastModified"].ToString())
                            };
                        }
                    }
                }
            }
            return record;
        }


        public List<ModelRecord> GetAllModelDataRecords()
        {
            var recordList = new List<ModelRecord>();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT * FROM [Models]";

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var record = new ModelRecord
                            {
                                ID = Convert.ToInt32(rdr["ID"]),
                                ModelPath = rdr["FilePath"].ToString(),
                                LastUpdated = DateTime.Parse(rdr["LastModified"].ToString())
                            };
                            recordList.Add(record);
                        }
                    }
                }
            }
            return recordList;
        }

        public List<string> AllKnownFileNumbers()
        {
            var fileNumbers = new List<string>();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT DISTINCT [FileNumber] FROM [Wells] ORDER BY [FileNumber]";

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string fileNumber = rdr["FileNumber"].ToString().Trim();
                            if (fileNumber != string.Empty)
                            {
                                fileNumbers.Add(fileNumber);
                            }
                        }
                    }
                }
            }
            return fileNumbers;
        }

        public List<FileNumberRecord> SearchByFileNumber(string fileNumber)
        {
            var records = new List<FileNumberRecord>();
            foreach (string fn in AllKnownFileNumbers())
            {
                if (fn.ToLower().Contains(fileNumber.ToLower()))
                {
                    foreach (var wr in GetWellDataByFileNumber(fn))
                    {
                        var mr = GetModelByID(wr.ModelID);
                        var record = new FileNumberRecord
                        {
                            ModelRecord = mr,
                            WellID = wr.ID,
                            WellInfo = new WellInfo
                            {
                                FileNumber = wr.FileNumber,
                                WellName = wr.WellName,
                                PlanVersionList =  GetPlanDataByWellID(wr.ID)
                            }
                        };

                        records.Add(record);
                    }
                }
            }
            return records;
        }

        public void UpdateModel(ModelInfo info, FieldParser parser)
        {
            var existingModelRecord = GetModelByPath(info);
            if (existingModelRecord != null)
            {
                ClearModelData(existingModelRecord.ID);
            }
            UpdateModelTable(info);
            var newModelRecord = GetModelByPath(info);
            AddWellData(newModelRecord, parser);
            SetLastUpdatedTime();
        }

        private void ClearModelData(int modelID)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "DELETE FROM [Wells] WHERE [ModelID] = $modelID";

                    cmd.Parameters.AddWithValue("$modelID", modelID);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM [PlanVersions] WHERE [ModelID] = $modelID";

                    cmd.Parameters.AddWithValue("$modelID", modelID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void UpdateModelTable(ModelInfo info)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "REPLACE INTO [Models] VALUES (NULL,$filePath,$lastUpdateTime)";

                    cmd.Parameters.AddWithValue("$filePath", info.ModelPath);
                    cmd.Parameters.AddWithValue("$lastUpdateTime", info.LastModified());

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void AddWellData(ModelRecord record, FieldParser parser)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var txn = conn.BeginTransaction())
                {
                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.Transaction = txn;
                        foreach (var wi in parser.AllWellInfo)
                        {
                            if (wi.FileNumber.Trim() == string.Empty) continue;

                            cmd.CommandText = "INSERT INTO [Wells] VALUES (NULL, $modelID, $wellName, $fileNumber)";

                            cmd.Parameters.AddWithValue("$modelID", record.ID);
                            cmd.Parameters.AddWithValue("$wellName", wi.WellName);
                            cmd.Parameters.AddWithValue("$fileNumber", wi.FileNumber.Trim());

                            cmd.ExecuteNonQuery();

                            int wellID = GetLastInsertRowId(conn);
                            AddPlanVersionData(conn, txn, record, wi, wellID);
                        }
                    }
                    txn.Commit();
                }
            }
        }

        private void AddPlanVersionData(SQLiteConnection conn, SQLiteTransaction txn, ModelRecord record, WellInfo wi, int wellID)
        {
            using (var cmd = new SQLiteCommand(conn))
            {
                cmd.Transaction = txn;
                foreach (var pvi in wi.PlanVersionList)
                {
                    cmd.CommandText = "INSERT INTO [PlanVersions] VALUES (NULL, $modelID, $wellID, $planName, $versionName, $creationDate, $isDefinitive, $isCurrent)";

                    cmd.Parameters.AddWithValue("$modelID", record.ID);
                    cmd.Parameters.AddWithValue("$wellID", wellID);
                    cmd.Parameters.AddWithValue("$planName", pvi.PlanName);
                    cmd.Parameters.AddWithValue("$versionName", pvi.VersionName);
                    cmd.Parameters.AddWithValue("$creationDate", pvi.CreationDate);
                    cmd.Parameters.AddWithValue("$isDefinitive", pvi.IsDefinitivePlan ? 1 : 0);
                    cmd.Parameters.AddWithValue("$isCurrent", pvi.IsCurrentPlan ? 1 : 0);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private int GetLastInsertRowId(SQLiteConnection conn)
        {
            int id = -1;

            using (var cmd = new SQLiteCommand(conn))
            {
                cmd.CommandText = "SELECT last_insert_rowid()";

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        id = Convert.ToInt32(rdr[0]);
                    }
                }
            }

            return id;
        }

        public DateTime GetLastUpdateTime()
        {
            DateTime lastUpdateTime = DateTime.MinValue;
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT * FROM [Settings] WHERE [Key]=$key";

                    cmd.Parameters.AddWithValue("$key", "LastUpdateTime");

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            DateTime.TryParse(rdr["Value"].ToString(), out lastUpdateTime);
                        }
                    }
                }
            }
            return lastUpdateTime;
        }

        private List<PlanVersionInfo> GetPlanDataByWellID(int wellID)
        {
            var recordList = new List<PlanVersionInfo>();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT * FROM [PlanVersions]  WHERE [WellID]=$wellID ORDER BY [CreationDate] DESC, [VersionName] DESC ";
                    cmd.Parameters.AddWithValue("$wellID", wellID);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var record = new PlanVersionInfo
                            {
                                PlanName = rdr["PlanName"].ToString(),
                                VersionName = rdr["VersionName"].ToString(),
                                CreationDate = DateTime.Parse(rdr["CreationDate"].ToString()),
                                IsCurrentPlan = Convert.ToInt32(rdr["IsCurrent"]) != 0,
                                IsDefinitivePlan = Convert.ToInt32(rdr["IsDefinitive"]) != 0
                            };

                            recordList.Add(record);
                        }
                    }
                }
            }
            return recordList;
        }

        private List<WellRecord> GetWellDataByFileNumber(string fileNumber)
        {
            var recordList = new List<WellRecord>();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "SELECT * FROM [Wells]  WHERE [FileNumber]=$fileNumber";
                    cmd.Parameters.AddWithValue("$fileNumber", fileNumber);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var record = new WellRecord
                            {
                                ID = Convert.ToInt32(rdr["ID"]),
                                ModelID = Convert.ToInt32(rdr["ModelID"]),
                                WellName = rdr["Name"].ToString(),
                                FileNumber = rdr["FileNumber"].ToString()
                            };
                            recordList.Add(record);
                        }
                    }
                }
            }
            return recordList;
        }

        private void CreateDatabase()
        {
            SQLiteConnection.CreateFile(_dbFilePath);

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(conn))
                {
                    string createSettingsTableCommand = @"CREATE TABLE IF NOT EXISTS [Settings] (
                                                          [ID] INTEGER PRIMARY KEY,
                                                          [Key] NVARCHAR(2048) NULL,
                                                          [Value] VARCHAR(2048) NULL
                                                          )";

                    cmd.CommandText = createSettingsTableCommand; 
                    cmd.ExecuteNonQuery();

                    string createSettingsKeyIndexCommand = @"CREATE UNIQUE INDEX [Settings_Unique_Key] ON Settings (Key)";
                    cmd.CommandText = createSettingsKeyIndexCommand;
                    cmd.ExecuteNonQuery();



                    string createModelTableCommand = @"CREATE TABLE IF NOT EXISTS [Models] (
                                                          [ID] INTEGER PRIMARY KEY,
                                                          [FilePath] NVARCHAR(4096) NOT NULL,
                                                          [LastModified] DATETIME
                                                          )";

                    cmd.CommandText = createModelTableCommand;
                    cmd.ExecuteNonQuery();

                    string createModelsFilePathIndexCommand = @"CREATE UNIQUE INDEX [File_Path_Unique_Key] ON Models (FilePath)";
                    cmd.CommandText = createModelsFilePathIndexCommand;
                    cmd.ExecuteNonQuery();


                    string createWellTableCommand = @"CREATE TABLE IF NOT EXISTS [Wells] (
                                                          [ID] INTEGER PRIMARY KEY,
                                                          [ModelID] INTEGER NOT NULL,
                                                          [Name] VARCHAR(2048) NOT NULL,
                                                          [FileNumber] VARCHAR(2048) 
                                                          )";

                    cmd.CommandText = createWellTableCommand;
                    cmd.ExecuteNonQuery();

                    string createPlanVersionTableCommand = @"CREATE TABLE IF NOT EXISTS [PlanVersions] (
                                                          [ID] INTEGER PRIMARY KEY,
                                                          [ModelID] INTEGER NOT NULL,
                                                          [WellID] INTEGER NOT NULL,
                                                          [PlanName] VARCHAR(2048) NOT NULL,
                                                          [VersionName] VARCHAR(2048) NOT NULL,
                                                          [CreationDate] DATETIME, 
                                                          [IsDefinitive] INTEGER NOT NULL DEFAULT 0, 
                                                          [IsCurrent] INTEGER NOT NULL DEFAULT 0 
                                                          )";

                    cmd.CommandText = createPlanVersionTableCommand;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
