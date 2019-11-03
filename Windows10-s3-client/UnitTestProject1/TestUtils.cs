using FileMonitor;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtils
{
    public class MockUtils
    {
        public static Configuration getConfig()
        {
            return new Configuration()
            {
                Username = "asdf",
                BucketName = "adsf",
                BucketRegion = "eu-west-1",
                CloudStorageFolderPath = @"C:\\CloudStorageFolder",
                CloudStorageRemoteFolderName = "asdf",
                Password = "asdf"
            };
        }

        public static Mock<IFileSystemHelper> getMockFilePathUtils()
        {
            var mockPathUtils = new Mock<IFileSystemHelper>();
            mockPathUtils.Setup(x => x.isDirectory(It.IsAny<string>())).Returns(false);
            return mockPathUtils;
        }

        public static Mock<IFileSystemHelper> getMockDirPathUtils()
        {
            var mockPathUtils = new Mock<IFileSystemHelper>();
            mockPathUtils.Setup(x => x.isDirectory(It.IsAny<string>())).Returns(true);
            return mockPathUtils;
        }
    }
}
