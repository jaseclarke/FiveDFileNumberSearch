using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using File = Google.Apis.Drive.v3.Data.File;

namespace FiveDFileNumberSearchLib
{
    public class GoogleDriveHelper
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "FiveDFileSearch";

        private UserCredential _credential;
        private DriveService _driveService;

        public GoogleDriveHelper()
        {
            GetCredentials();

            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = ApplicationName,
            });
        }

        public void UpdateStoredFile(string filePath, string folderName)
        {
            var matchingFolders = FindFolder(folderName);
            if (matchingFolders == null || matchingFolders.Count == 0)
            {
                matchingFolders =
                    new List<Google.Apis.Drive.v3.Data.File> {CreateDirectory(folderName, "5D File Search Data")};
            }
            var parentFolder = matchingFolders[0];

            InsertOrUpdateFile(filePath, parentFolder);
        }

        private void GetCredentials()
        {
            using (var stream =
                new FileStream("client_id.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/FiveDFileSearch.json");

                _credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

                //Console.WriteLine("Credential file saved to: " + credPath);
            }
        }

        private Google.Apis.Drive.v3.Data.File InsertOrUpdateFile(string filePath, Google.Apis.Drive.v3.Data.File parentFolder)
        {
            string fileName = Path.GetFileName(filePath);

            var fileMetaData = new Google.Apis.Drive.v3.Data.File
            {
                Name = fileName,
                Description = "5D File Search Database",
                MimeType = "application/octet-stream"
            };

            Google.Apis.Drive.v3.Data.File existingFile = null;
            var matchingFileList = FindFileInFolder(fileName, parentFolder);

            if (matchingFileList != null && matchingFileList.Count > 0)
            {
                existingFile = matchingFileList[0];
            }

            if (existingFile == null)
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    var request = _driveService.Files.Create(fileMetaData, stream, "application/octet-stream");
                    request.Fields = "id,name,parents";
                    fileMetaData.Parents = new List<string> { parentFolder.Id };
                    request.Upload();

                    return request.ResponseBody;
                }
            }
            else
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    var request = _driveService.Files.Update(fileMetaData, existingFile.Id, stream, "application/octet-stream");
                    request.Fields = "id,name,parents";
                    request.Upload();

                    return request.ResponseBody;
                }
            }
        }

        private Google.Apis.Drive.v3.Data.File CreateDirectory(string name, string description)
        {
            Google.Apis.Drive.v3.Data.File newDirectory = null;

            var body = new Google.Apis.Drive.v3.Data.File
            {
                Name = name,
                Description = description,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string> { "root" }
            };

            try
            {
                var request = _driveService.Files.Create(body);
                newDirectory = request.Execute();
            }
            catch (Exception e)
            {
                return null;
            }
            return newDirectory;
        }

        private IList<Google.Apis.Drive.v3.Data.File> FindFileInFolder(string fileName, Google.Apis.Drive.v3.Data.File parentFolder)
        {
            // Define parameters of request.
            FilesResource.ListRequest listRequest = _driveService.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name, parents)";
            listRequest.Q = $"name contains '{fileName}' and '{parentFolder.Id}' in parents";

            return DoFileQuery(listRequest);
        }

        private IList<Google.Apis.Drive.v3.Data.File> FindFolder(string folderName)
        {
            // Define parameters of request.
            FilesResource.ListRequest listRequest = _driveService.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name, parents)";
            listRequest.Q = $"mimeType = 'application/vnd.google-apps.folder' and name contains '{folderName}'";

            return DoFileQuery(listRequest);
        }

        private static IList<Google.Apis.Drive.v3.Data.File> DoFileQuery(FilesResource.ListRequest listRequest)
        {
            List<Google.Apis.Drive.v3.Data.File> fileList = new List<Google.Apis.Drive.v3.Data.File>();

            var f = listRequest.Execute();
            while (f.Files != null)
            {
                var files = listRequest.Execute().Files;
                if (files != null && files.Count > 0)
                {
                    fileList.AddRange(files);
                }
                if (f.NextPageToken != null)
                {
                    listRequest.PageToken = f.NextPageToken;
                    f = listRequest.Execute();
                }
                else
                {
                    break;
                }
            }
            return fileList;
        }


    }
}
