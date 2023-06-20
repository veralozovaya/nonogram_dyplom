//SettingsData.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonogram
{
    public class SettingsData
    {
        private static SettingsData settingsData = new SettingsData();

        public bool current_mark_highlight { get; set; }
        public bool counter { get; set; }
        public bool legends_deact { get; set; }
        public bool autofill { get; set; }
        public int hint_amount { get; set; }

        public static SettingsData getSettings() //отримтаи налаштування
        {
            if (File.Exists("settings.json"))
            {
                string jsonData = File.ReadAllText("settings.json");
                settingsData = JsonConvert.DeserializeObject<SettingsData>(jsonData);
            }
            return settingsData;
        }
        public static void setSettings(SettingsData settings) //встановит налаштування
        {
            string jsonData = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText("settings.json", jsonData);
        }
    }
}
