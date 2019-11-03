using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.S3.Model;

namespace FileMonitor
{
    public interface IAWSS3Helper
    {
        void CopyObject(string bucketPath, string newBucketPath);
        void DeleteObject(string bucketPath);
        void DownloadDirectory(string bucketPath, string localPath);
        Task<GetObjectResponse> DownloadObject(string bucketPath);
        void DownloadObject(string bucketPath, string localPath);
        List<string> ListFilesInDirectory(string bucketPath);
        List<S3Object> ListS3ObjectsInRemoteDirectory(string bucketPath);
        void WriteObject(string localPath, string bucketPath);
    }
}