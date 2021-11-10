using Airslip.Common.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.IO;

namespace Airslip.Common.Testing
{
    public static class OptionsMock
    {
        public static IConfiguration? InitialiseConfiguration(string projectName)
        {
            string? basePath = GetBasePath(projectName);
            
            if(basePath is null)
                return null;
            
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
        
        public static string? GetBasePath(string projectName)
        {
            string currentDirectory  = Directory.GetCurrentDirectory();
            int index = currentDirectory.IndexOf("src", StringComparison.Ordinal);
            
            if(index is -1)
                return null;
            
            string projectSrcFolder = currentDirectory.Substring(0, currentDirectory.IndexOf("src", StringComparison.Ordinal) + 4);
            return Path.Combine(projectSrcFolder, projectName);
        }

        public static Mock<IOptions<T>>? SetUpOptionSettings<T>(string projectName) where T : class
        {
            Mock<IOptions<T>> mockSettings = new();
            IConfiguration? configuration = InitialiseConfiguration(projectName);
            if (configuration is null)
                return null;
            
            T settings = UtilityExtensions.GetConfigurationSection<T>(configuration);
            
            mockSettings
                .Setup(s => s.Value)
                .Returns(settings);

            return mockSettings;
        }
    }
}