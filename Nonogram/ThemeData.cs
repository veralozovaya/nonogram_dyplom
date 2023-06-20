//ThemeData.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonogram
{
    internal class ThemeData
    {
        private static ThemeData theme = new ThemeData();
        public string bg_color { get; set; }
        public string button_color_1 { get; set; }
        public string button_color_2 { get; set; }
        public string button_color_3 { get; set; }
        public string accent_color_1 { get; set; }
        public string accent_color_2 { get; set; }
        public string highlight_color { get; set; }
        public int font_size { get; set; }
        public string font_name { get; set; }
        public string font_color_light { get; set; }
        public string font_color_dark { get; set; }
        public string block_color { get; set; }
        public string todo_color { get; set; }
        public string in_progress_color { get; set; }
        public string done_color { get; set; }

        public static ThemeData getTheme() //отримати дані теми
        {
            if (File.Exists("theme.json"))
            {
                string jsonData = File.ReadAllText("theme.json");
                theme = JsonConvert.DeserializeObject<ThemeData>(jsonData);
            }
            return theme;
        }
    }
}
