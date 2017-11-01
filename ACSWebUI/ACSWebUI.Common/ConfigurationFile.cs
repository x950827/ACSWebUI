using System;
using System.IO;
using System.Reflection;
using ACSWebUI.Common.Extensions;

namespace ACSWebUI.Common {
    public abstract class ConfigurationFile<TConfiguration> where TConfiguration : ConfigurationFile<TConfiguration>, new() {
        protected abstract string ConfigurationFileName { get; }
        private string configurationDirectory;

        public static TConfiguration ReadConfiguration(string directory = "settings") {
            var configurationFileName = Path.Combine(directory, new TConfiguration().ConfigurationFileName);
            var configurationFileDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            while (!string.IsNullOrEmpty(configurationFileDirectory) && !File.Exists(Path.Combine(configurationFileDirectory, configurationFileName)))
                configurationFileDirectory = Path.GetDirectoryName(configurationFileDirectory);

            if (string.IsNullOrEmpty(configurationFileDirectory))
                return new TConfiguration { configurationDirectory = directory };

            try {
                var configuration = File.ReadAllBytes(Path.Combine(configurationFileDirectory, configurationFileName)).FromXml<TConfiguration>();
                configuration.configurationDirectory = Path.Combine(configurationFileDirectory, directory);
                return configuration;
            }
            catch (Exception) {
                return new TConfiguration { configurationDirectory = directory };
            }
        }

        public static TConfiguration ReadBackups(string directory = "Backup") {
            var configurationFileName = Path.Combine(directory, new TConfiguration().ConfigurationFileName);
            var configurationFileDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            while (!string.IsNullOrEmpty(configurationFileDirectory) && !File.Exists(Path.Combine(configurationFileDirectory, configurationFileName)))
                configurationFileDirectory = Path.GetDirectoryName(configurationFileDirectory);

            if (string.IsNullOrEmpty(configurationFileDirectory))
                return new TConfiguration { configurationDirectory = directory };

            try {
                var configuration = File.ReadAllBytes(Path.Combine(configurationFileDirectory, configurationFileName)).FromXml<TConfiguration>();
                configuration.configurationDirectory = Path.Combine(configurationFileDirectory, directory);
                return configuration;
            }
            catch (Exception) {
                return new TConfiguration { configurationDirectory = directory };
            }
        }

        public void WriteConfiguration() {
            if (!Directory.Exists(configurationDirectory))
                Directory.CreateDirectory(configurationDirectory);

            File.WriteAllBytes(Path.Combine(configurationDirectory, ConfigurationFileName), this.ToXml());
        }
    }
}