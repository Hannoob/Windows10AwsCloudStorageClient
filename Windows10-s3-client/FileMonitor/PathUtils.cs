using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMonitor
{
    public class PathUtils : IFileSystemHelper
    {
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
            var relativeUri = sourcePath.MakeRelativeUri(destinationPath);

            return DecodeUrlString(relativeUri.ToString().TrimStart('/'));
        }

        public List<string> getAllNestedLocalFiles(string directory)
        {
            var files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            return new List<string>(files);
        }

        public bool isDirectory(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return attr.HasFlag(FileAttributes.Directory);
        }

        public void DeleteLocalFile(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteLocalDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public void RenameLocalFile(string oldPath, string newPath)
        {
            throw new NotImplementedException();
        }

        public void RenameLocalDirectory(string oldPath, string newPath)
        {
            throw new NotImplementedException();
        }
    }
}
