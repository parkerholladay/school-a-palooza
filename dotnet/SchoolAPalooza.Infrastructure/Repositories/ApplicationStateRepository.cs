using System;
using System.Configuration;

namespace SchoolAPalooza.Infrastructure.Repositories
{
    public interface IApplicationStateRepository
    {
        string GetPrimaryConnectionString();
        string GetConnectionString(string name);
        string GetRequiredSetting(string key);
        string GetOptionalSetting(string key);
    }

    public class ApplicationStateRepository : IApplicationStateRepository
    {
        public string GetPrimaryConnectionString()
        {
            return GetConnectionString("SchoolAPaloozaConnectionString");
        }

        public string GetConnectionString(string name)
        {
            var value = ConfigurationManager.ConnectionStrings[name];
            if (value == null)
                throw new MissingSettingException(name);

            return value.ConnectionString;
        }

        public string GetRequiredSetting(string key)
        {
            var value = GetOptionalSetting(key);
            if (value.IsNullOrWhitespace())
                throw new MissingSettingException(key);

            return value;
        }

        public string GetOptionalSetting(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (value.IsNullOrWhitespace())
                return string.Empty;

            return value;
        }
    }

    public class MissingSettingException : Exception
    {
        public MissingSettingException(string key)
            : base($"The setting key {key} is missing from config.")
        { }
    }
}
