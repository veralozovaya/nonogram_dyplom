//levelMenu.cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;

namespace Nonogram
{
    public partial class levelMenu : Form
    {
        TableLayoutPanel levelGrid;
        ThemeData theme;
        NonogramData[] levelPack;
        ColorConverter colorConverter = new ColorConverter();
        string filename;
        public levelMenu(string _filename) //конструктор
        {
            InitializeComponent();
            this.CenterToScreen();
            theme = ThemeData.getTheme();
            this.BackColor = (Color)colorConverter.ConvertFromString(theme.bg_color);
            this.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_light);
            Font f = new Font(theme.font_name, theme.font_size);
            this.Font = f;
            filename = _filename;
            levelGrid = new TableLayoutPanel();
            levelGrid.ColumnCount = 5;
            levelGrid.AutoSize = true;
            levelGrid.Margin = new Padding(0);
            levelPack = NonogramData.getLevelPack(filename);
            for (int i = 0; i < levelPack.Length; i++)
            {
                Button button = new Button();
                button.Text = $"{i + 1}";
                button.TextAlign = ContentAlignment.MiddleCenter;
                button.AutoSize = false;
                button.Width = 75;
                button.Height = 75;
                button.MouseClick += new MouseEventHandler(OnBoxMouseClick);
                button.Tag = new Point(i, 0);
                if (levelPack[i].progress_state == 0)
                {
                    button.BackColor = (Color)colorConverter.ConvertFromString(theme.todo_color);
                    button.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_dark);
                }
                else if (levelPack[i].progress_state == 1)
                {
                    button.BackColor = (Color)colorConverter.ConvertFromString(theme.in_progress_color);
                    button.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_dark);
                }
                else if (levelPack[i].progress_state == 2) { button.BackColor = (Color)colorConverter.ConvertFromString(theme.done_color); }
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                levelGrid.Controls.Add(button, i, 5);
            }
            Controls.Add(levelGrid);
            levelGrid.Dock = DockStyle.None;
            Button mm = new Button();
            mm.Text = "Назад до головного меню";
            mm.Width = levelGrid.Width - 10;
            mm.Height = 75;
            mm.Location = new Point(5, levelGrid.Height);
            mm.Click += new EventHandler(mm_Click);
            mm.BackColor = (Color)colorConverter.ConvertFromString(theme.button_color_2);
            mm.FlatStyle = FlatStyle.Flat;
            mm.FlatAppearance.BorderSize = 0;
            Controls.Add(mm);

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.CenterToScreen();
        }
        private void OnBoxMouseClick(object sender, MouseEventArgs e) //обробник події натискання на кнопку рівня
        {
            Button button = sender as Button;
            Point point = (Point)button.Tag;
            int level = levelGrid.ColumnCount * point.Y + point.X + 1;
            NonogramData[] d = NonogramData.getLevelPack(filename);
            if (d[level - 1].progress_state == 2)
            {
                for (int i = 0; i < d[level - 1].size; i++)
                {
                    for (int j = 0; j < d[level - 1].size; j++)
                    {
                        d[level - 1].progress_matrix[i, j] = "0";
                    }
                }
            }
            gameForm f = new gameForm(ref d, level);
            f.Show();
            this.Hide();
        }

        private void mm_Click(object sender, EventArgs e) //обробник події натискання на кнопку повернення до головного меню
        {
            mainMenu mm = new mainMenu();
            this.Hide();
            mm.Show();
        }

        private void levelMenu_FormClosed(object sender, FormClosedEventArgs e) //обробник події закривання форми
        {
            Application.Exit();
        }
    }
}
