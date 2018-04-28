using System;
using Microsoft.Extensions.Configuration;
using MomitKiller.Api.Models;

namespace MomitKiller.Api
{
    public class Settings
    {
        private IConfiguration _appSettings;

        public Settings(IConfiguration appSettings)
        {
            _appSettings = appSettings;
        }

        public T GetValue<T>(string key)
        {
            return _appSettings.GetValue<T>(key);
        }
    }
}
