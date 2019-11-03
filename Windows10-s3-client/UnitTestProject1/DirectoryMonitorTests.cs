using NUnit.Framework;
using System;
using FluentAssertions;
using FileMonitor;
using System.IO;
using Moq;
using System.Collections.Generic;
using TestUtils;

namespace DirectoryMonitorTests
{

    public class When_uploading_a_new_directory
    {
        private Mock<IAWSS3Helper> mockAWSHandler;

        [SetUp]
        public void Setup()
        {
            var mockConfig = MockUtils.getConfig();

            mockAWSHandler = new Mock<IAWSS3Helper>();

            var fileMonitor = new FileMonitor.FileMonitor(mockConfig, mockAWSHandler.Object, MockUtils.getMockDirPathUtils().Object);
            var mockFileSystemEvent = new FileSystemEventArgs(WatcherChangeTypes.Created, "C:\\some_dir", "");
            fileMonitor.created(null, mockFileSystemEvent);
        }

        [Test]
        [Description("Then the AWS helper should NOT be called")]
        public void Then_the_AWS_helper_should_NOT_be_called()
        {
            mockAWSHandler.Verify(t => t.WriteObject(It.IsAny<string>(),It.IsAny<string>()), Times.Never);
        }
    }

    public class When_deleting_a_file
    {
        private Mock<IAWSS3Helper> mockAWSHandler;

        [SetUp]
        public void Setup()
        {
            var mockConfig = MockUtils.getConfig();

            mockAWSHandler = new Mock<IAWSS3Helper>();
            var mockFileList = new List<string>()
            {
                @"some_dir/file1",
                @"some_dir/file2",
                @"some_dir/file3",
            };
            mockAWSHandler.Setup(x => x.ListFilesInDirectory(It.IsAny<string>())).Returns(mockFileList);

            var fileMonitor = new FileMonitor.FileMonitor(mockConfig, mockAWSHandler.Object, MockUtils.getMockFilePathUtils().Object);
            var mockFileSystemEvent = new FileSystemEventArgs(WatcherChangeTypes.Deleted, @"C:\", "some_dir");
            fileMonitor.deleted(null, mockFileSystemEvent);
        }

        [Test]
        [Description("Then the AWS helper should be called with the correct params")]
        public void Then_the_AWS_helper_should_be_called_with_the_correct_params()
        {
            mockAWSHandler.Verify(t => t.DeleteObject(@"some_dir/file1"));
            mockAWSHandler.Verify(t => t.DeleteObject(@"some_dir/file2"));
            mockAWSHandler.Verify(t => t.DeleteObject(@"some_dir/file3"));
        }
    }

    public class When_renaming_a_file
    {
        private Mock<IAWSS3Helper> mockAWSHandler;

        [SetUp]
        public void Setup()
        {
            var mockConfig = MockUtils.getConfig();

            mockAWSHandler = new Mock<IAWSS3Helper>();

            var mockFileUtils = MockUtils.getMockDirPathUtils();
            var mockFileList = new List<string>()
            {
                @"C:\new_dir\file1",
                @"C:\new_dir\file2",
                @"C:\new_dir\file3",
            };
            mockFileUtils.Setup(x => x.getAllNestedLocalFiles(It.IsAny<string>())).Returns(mockFileList);

            var fileMonitor = new FileMonitor.FileMonitor(mockConfig, mockAWSHandler.Object, mockFileUtils.Object);
            var mockFileSystemEvent = new RenamedEventArgs(WatcherChangeTypes.Renamed, @"C:\", "new_dir", "old_dir");
            fileMonitor.renamed(null, mockFileSystemEvent);
        }

        [Test]
        [Description("Then the AWS helper should be called to copy the old files to the new directory")]
        public void Then_the_AWS_helper_copy_the_old_file_to_the_new_name()
        {
            mockAWSHandler.Verify(t => t.CopyObject(@"old_dir/file1", @"new_dir/file1"));
            mockAWSHandler.Verify(t => t.CopyObject(@"old_dir/file2", @"new_dir/file2"));
            mockAWSHandler.Verify(t => t.CopyObject(@"old_dir/file3", @"new_dir/file3"));
        }

        [Test]
        [Description("Then the AWS helper should be called to delete the old files")]
        public void Then_the_AWS_helper_should_be_called_to_delete_the_old_file()
        {
            mockAWSHandler.Verify(t => t.DeleteObject(@"old_dir/file1"));
            mockAWSHandler.Verify(t => t.DeleteObject(@"old_dir/file2"));
            mockAWSHandler.Verify(t => t.DeleteObject(@"old_dir/file3"));
        }
    }

    public class When_changing_a_file
    {
        private Mock<IAWSS3Helper> mockAWSHandler;

        [SetUp]
        public void Setup()
        {
            var mockConfig = MockUtils.getConfig();

            mockAWSHandler = new Mock<IAWSS3Helper>();
            mockAWSHandler.Setup(x => x.ListFilesInDirectory(It.IsAny<string>())).Returns(new List<string>());

            var fileMonitor = new FileMonitor.FileMonitor(mockConfig, mockAWSHandler.Object, MockUtils.getMockDirPathUtils().Object);
            var mockFileSystemEvent = new FileSystemEventArgs(WatcherChangeTypes.Changed, "C:\\some_dir", "some_File");
            fileMonitor.changed(null, mockFileSystemEvent);
        }

        [Test]
        [Description("Then the AWS helper should NOT be called")]
        public void Then_the_AWS_helper_should_NOT_be_called()
        {
            mockAWSHandler.Verify(t => t.WriteObject(It.IsAny<string>(),It.IsAny<string>()), Times.Never);
        }
    }
}
