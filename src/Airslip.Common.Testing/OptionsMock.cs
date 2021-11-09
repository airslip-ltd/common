using Airslip.Common.Types;
using Airslip.Common.Utilities.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.IO;

namespace Airslip.Common.Testing
{
    public static class OptionsMock
    {
        public static IConfiguration InitialiseConfiguration(string projectName)
        {
            string basePath = GetBasePath(projectName);
            
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
        
        public static string GetBasePath(string projectName)
        {
            string currentDirectory  = Directory.GetCurrentDirectory();
            string projectSrcFolder = currentDirectory.Substring(0, currentDirectory.IndexOf(projectName, StringComparison.Ordinal));
            return Path.Combine(projectSrcFolder, projectName);
        }

        public static Mock<IOptions<T>> SetUpOptionSettings<T>(string projectName) where T : class
        {
            Mock<IOptions<T>> mockSettings = new();
            string basePath = GetBasePath(projectName);
            IConfiguration configuration = InitialiseConfiguration(basePath);
            T settings = UtilityExtensions.GetConfigurationSection<T>(configuration);
            
            mockSettings
                .Setup(s => s.Value)
                .Returns(settings);

            return mockSettings;
        }
    }
}