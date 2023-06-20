//Settings.cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nonogram
{
    public partial class Settings : Form
    {
        SettingsData settings = new SettingsData();
        ThemeData theme = new ThemeData();
        ColorConverter colorConverter = new ColorConverter();
        TableLayoutPanel panel = new TableLayoutPanel();
        CheckBox c_highlights = new CheckBox();
        CheckBox c_counter = new CheckBox();
        CheckBox c_legends = new CheckBox();
        CheckBox c_autofill = new CheckBox();
        NumericUpDown c_hints = new NumericUpDown();
        public Settings() //конструктор
        {
            InitializeComponent();
            settings = SettingsData.getSettings();
            theme = ThemeData.getTheme();

            this.BackColor = (Color)colorConverter.ConvertFromString(theme.bg_color);
            this.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_light);
            this.Font = new Font(theme.font_name, theme.font_size);
            Label header = new Label();
            header.Font = new Font(theme.font_name, 30);
            header.Text = "Налаштування";
            header.AutoSize = true;
            header.Location = new Point(header.Location.X, header.Location.Y + 20);
            Controls.Add(header);

            panel.AutoSize = true;
            panel.ColumnCount = 2;
            panel.RowCount = 6;

            Label highlights = new Label();
            highlights.Text = "Підсвітка рядків та стовпців";
            highlights.AutoSize = true;
            panel.Controls.Add(highlights, 0, 0);

            Label counter = new Label();
            counter.Text = "Лічильник";
            counter.AutoSize = true;
            panel.Controls.Add(counter, 0, 1);

            Label legends = new Label();
            legends.Text = "Підсвітка умови";
            legends.AutoSize = true;
            panel.Controls.Add(legends, 0, 2);

            Label autofill = new Label();
            autofill.Text = "Автозаповення пустих клітинок";
            autofill.AutoSize = true;
            panel.Controls.Add(autofill, 0, 3);

            Label hints = new Label();
            hints.Text = "Кількість підказок";
            hints.AutoSize = true;
            panel.Controls.Add(hints, 0, 4);

            c_highlights.Checked = settings.current_mark_highlight ? true : false;
            c_highlights.CheckedChanged += new EventHandler(c_highlights_Changed);
            panel.Controls.Add(c_highlights, 1, 0);

            c_counter.Checked = settings.counter ? true : false;
            c_counter.CheckedChanged += new EventHandler(c_counter_Changed);
            panel.Controls.Add(c_counter, 1, 1);

            c_legends.Checked = settings.legends_deact ? true : false;
            c_legends.CheckedChanged += new EventHandler(c_legends_Changed);
            panel.Controls.Add(c_legends, 1, 2);

            c_autofill.Checked = settings.autofill ? true : false;
            c_autofill.CheckedChanged += new EventHandler(c_autofill_Changed);
            panel.Controls.Add(c_autofill, 1, 3);

            c_hints.Value = settings.hint_amount;
            c_hints.ValueChanged += new EventHandler(c_hints_Changed);
            panel.Controls.Add(c_hints, 1, 4);

            panel.Location = new Point(header.Location.X, header.Location.Y + header.Height + 50);
            panel.Height = autofill.Height * 10;
            Button statistics = new Button();
            statistics.Text = "Статистика";
            statistics.Height = 75;
            statistics.BackColor = (Color)colorConverter.ConvertFromString(theme.button_color_3);
            statistics.FlatStyle = FlatStyle.Flat;
            statistics.FlatAppearance.BorderSize = 0;
            statistics.Anchor = AnchorStyles.Right;  // Align the button to the right side of the panel
            statistics.Location = new Point(panel.Location.X, panel.Location.Y + panel.Height + 20);
            statistics.Width = panel.Width;
            statistics.Click += new EventHandler(stats_Click);
            panel.Controls.Add(statistics);
            panel.SetRowSpan(statistics, 2);
            panel.SendToBack();

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(panel);



            this.CenterToScreen();
        }

        private void stats_Click(object sender, EventArgs e) //обробник події натискання на кнопку перегляду статистики
        {
            Stats stats = new Stats();
            stats.Show();
        }

        private void c_hints_Changed(object sender, EventArgs e) //зміна кількості підказок
        {
            settings.hint_amount = Convert.ToInt32(c_hints.Value);
            SettingsData.setSettings(settings);
        }

        private void c_autofill_Changed(object sender, EventArgs e) //зміна прапорця автозаповнення
        {
            settings.autofill = settings.autofill ? false : true;
            SettingsData.setSettings(settings);
        }

        private void c_legends_Changed(object sender, EventArgs e) //зміна прапорця підсвітки виконаних умов
        {
            settings.legends_deact = settings.legends_deact ? false : true;
            SettingsData.setSettings(settings);
        }

        private void c_counter_Changed(object sender, EventArgs e) //зміна налаштування видимості лічильника
        {
            settings.counter = settings.counter ? false : true;
            SettingsData.setSettings(settings);
        }

        private void c_highlights_Changed(object sender, EventArgs e) //зміна прапорця підсвітки поточних рядка та стовпця
        {
            settings.current_mark_highlight = settings.current_mark_highlight ? false : true;
            SettingsData.setSettings(settings);
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e) //обробник події закриття форми
        {
            mainMenu mm = new mainMenu(); mm.Show(); this.Hide();
        }
    }
}
