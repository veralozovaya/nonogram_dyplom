//mainMenu.cs
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
    public partial class mainMenu : Form
    {
        TableLayoutPanel tlp = new TableLayoutPanel();
        ThemeData theme = new ThemeData();
        ColorConverter colorConverter = new ColorConverter();
        private static readonly Dictionary<int, string> _filenames = new Dictionary<int, string>
    {
        { 0, "easy.json" },
        { 1, "medium.json" },
        { 2, "hard.json" }
    };
        private static readonly Dictionary<int, string> _difficulties = new Dictionary<int, string>
    {
        { 0, "Простий" },
        { 1, "Середній" },
        { 2, "Складний" }
    };
        public mainMenu()
        {
            //надання кнопкам властивостей
            InitializeComponent();
            theme = ThemeData.getTheme();
            Font f = new Font(theme.font_name, theme.font_size);
            this.Font = f;
            this.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_light);
            this.BackColor = (Color)colorConverter.ConvertFromString(theme.bg_color);

            label1.Font = new Font("Montserrat", 30);
            label1.Location = new Point(this.Width / 2 - label1.Width / 2, label1.Height);

            button1.Location = new Point(this.Width / 2 - button1.Width / 2, this.Height - button1.Height * 5);
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button1.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_dark);
            button1.BackColor = (Color)colorConverter.ConvertFromString(theme.button_color_1);

            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.BackColor = (Color)colorConverter.ConvertFromString(theme.button_color_2);
            button2.Location = new Point(this.Width / 2 - button2.Width / 2, this.Height - button2.Height * 3);

            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button3.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_light);
            button3.BackColor = (Color)colorConverter.ConvertFromString(theme.button_color_3);
            button3.Location = new Point(this.Width / 2 - button3.Width / 2, this.Height - button3.Height);

            this.Height += button3.Height + 35;

            this.CenterToScreen();
        }

        private void exit_Click(object sender, EventArgs e) //натискання на кнопку виходу
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e) //натискання на кнопку налаштувань
        {
            Settings set = new Settings();
            set.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e) //натискання на кнопку "Грати"
        {
            button1.Visible = false;
            button2.Visible = false;

            Label action = new Label();
            action.Text = "Оберіть складність";
            action.Width = button1.Width;
            action.Height = button1.Height;
            action.TextAlign = ContentAlignment.MiddleCenter;
            action.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_light);

            TrackBar tb = new TrackBar();
            tb.Maximum = 2;
            tb.Minimum = 0;
            tb.Width = button1.Width;
            tb.Height = button1.Height - 10;
            tb.ValueChanged += new EventHandler(tb_ValueChanged);

            Label difficulty = new Label();
            difficulty.Text = _difficulties[0];
            difficulty.Width = button1.Width;
            difficulty.Height = button1.Height;
            difficulty.TextAlign = ContentAlignment.MiddleCenter;
            difficulty.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_light);

            Button go = new Button();
            go.Text = "Обрати рівень";
            go.Width = button1.Width;
            go.Height = button1.Height;
            go.Location = button2.Location;
            go.FlatStyle = FlatStyle.Flat;
            go.FlatAppearance.BorderSize = 0;
            go.Click += new EventHandler(go_Click);
            go.BackColor = (Color)colorConverter.ConvertFromString(theme.button_color_1);
            go.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_dark);
            this.Controls.Add(go);

            tlp.RowCount = 4;
            tlp.Controls.Add(action);
            tlp.Controls.Add(tb);
            tlp.Controls.Add(difficulty);
            tlp.AutoSize = true;
            tlp.Margin = new Padding(5);
            Point point = new Point(button2.Location.X, button2.Location.Y - 150);
            tlp.Location = point;
            this.Controls.Add(tlp);
        }

        private void tb_ValueChanged(object sender, EventArgs e) //обробник подій зміни значення слайдера
        {
            TrackBar trackbar = (TrackBar)tlp.GetControlFromPosition(0, 1);
            int val = trackbar.Value;
            tlp.GetControlFromPosition(0, 2).Text = _difficulties[val];
        }

        private void go_Click(object sender, EventArgs e) //натискання на кнопку вибору рівння
        {
            TrackBar trackbar = (TrackBar)tlp.GetControlFromPosition(0, 1);
            int val = trackbar.Value;
            levelMenu lvl = new levelMenu(_filenames[val]);
            lvl.Show();
            this.Hide();
        }

        private void mainMenu_FormClosed(object sender, FormClosedEventArgs e) //оброюник події закривання форми
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e) //обробник події натискання на кнопку виходу
        {
            Application.Exit();
        }
    }
}
