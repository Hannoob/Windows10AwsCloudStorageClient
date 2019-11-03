using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileMonitor
{
    public class FileMonitor
    {
        private int _changeBuffer = 6000; //The buffer built in for changes on the remote server in miliseconds.

        private IAWSS3Helper _awsS3Helper;
        private FileSystemWatcher _watcher;
        private Configuration _configuration;
        private IFileSystemHelper _fileSystemHelper;

        public FileMonitor(Configuration configuration, IAWSS3Helper awsS3Helper, IFileSystemHelper pathUtils)
        {
            _configuration = configuration;
            _awsS3Helper = awsS3Helper;
            _fileSystemHelper = pathUtils;

            _watcher = new FileSystemWatcher(_configuration.CloudStorageFolderPath);
            _watcher.IncludeSubdirectories = true;
            _watcher.Filter = "";
            _watcher.Renamed += new RenamedEventHandler(renamed);
            _watcher.Deleted += new FileSystemEventHandler(deleted);
            _watcher.Changed += new FileSystemEventHandler(changed);
            _watcher.Created += new FileSystemEventHandler(created);
        }

        public FileMonitor(Configuration configuration) : this(configuration, new AWSS3Helper(configuration), new PathUtils())
        {
        }

        public void StartMonitoring()
        {
            _watcher.EnableRaisingEvents = true;
        }

        public void StopMonitoring()
        {
            _watcher.EnableRaisingEvents = false;
        }

        public void renamed(object sender, RenamedEventArgs e)
        {
            if (!_fileSystemHelper.isDirectory(e.FullPath))
            {
                renameFile(e);
                Console.WriteLine("File renamed!");
            }
            else
            {
                var files = _fileSystemHelper.getAllNestedLocalFiles(e.FullPath);
                if (files.Count > 0)
                {
                    files.ForEach(fileName => renameFile(fileName, e.OldFullPath, e.FullPath));
                    Console.WriteLine("Directory renamed!");
                }
                else
                    Console.WriteLine("Empty Directory, no action");
            }
        }

        public void changed(object sender, FileSystemEventArgs e)
        {
            if (!_fileSystemHelper.isDirectory(e.FullPath))
            {
                uploadFile(e.FullPath);
                Console.WriteLine("File Updated!");
            }
            else
                Console.WriteLine("Directory changed, ignoring changes");
        }

        public void deleted(object sender, FileSystemEventArgs e)
        {
            deleteFolder(e.FullPath);

        }

        public void created(object sender, FileSystemEventArgs e)
        {
            if (!_fileSystemHelper.isDirectory(e.FullPath))
            {
                uploadFile(e.FullPath);
                Console.WriteLine("File Uploaded!");
            }
            else
            {
                //var files = getAllNestedFiles(e.FullPath);
                //if (files.Count > 0)
                //    files.ForEach(fileName => uploadFile(fileName));
                //else
                //    Console.WriteLine("Directory created, no subfolders");
                Console.WriteLine("Directory created, ignore");

            }
        }

        public void refresh()
        {
            StopMonitoring();
            SyncToRemote();
            SyncFromRemote();
            StartMonitoring();
        }


        public void renameFile(RenamedEventArgs e)
        {
            var oldPath = getRemotePath(_configuration.CloudStorageFolderPath, e.OldFullPath);
            var newPath = getRemotePath(_configuration.CloudStorageFolderPath, e.FullPath);
            _awsS3Helper.CopyObject(oldPath, newPath);
            _awsS3Helper.DeleteObject(oldPath);
        }

        public void renameFile(string fileName, string oldFolderPath, string newFolderPath)
        {
            var oldFileName = fileName.Replace(newFolderPath, oldFolderPath);
            var oldRemotePath = getRemotePath(_configuration.CloudStorageFolderPath, oldFileName);
            var newRemotePath = getRemotePath(_configuration.CloudStorageFolderPath, fileName);
            _awsS3Helper.CopyObject(oldRemotePath, newRemotePath);
            _awsS3Helper.DeleteObject(oldRemotePath);
        }

        public void deleteFile(string localPath)
        {
            var remotePath = getRemotePath(_configuration.CloudStorageFolderPath, localPath);
            _awsS3Helper.DeleteObject(remotePath);
        }

        public void deleteFolder(string localPath)
        {
            var remotePath = getRemotePath(_configuration.CloudStorageFolderPath, localPath);
            var remoteFiles = _awsS3Helper.ListFilesInDirectory(remotePath);
            if (remoteFiles.Count > 0)
            {

                remoteFiles.ForEach(filePath => _awsS3Helper.DeleteObject(filePath));
                Console.WriteLine("Directory Deleted!");
            }
            else
            {
                _awsS3Helper.DeleteObject(remotePath);
                Console.WriteLine("File Deleted!");
            }

        }

        public void uploadFile(string localPath)
        {
            var remotePath = getRemotePath(_configuration.CloudStorageFolderPath, localPath);
            _awsS3Helper.WriteObject(localPath, remotePath);
        }



        //Utils
        public string DecodeUrlString(string url)
        {
            string newUrl;
            while ((newUrl = Uri.UnescapeDataString(url)) != url)
                url = newUrl;
            return newUrl;
        }

        public string getRemotePath(string localBasePath, string localEventPath)
        {
            var sourcePath = new Uri(Path.GetDirectoryName(localBasePath));
            var destinationPath = new Uri(localEventPath);
            var relativeUri = $"{_configuration.CloudStorageRemoteFolderName}/{sourcePath.MakeRelativeUri(destinationPath)}";

            return DecodeUrlString(relativeUri.ToString().TrimStart('/'));
        }

        public List<(string, DateTime)> CalculateRemoteDelta()//Calculate remote changes that need to be synced to local
        {
            var localFiles = _fileSystemHelper.getAllNestedLocalFiles(_configuration.CloudStorageFolderPath);

            var remotePath = getRemotePath(_configuration.CloudStorageFolderPath, _configuration.CloudStorageFolderPath);
            var remoteFiles = _awsS3Helper.ListS3ObjectsInRemoteDirectory(remotePath);

            var localFileNamesAndChanges = localFiles.Select(fileName => (fileName, File.GetLastWriteTime(fileName)));

            var remoteFilesAndChanges = remoteFiles.Select(s3Object => (s3Object.Key, s3Object.LastModified));


            var tempRemotePathOfLocalFiles = localFileNamesAndChanges
                .Select(nameDate => (getRemotePath(_configuration.CloudStorageFolderPath, nameDate.Item1), nameDate.Item2))
                .ToList();

            //Changes on the remoteServer that needs to be synced to local
            var remoteDelta = remoteFilesAndChanges.Where(nameDate =>
            {
                //REmove Directories
                if (nameDate.Item1.EndsWith("/"))
                    return false;

                var (localFileName, localFileDate) = tempRemotePathOfLocalFiles.FirstOrDefault(fileDate => fileDate.Item1 == nameDate.Item1);
                if (String.IsNullOrWhiteSpace(localFileName) || localFileDate.ToUniversalTime().Ticks + _changeBuffer < (nameDate.Item2.ToUniversalTime().Ticks))
                    return true;
                return false;
            })
            //.Select(nameDate => nameDate.Item1)
            .ToList();

            return remoteDelta;
        }

        public List<(string, DateTime)> CalculateLocalDelta()//Calculate local files to be synced to remote
        {
            var localFiles = _fileSystemHelper.getAllNestedLocalFiles(_configuration.CloudStorageFolderPath);

            var remotePath = getRemotePath(_configuration.CloudStorageFolderPath, _configuration.CloudStorageFolderPath);
            var remoteFiles = _awsS3Helper.ListS3ObjectsInRemoteDirectory(remotePath);

            var localFileNamesAndChanges = localFiles.Select(fileName =>
            {
                var modifiedDate = File.GetLastWriteTime(fileName);
                return (fileName, modifiedDate);
            });

            var remoteFilesAndChanges = remoteFiles.Select(s3Object =>
            {
                return (s3Object.Key, s3Object.LastModified);
            });


            //Changes on the local that needs to be synced to remote
            var localDelta = localFileNamesAndChanges.Where(localNameDate =>
            {
                var (remoteFileName, remoteDate) = remoteFilesAndChanges.FirstOrDefault(remoteFileDate => remoteFileDate.Item1 == getRemotePath(_configuration.CloudStorageFolderPath, localNameDate.Item1));
                if (String.IsNullOrWhiteSpace(remoteFileName) || localNameDate.Item2.ToUniversalTime().Ticks > (remoteDate.ToUniversalTime().Ticks + _changeBuffer))
                    return true;
                return false;
            })
            .ToList();

            return localDelta;
        }

        public void SyncFromRemote()
        {
            var remoteFiles = CalculateRemoteDelta();
            Console.WriteLine($"{remoteFiles.Count} files have been changed on the remote branch and needs to be synced to the local system.");
            var parent = Directory.GetParent(_configuration.CloudStorageFolderPath).FullName;
            remoteFiles.ForEach(file => {
                _awsS3Helper.DownloadObject(file.Item1, parent);
                File.SetLastWriteTimeUtc(parent + "\\" + file.Item1, file.Item2);
            });
        }

        public void SyncToRemote()
        {
            var localFiles = CalculateLocalDelta();
            Console.WriteLine($"{localFiles.Count} files have been changed on the local branch and needs to be synced to the remote server.");
            localFiles.ForEach(file =>
            {
                _awsS3Helper.WriteObject(file.Item1, getRemotePath(_configuration.CloudStorageFolderPath, file.Item1));
                File.SetLastWriteTimeUtc(file.Item1, DateTime.UtcNow);
            });
        }
    }
}
