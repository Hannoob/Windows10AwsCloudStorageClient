using Amazon;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMonitor
{
    public interface IConfigHelper
    {
        Configuration GetConfiguration();
        void SaveConfiguration(Configuration configuration);
        void UpdateLastSyncDate();
        DateTime GetLastSyncDate();

    }

    public class ConfigurationHelper : IConfigHelper
    {
        public static string settingsFile = @"appsettings.json";
        public Configuration GetConfiguration()
        {
            using (StreamReader streamReader = new StreamReader(settingsFile))
            {
                string jsonString = streamReader.ReadToEnd();
                var configuration = JsonConvert.DeserializeObject<Configuration>(jsonString);
                return configuration;
            }
        }

        public void SaveConfiguration(Configuration configuration)
        {
            using (StreamWriter file = File.CreateText(settingsFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, configuration);
            }
        }

        public DateTime GetLastSyncDate()
        {
            throw new NotImplementedException();
        }

        public void UpdateLastSyncDate()
        {
            throw new NotImplementedException();
        }
    }

    public class Configuration
    {
        public RegionEndpoint ParsedBucketRegion {
            get { return RegionEndpoint.GetBySystemName(BucketRegion ?? "eu-west-1"); }
        }
        public string BucketRegion { get; set; }
        public string CloudStorageFolderPath { get; set; }
        public string CloudStorageRemoteFolderName { get; set; }
        public string BucketName { get; set; }
        public string AccessKeyId { get; set; }
        public string SecretAccessKey { get; set; }

    }
}
