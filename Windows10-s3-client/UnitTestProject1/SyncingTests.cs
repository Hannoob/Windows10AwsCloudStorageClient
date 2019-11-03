using FileMonitor;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace SyncTests
{
    //Special cases
    //If a file is deleted and later added with the same name, it should be added to local
    //If a file is renamed and then deleted, the local version with the old name should be deleted

    public class When_syncing_from_remote
    {
        private Mock<IFileSystemHelper> mockFileSystemHelper;
        private Mock<IAWSS3Helper> mockAwsS3Helper;
        private Mock<IConfigHelper> mockConfigHelper;

        [OneTimeSetUp]
        public void Setup()
        {
            var remoteChanges = new List<(DateTime, string, string, string)>()
            {
                (DateTime.UtcNow.AddDays(-2),"remote/old-file","delete",""),

                (DateTime.UtcNow,"remote/delete-file","delete",""),
                (DateTime.UtcNow,"remote/delete-directory","delete",""),

                (DateTime.UtcNow,"remote/add-file","add",""),

                (DateTime.UtcNow,"remote/rename-file","rename","remote/rename-file-newname"),
                (DateTime.UtcNow,"remote/rename-directory","rename","remote/rename-directory-newname"),

                (DateTime.UtcNow,"remote/change-file","change","")
            };

            var lastSyncToLocal = DateTime.UtcNow.AddDays(-2);

            var fileMonitor = new FileMonitor.FileMonitor(MockUtils.getConfig(), mockAwsS3Helper.Object, mockFileSystemHelper.Object);
            fileMonitor.SyncFromRemote();
        }

        [Test]
        public void Then_the_old_remote_changes_should_be_ignored()
        {
            mockAwsS3Helper.Verify(x => x.DownloadObject("remote/old-file"), Times.Never);
        }

        [Test]
        public void Then_the_new_remote_file_should_be_downloaded()
        {
            mockAwsS3Helper.Verify(x => x.DownloadObject("remote/add-file"));
        }

        [Test]
        public void Then_the_changed_remote_file_should_be_downloaded()
        {
            mockAwsS3Helper.Verify(x => x.DownloadObject("remote/change-file"));
        }

        [Test]
        public void Then_the_deleted_remote_file_should_be_deleted()
        {
            mockFileSystemHelper.Verify(x => x.DeleteLocalFile("local/delete-file"));
        }

        [Test]
        public void Then_the_deleted_remote_folder_should_be_deleted_locally()
        {
            mockFileSystemHelper.Verify(x => x.DeleteLocalDirectory("local/delete-directory"));
        }

        [Test]
        public void Then_the_renamed_remote_file_should_be_renamed()
        {
            mockFileSystemHelper.Verify(x => x.RenameLocalFile("local/rename-file", "local/rename-file-newname"));
        }

        [Test]
        public void Then_the_renamed_remote_directory_should_be_renamed_locally()
        {
            mockFileSystemHelper.Verify(x => x.RenameLocalDirectory("local/rename-directory", "local/rename-directory-newname"));
        }

        [Test]
        public void Then_local_last_sync_file_should_be_reset()
        {
            mockConfigHelper.Verify(x => x.UpdateLastSyncDate());
        }

        [Test]
        public void Then_local_last_sync_file_should_be_read()
        {
            mockConfigHelper.Verify(x => x.GetLastSyncDate());
        }

    }

    public class When_syncing_from_remote_with_multiple_changes_to_a_file
    {
        private Mock<IFileSystemHelper> mockFileSystemHelper;
        private Mock<IAWSS3Helper> mockAwsS3Helper;

        [OneTimeSetUp]
        public void Setup()
        {
            var remoteChanges = new List<(DateTime, string, string, string)>()
            {
                (DateTime.UtcNow,"remote/change","change",""),
                (DateTime.UtcNow,"remote/change","change",""),
                (DateTime.UtcNow,"remote/change","change",""),
                (DateTime.UtcNow,"remote/change","change","")
            };

            var lastSyncToLocal = DateTime.UtcNow.AddDays(-2);

            var fileMonitor = new FileMonitor.FileMonitor(MockUtils.getConfig(), mockAwsS3Helper.Object, mockFileSystemHelper.Object);
            fileMonitor.SyncFromRemote();
        }

        public void Then_the_file_should_only_be_downloaded_once()
        {
            mockAwsS3Helper.Verify(x => x.DownloadObject("remote/change"), Times.Once);
        }
    }

    public class When_syncing_from_remote_with_an_uploaded_and_deleted_file
    {
        private Mock<IFileSystemHelper> mockFileSystemHelper;
        private Mock<IAWSS3Helper> mockAwsS3Helper;

        [OneTimeSetUp]
        public void Setup()
        {
            var remoteChanges = new List<(DateTime, string, string, string)>()
            {
                (DateTime.UtcNow,"remote/file","add",""),
                (DateTime.UtcNow,"remote/file","change",""),
                (DateTime.UtcNow,"remote/file","change",""),
                (DateTime.UtcNow,"remote/file","delete","")
            };

            var lastSyncToLocal = DateTime.UtcNow.AddDays(-2);

            var fileMonitor = new FileMonitor.FileMonitor(MockUtils.getConfig(), mockAwsS3Helper.Object, mockFileSystemHelper.Object);
            fileMonitor.SyncFromRemote();
        }

        public void Then_the_file_not_be_downloaded()
        {
            mockAwsS3Helper.Verify(x => x.DownloadObject(It.IsAny<string>()), Times.Never);
        }
    }

    public class When_syncing_from_remote_with_an_uploaded_and_renamed_file
    {
        private Mock<IFileSystemHelper> mockFileSystemHelper;
        private Mock<IAWSS3Helper> mockAwsS3Helper;

        [OneTimeSetUp]
        public void Setup()
        {
            var remoteChanges = new List<(DateTime, string, string, string)>()
            {
                (DateTime.UtcNow,"remote/file","add",""),
                (DateTime.UtcNow,"remote/file","change",""),
                (DateTime.UtcNow,"remote/file","change",""),
                (DateTime.UtcNow,"remote/file","rename","remote/file2")
            };

            var lastSyncToLocal = DateTime.UtcNow.AddDays(-2);

            var fileMonitor = new FileMonitor.FileMonitor(MockUtils.getConfig(), mockAwsS3Helper.Object, mockFileSystemHelper.Object);
            fileMonitor.SyncFromRemote();
        }

        public void Then_the_file_download_with_the_correct_name()
        {
            mockAwsS3Helper.Verify(x => x.DownloadObject("remote/file2"), Times.Once);
        }

        public void Then_the_old_file_should_not_be_downloaded()
        {
            mockAwsS3Helper.Verify(x => x.DownloadObject("remote/file"), Times.Never);
        }
    }

    public class When_syncing_from_remote_with_a_renamed_and_deleted_file
    {
        private Mock<IFileSystemHelper> mockFileSystemHelper;
        private Mock<IAWSS3Helper> mockAwsS3Helper;
        
        [OneTimeSetUp]
        public void Setup()
        {
            var remoteChanges = new List<(DateTime, string, string, string)>()
            {
                (DateTime.UtcNow,"remote/file","change",""),
                (DateTime.UtcNow,"remote/file","change",""),
                (DateTime.UtcNow,"remote/file","rename","remote/file2"),
                (DateTime.UtcNow,"remote/file2","delete","")
            };

            var lastSyncToLocal = DateTime.UtcNow.AddDays(-2);

            var fileMonitor = new FileMonitor.FileMonitor(MockUtils.getConfig(), mockAwsS3Helper.Object, mockFileSystemHelper.Object);
            fileMonitor.SyncFromRemote();
        }

        public void Then_the_file_file_with_the_old_name_should_be_deleted()
        {
            mockFileSystemHelper.Verify(x => x.DeleteLocalFile("remote/file"), Times.Once);
        }

        public void Then_the_old_file_should_not_be_downloaded()
        {
            mockAwsS3Helper.Verify(x => x.DownloadObject("remote/file"), Times.Never);
        }
    }

    public class When_syncing_to_remote
    {
        private Mock<IFileSystemHelper> mockFileSystemHelper;
        private Mock<IAWSS3Helper> mockAwsS3Helper;

        [SetUp]
        public void Setup()
        {
            var localChanges = new List<(DateTime, string, string, string)>()
            {
                (DateTime.UtcNow,"remote/delete-file","delete",""),
                (DateTime.UtcNow,"remote/delete-directory","delete",""),
                (DateTime.UtcNow,"local/add-file","add",""),
                (DateTime.UtcNow,"remote/rename-file","rename","remote/rename-file-newname"),
                (DateTime.UtcNow,"local/change-file","change","")
            };

            var lastSyncToLocal = DateTime.UtcNow.AddDays(-2);

            var fileMonitor = new FileMonitor.FileMonitor(MockUtils.getConfig(), mockAwsS3Helper.Object, mockFileSystemHelper.Object);
            fileMonitor.SyncFromRemote();
        }

        [Test]
        public void Then_the_old_local_changes_should_be_ignored()
        {
            Assert.Fail("not built");
        }

        [Test]
        public void Then_the_new_local_file_should_be_uploaded()
        {
            Assert.Fail("not built");
        }

        [Test]
        public void Then_the_changed_local_file_should_be_uploaded()
        {
            Assert.Fail("not built");
        }

        [Test]
        public void Then_the_locally_deleted_file_should_be_deleted()
        {
            Assert.Fail("not built");
        }

        [Test]
        public void Then_the_locally_renamed_file_should_be_renamed()
        {
            Assert.Fail("not built");
        }

        [Test]
        public void Then_local_changes_file_should_be_cleared()
        {
            Assert.Fail("not built");
        }

    }

    //How to handle conflics? We don't, simply always get remote when connecting if we cannot. we display a warning that changes will not be synced
}
