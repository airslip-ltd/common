using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Airslip.Common.Services.MongoDb.Tests
{
    public static class Helpers
    {
        public static IConfiguration InitialiseConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public static T GetConfigurationSection<T>(IConfiguration configuration)
        {
            string className = typeof(T).Name;
            IConfigurationSection configurationSection = configuration.GetSection(className);
            return configurationSection.Get<T>();
        }
    }
}