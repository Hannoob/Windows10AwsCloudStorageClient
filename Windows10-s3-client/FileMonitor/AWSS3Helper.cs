using System;
using System.Collections.Generic;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using System.Threading.Tasks;
using Amazon;
using System.IO;
using System.Net;
using Amazon.S3.Transfer;
using Amazon.Runtime;
using System.Linq;
using System.Threading;
using Amazon.CognitoIdentity;

namespace FileMonitor
{
    //https://docs.aws.amazon.com/AmazonS3/latest/dev/RESTAuthentication.html
    //https://docs.aws.amazon.com/IAM/latest/UserGuide/reference_policies_examples_s3_cognito-bucket.html
//    {
//    "Version": "2012-10-17",
//    "Statement": [
//        {
//            "Sid": "ListYourObjects",
//            "Effect": "Allow",
//            "Action": "s3:ListBucket",
//            "Resource": ["arn:aws:s3:::bucket-name"],
//            "Condition": {
//                "StringLike": {
//                    "s3:prefix": ["cognito/application-name/${cognito-identity.amazonaws.com:sub}"]
//}
//            }
//        },
//        {
//            "Sid": "ReadWriteDeleteYourObjects",
//            "Effect": "Allow",
//            "Action": [
//                "s3:GetObject",
//                "s3:PutObject",
//                "s3:DeleteObject"
//            ],
//            "Resource": [
//                "arn:aws:s3:::bucket-name/cognito/application-name/${cognito-identity.amazonaws.com:sub}",
//                "arn:aws:s3:::bucket-name/cognito/application-name/${cognito-identity.amazonaws.com:sub}/*"
//            ]
//        }
//    ]
//}
    public class AWSS3Helper : IAWSS3Helper
    {
        private string _bucketName;
        private readonly RegionEndpoint _bucketRegion;
        //private Configuration _configuration;

        private IAmazonS3 _client;

        public AWSS3Helper(Configuration configuration)
        {
            //_configuration = configuration;
            _bucketName = configuration.BucketName;
            _bucketRegion = configuration.ParsedBucketRegion;
            var _authenticationHelper = new AuthenticationHelper(configuration);

            var creds = new BasicAWSCredentials(configuration.AccessKeyId, configuration.SecretAccessKey);

            //https://aws.amazon.com/blogs/developer/cognitoauthentication-extension-library-developer-preview/
            _client = new AmazonS3Client(creds,_bucketRegion);
        }

        public List<string> ListFilesInDirectory(String bucketPath)
        {
            return ListS3ObjectsInRemoteDirectory(bucketPath).Select(obj => obj.Key).ToList();
        }

        public List<S3Object> ListS3ObjectsInRemoteDirectory(String bucketPath)
        {
            ListObjectsV2Request listRequest = new ListObjectsV2Request()
            {
                BucketName = _bucketName,
                Prefix = bucketPath + "/",
                MaxKeys = int.MaxValue
            };

            // get all objects inside the "folder"
            ListObjectsV2Response objects = _client.ListObjectsV2Async(listRequest).Result;
            return objects.S3Objects;
        }

        public void WriteObject(String localPath, String bucketPath)
        {
            try
            {
                TransferUtility utility = new TransferUtility(_client);
                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest()
                {
                    BucketName = _bucketName,
                    Key = bucketPath,
                    FilePath = localPath
                };

                utility.Upload(request); 
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred with Message:'{0}' when writing an object", e.Message);
            }
        }

        public void CopyObject(String bucketPath, String newBucketPath)
        {
            try
            {
                CopyObjectRequest request = new CopyObjectRequest
                {
                    SourceBucket = _bucketName, 
                    SourceKey = bucketPath,
                    DestinationBucket = _bucketName,
                    DestinationKey = newBucketPath
                };
                CopyObjectResponse response = _client.CopyObjectAsync(request).Result;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when copying an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when copying an object", e.Message);
            }
        }

        public void DeleteObject(String bucketPath)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = bucketPath
                };

                var response = _client.DeleteObjectAsync(deleteObjectRequest).Result;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }


        public async Task<GetObjectResponse> DownloadObject(string bucketPath)
        {
            try
            {
                GetObjectRequest request1 = new GetObjectRequest()
                {
                    BucketName = _bucketName,
                    Key = bucketPath
                    
                };

                GetObjectResponse Response = await _client.GetObjectAsync(request1);
                return Response;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred with Message:'{0}' when downloading an object", e.Message);
                return null;
            }
        }

        public void DownloadObject(string bucketPath, string localPath)
        {
            var obj = DownloadObject(bucketPath).Result;
            CancellationToken ct = default(CancellationToken);
            obj.WriteResponseStreamToFileAsync(localPath + "\\" + bucketPath, true, ct).Wait();
        }

        public void DownloadDirectory(string bucketPath, string localPath)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls | SecurityProtocolType.Tls;

            var files = ListFilesInDirectory(bucketPath);

            CancellationToken ct = default(CancellationToken);
            files.ForEach(async file => 
            {
                var downloadResult = await DownloadObject(file);
                //using (Stream responseStream = downloadResult.ResponseStream)
                //{
                await downloadResult.WriteResponseStreamToFileAsync(localPath + "\\" + file, true, ct);
                //}

            });
        }

    }
}