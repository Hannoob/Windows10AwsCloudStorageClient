using System.Collections.Generic;

namespace FileMonitor
{
    public interface IFileSystemHelper
    {
        string DecodeUrlString(string url);
        List<string> getAllNestedLocalFiles(string directory);
        string getRemotePath(string localBasePath, string localEventPath);
        bool isDirectory(string path);

        void DeleteLocalFile(string path);
        void DeleteLocalDirectory(string path);
        void RenameLocalFile(string oldPath, string newPath);
        void RenameLocalDirectory(string oldPath, string newPath);
    }
}