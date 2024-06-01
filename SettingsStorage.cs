using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace Sapper2
{
    public static class SettingsStorage
    {
        private static IsolatedStorageSettings Settings = IsolatedStorageSettings.ApplicationSettings;

        static SettingsStorage()
        {
            if (Settings.Contains("SettingsVersion") == false)
            {
                SetDefaultSettings();
            }
        }

        public static void StoreSetting(string settingName, string value)
        {
            StoreSetting<string>(settingName, value);
        }

        public static void StoreSetting<TValue>(string settingName, TValue value)
        {
            if (!Settings.Contains(settingName))
                Settings.Add(settingName, value);
            else
                Settings[settingName] = value;

            Settings.Save();
        }

        public static bool TryGetSetting<TValue>(string settingName, out TValue value)
        {
            if (Settings.Contains(settingName))
            {
                value = (TValue)Settings[settingName];
                return true;
            }
            else if (ListOfDefaultSettings.Contains(settingName))
            {
                if (Settings.Contains(settingName))
                {
                    value = (TValue)Settings[settingName];
                    return true;
                }
            }

            value = default(TValue);
            return false;
        }

        private static readonly string[] ListOfDefaultSettings = { "Vibration" };

        public static void SetDefaultSettings()
        {
            Settings["SettingsVersion"] = 1.0;
            Settings["Vibration"] = true;
        }
    }
}
