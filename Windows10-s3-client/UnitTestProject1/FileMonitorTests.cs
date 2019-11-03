using NUnit.Framework;
using System;
using FluentAssertions;
using FileMonitor;
using System.IO;
using Moq;
using System.Collections.Generic;
using TestUtils;

namespace FileMonitorTests
{
    public class When_uploading_a_new_file
    {
        private Mock<IAWSS3Helper> mockAWSHandler;

        [SetUp]
        public void Setup()
        {
            var mockConfig = MockUtils.getConfig();

            mockAWSHandler = new Mock<IAWSS3Helper>();

            var fileMonitor = new FileMonitor.FileMonitor(mockConfig, mockAWSHandler.Object, MockUtils.getMockFilePathUtils().Object);
            var mockFileSystemEvent = new FileSystemEventArgs(WatcherChangeTypes.Created, "C:\\some_dir", "some_File");
            fileMonitor.created(null, mockFileSystemEvent);
        }

        [Test]
        [Description("Then the AWS helper should be called with the correct params")]
        public void Then_the_AWS_helper_should_be_called_with_the_correct_params()
        {
            mockAWSHandler.Verify(t => t.WriteObject(@"C:\some_dir\some_File", @"some_dir/some_File"));
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
            mockAWSHandler.Setup(x => x.ListFilesInDirectory(It.IsAny<string>())).Returns(new List<string>());

            var fileMonitor = new FileMonitor.FileMonitor(mockConfig, mockAWSHandler.Object, MockUtils.getMockFilePathUtils().Object);
            var mockFileSystemEvent = new FileSystemEventArgs(WatcherChangeTypes.Deleted, "C:\\some_dir", "some_File");
            fileMonitor.deleted(null, mockFileSystemEvent);
        }

        [Test]
        [Description("Then the AWS helper should be called with the correct params")]
        public void Then_the_AWS_helper_should_be_called_with_the_correct_params()
        {
            mockAWSHandler.Verify(t => t.DeleteObject(@"some_dir/some_File"));
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
            mockAWSHandler.Setup(x => x.ListFilesInDirectory(It.IsAny<string>())).Returns(new List<string>());

            var fileMonitor = new FileMonitor.FileMonitor(mockConfig, mockAWSHandler.Object, MockUtils.getMockFilePathUtils().Object);
            var mockFileSystemEvent = new RenamedEventArgs(WatcherChangeTypes.Renamed, "C:\\some_dir", "new_name", "old_name");
            fileMonitor.renamed(null, mockFileSystemEvent);
        }

        [Test]
        [Description("Then the AWS helper should be called to copy the old file to the new name")]
        public void Then_the_AWS_helper_copy_the_old_file_to_the_new_name()
        {
            mockAWSHandler.Verify(t => t.CopyObject(@"some_dir/old_name", @"some_dir/new_name"));
        }

        [Test]
        [Description("Then the AWS helper should be called to delete the old file")]
        public void Then_the_AWS_helper_should_be_called_to_delete_the_old_file()
        {
            mockAWSHandler.Verify(t => t.DeleteObject(@"some_dir/old_name"));
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

            var fileMonitor = new FileMonitor.FileMonitor(mockConfig, mockAWSHandler.Object, MockUtils.getMockFilePathUtils().Object);
            var mockFileSystemEvent = new FileSystemEventArgs(WatcherChangeTypes.Changed, "C:\\some_dir", "some_File");
            fileMonitor.changed(null, mockFileSystemEvent);
        }

        [Test]
        [Description("Then the AWS helper should be called with the correct params")]
        public void Then_the_AWS_helper_should_be_called_with_the_correct_params()
        {
            mockAWSHandler.Verify(t => t.WriteObject(@"C:\some_dir\some_File", @"some_dir/some_File"));
        }
    }
}
