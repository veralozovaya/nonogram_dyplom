//Stats.cs
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
    public partial class Stats : Form
    {
        SettingsData settings = new SettingsData();
        ThemeData theme = new ThemeData();
        UserData user = new UserData();
        TableLayoutPanel tlp = new TableLayoutPanel();
        NonogramData[] data1;
        NonogramData[] data2;
        NonogramData[] data3;
        ColorConverter colorConverter = new ColorConverter();
        public Stats() //конструктор
        {
            InitializeComponent();
            settings = SettingsData.getSettings();
            theme = ThemeData.getTheme();
            user = UserData.getUserData();

            this.BackColor = (Color)colorConverter.ConvertFromString(theme.bg_color);
            this.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_light);
            this.Font = new Font(theme.font_name, theme.font_size);
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            init_lbls();

            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e) //обробник події натискання на кнопку скидання прогресу
        {
            if (MessageBox.Show("Чи дійсно ви бажаєте скинути весь прогрес?", "Скидання прогресу", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                clear_progress(NonogramData.getLevelPack("easy.json"));
                clear_progress(NonogramData.getLevelPack("medium.json"));
                clear_progress(NonogramData.getLevelPack("hard.json"));
                this.Controls.Clear();
                init_lbls();
            }
        }

        private void clear_progress(NonogramData[] data) //скидання прогресу
        {
            for (int i = 0; i < data.Length; i++)
            {
                for (int row = 0; row < data[i].progress_matrix.GetLength(0); row++)
                {
                    for (int col = 0; col < data[i].progress_matrix.GetLength(1); col++)
                    {
                        data[i].progress_matrix[row, col] = "0";
                    }
                }
                data[i].time_spent = 0;
                data[i].progress_state = 0;
            }
            Array.Fill(user.average, 0);
            Array.Fill(user.time, 0);
            Array.Fill(user.completed, 0);
            NonogramData.setData(data);
            UserData.setData(user);
        }

        private void init_lbls() //ініціалізування 
        {
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button1.Text = "Скинути прогрес";
            button1.AutoSize = true;
            button1.BackColor = (Color)colorConverter.ConvertFromString(theme.button_color_3);

            label13.Font = new Font("Montserrat", 30);
            label13.Text = "Статистика";

            label1.Text = $"Завершено простих: {user.completed[0]}";
            label2.Text = $"Завершено середніх: {user.completed[1]}";
            label3.Text = $"Завершено складних: {user.completed[2]}";
            label4.Text = $"Завершено загалом: {user.completed[3]}";

            label5.Text = $"Витрачено часу на легкі: {TimeSpan.FromSeconds(user.time[0]).ToString(@"hh\:mm\:ss")}";
            label6.Text = $"Витрачено часу на середні: {TimeSpan.FromSeconds(user.time[1]).ToString(@"hh\:mm\:ss")}";
            label7.Text = $"Витрачено часу на складні: {TimeSpan.FromSeconds(user.time[2]).ToString(@"hh\:mm\:ss")}";
            label8.Text = $"Витрачено часу загалом: {TimeSpan.FromSeconds(user.time[3]).ToString(@"hh\:mm\:ss")}";

            label9.Text = $"Витрачено у середньому часу на легкий рівень: {TimeSpan.FromSeconds(user.average[0]).ToString(@"hh\:mm\:ss")}";
            label10.Text = $"Витрачено у середньому часу на середній рівень: {TimeSpan.FromSeconds(user.average[1]).ToString(@"hh\:mm\:ss")}";
            label11.Text = $"Витрачено у середньому часу на складний рівень: {TimeSpan.FromSeconds(user.average[2]).ToString(@"hh\:mm\:ss")}";
            label12.Text = $"Середній загальний час : {TimeSpan.FromSeconds(user.average[3]).ToString(@"hh\:mm\:ss")}";

            tlp.Controls.Add(label13, 0, 0);
            tlp.SetColumnSpan(label13, 2);

            tlp.Controls.Add(button1, 2, 0);

            tlp.Controls.Add(label1, 0, 1);
            tlp.Controls.Add(label2, 0, 2);
            tlp.Controls.Add(label3, 0, 3);
            tlp.Controls.Add(label4, 0, 4);

            tlp.Controls.Add(label5, 1, 1);
            tlp.Controls.Add(label6, 1, 2);
            tlp.Controls.Add(label7, 1, 3);
            tlp.Controls.Add(label8, 1, 4);
            tlp.Controls.Add(label9, 2, 1);
            tlp.Controls.Add(label10, 2, 2);
            tlp.Controls.Add(label11, 2, 3);
            tlp.Controls.Add(label12, 2, 4);

            tlp.AutoSize = true;
            tlp.Dock = DockStyle.Fill;
            tlp.ColumnStyles.Clear();
            tlp.RowStyles.Clear();
            tlp.ColumnCount = 3;
            tlp.RowCount = 6;
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            this.Controls.Add(tlp);

            int buttonWidth = tlp.GetColumnWidths()[2];
            int buttonHeight = button1.Height;
            button1.Location = new Point(tlp.Width - buttonWidth, button1.Location.Y);
            button1.Size = new Size(buttonWidth, label13.Height);
        }
    }
}
