//gameForm.cs
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection.Emit;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

namespace Nonogram
{
    public partial class gameForm : Form
    {
        Label time_lbl = new Label();
        TableLayoutPanel grid;
        Label counter;
        int target = 0, hints_all = 0, hints_current = 0;
        NonogramData[] dataPack;
        NonogramData data;
        Calculation calc;
        ThemeData theme;
        SettingsData settings;
        ColorConverter colorConverter = new ColorConverter();
        Stopwatch time = new Stopwatch();
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        int level;
        bool hintPressed = false, timerRunning = false;
        Button pause = new Button();
        TimeSpan timeSpan, totalElapsed;
        Button hint = new Button();
        Button restart = new Button();
        Button exit = new Button();

        public gameForm(ref NonogramData[] _data, int _level) //конструктор
        {
            InitializeComponent();
            theme = ThemeData.getTheme();
            timer.Tick += new EventHandler(timer_Tick);
            dataPack = _data;
            level = _level;
            data = dataPack[level - 1];
            calc = new Calculation(ref data, this, NonogramData.getFilename(data.size));
            target = calc.targetFilled();
            settings = SettingsData.getSettings();
            hints_all = settings.hint_amount;
            hints_current = settings.hint_amount;
            time_lbl.Height *= 2;
            timeSpan = TimeSpan.FromSeconds(data.time_spent);

            Font f = new Font(theme.font_name, theme.font_size);
            this.Font = f;
            this.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_light);
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Text = "";
            init_Form_elements();

            pause.Click += new EventHandler(pause_Click);
            restart.Click += new EventHandler(restart_Click);
            hint.Click += new EventHandler(hint_Click);
            exit.Click += new EventHandler(exit_Click);

            if (settings.legends_deact)
            {
                for (int i = 0; i < data.size; i++)
                    color_legends(new Point(i, i));
            }
            if (data.progress_state == 2) { reset_restart(); }
            data.progress_state = 1;
            start_timer();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void start_timer() //запуск таймера
        {
            if (!timerRunning) { timer.Start(); }
            if (!time.IsRunning) { time.Start(); }
        }

        private void init_Form_elements() //ініціалізація елементів на формі
        {
            counter = new Label();
            counter.Text = $"{Counter.getCurrentFilled(data)}/{target}";
            counter.AutoSize = true;
            counter.ForeColor = settings.counter ? (Color)colorConverter.ConvertFromString(theme.font_color_light) : (Color)colorConverter.ConvertFromString(theme.bg_color);
            this.Controls.Add(counter);

            grid = new TableLayoutPanel();
            grid.RowCount = data.size + 1;
            grid.ColumnCount = data.size + 1;
            grid.AutoSize = true;
            grid.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            grid.Margin = new Padding(0);

            for (int i = 0; i < data.size; i++)
            {
                Label label = new Label();
                label.Text = string.Join(" ", data.rows[i]);
                label.TextAlign = ContentAlignment.MiddleLeft;
                label.AutoSize = true;
                label.Dock = DockStyle.Fill;
                grid.Controls.Add(label, data.size, i);
            }

            for (int j = 0; j < data.size; j++)
            {
                Label label = new Label();
                label.Text = string.Join("\n", data.columns[j]);
                label.TextAlign = ContentAlignment.TopCenter;
                label.AutoSize = true;
                label.Dock = DockStyle.Fill;
                grid.Controls.Add(label, j, data.size);
            }

            for (int i = 0; i < data.size; i++)
            {
                for (int j = 0; j < data.size; j++)
                {
                    PictureBox box = new PictureBox();
                    box.BackColor = Color.White;
                    if (data.progress_matrix[i, j] == "1") { box.ImageLocation = "filled.png"; }
                    else if (data.progress_matrix[i, j] == "2") { box.ImageLocation = "cross.png"; }
                    box.SizeMode = PictureBoxSizeMode.StretchImage;
                    box.Size = new Size(30, 30);
                    box.Tag = new Point(i, j);
                    box.MouseClick += new MouseEventHandler(OnBoxMouseClick);
                    grid.Controls.Add(box, j, i);
                }
            }
            this.BackColor = (Color)colorConverter.ConvertFromString(theme.bg_color);

            color_cells(grid);
            grid.SendToBack();
            Controls.Add(grid);
            grid.Location = new Point(40, 80);
            counter.Location = new Point(grid.Width / 2, counter.Height);
            time_lbl.Text = timeSpan.ToString(@"hh\:mm\:ss");
            time_lbl.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_light);
            time_lbl.TextAlign = ContentAlignment.MiddleCenter;

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.FlowDirection = FlowDirection.TopDown;
            panel.WrapContents = false;
            panel.AutoSize = true;
            panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel.Location = new Point(grid.Location.X * 3 + grid.Width - 20, 0);
            panel.Height = grid.Location.Y + grid.Height;
            panel.Width = time_lbl.Width + 10;

            exit.Text = "В меню";
            exit.Width = time_lbl.Width;
            exit.Height = 75;
            exit.BackColor = (Color)colorConverter.ConvertFromString(theme.button_color_2);
            exit.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_light);
            exit.Font = new Font(theme.font_name, 10);
            exit.FlatStyle = FlatStyle.Flat;
            exit.FlatAppearance.BorderSize = 0;
            panel.Controls.Add(exit);

            hint.Text = $"Підказки {hints_current}/{hints_all}";
            hint.Width = time_lbl.Width;
            hint.Height = 75;
            hint.BackColor = (Color)colorConverter.ConvertFromString(theme.button_color_1);
            hint.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_dark);
            hint.Font = new Font(theme.font_name, 10);
            hint.FlatStyle = FlatStyle.Flat;
            hint.FlatAppearance.BorderSize = 0;
            panel.Controls.Add(hint);

            panel.Controls.Add(time_lbl);

            pause.Text = "Пауза";
            pause.Width = hint.Width;
            pause.Height = 75;
            pause.BackColor = (Color)colorConverter.ConvertFromString(theme.button_color_1);
            pause.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_dark);
            pause.Font = new Font(theme.font_name, 10);
            pause.FlatStyle = FlatStyle.Flat;
            pause.FlatAppearance.BorderSize = 0;
            panel.Controls.Add(pause);


            restart.Text = $"Заново";
            restart.Width = hint.Width;
            restart.Height = 75;
            restart.BackColor = (Color)colorConverter.ConvertFromString(theme.button_color_3);
            restart.ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_light);
            restart.Font = new Font(theme.font_name, 10);
            restart.FlatStyle = FlatStyle.Flat;
            restart.FlatAppearance.BorderSize = 0;
            panel.Controls.Add(restart);

            Controls.Add(panel);
        }


        private void timer_Tick(object sender, EventArgs e) //
        {
            if (time.IsRunning)
            {
                totalElapsed = time.Elapsed + timeSpan;
                string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}",
                    totalElapsed.Hours, totalElapsed.Minutes, totalElapsed.Seconds);
                time_lbl.Text = elapsedTime;
                data.time_spent = Convert.ToInt32(totalElapsed.TotalSeconds);
            }
        }

        private void restart_Click(object sender, EventArgs e) //обробник події натискання на кнопку "Заново"
        {
            reset_restart();
            UserData.updateData(dataPack);
            start_timer();
        }

        private void reset_restart() //початок рівня заново
        {
            if (MessageBox.Show("Почати заново?", "Почати заново?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                for (int i = 0; i < data.size; i++)
                {
                    for (int j = 0; j < data.size; j++)
                    {
                        data.progress_matrix[i, j] = "0";
                    }
                }
                data.time_spent = 0;
                time.Stop();
                time.Reset();
                totalElapsed = TimeSpan.Zero;
                timeSpan = TimeSpan.Zero;
                this.Controls.Clear();
                init_Form_elements();
                NonogramData.setData(dataPack);
            }
        }

        private void hint_Click(object sender, EventArgs e) //обробник події натискання на кнопку підказок
        {
            hintPressed = hintPressed ? false : true;
            hint.BackColor = hintPressed ? (Color)colorConverter.ConvertFromString(theme.button_color_3) : (Color)colorConverter.ConvertFromString(theme.button_color_1);
        }

        private void highlight(Point point) //підсвітка поточних рядка та стовпця
        {
            for (int i = 0; i < data.size; i++)
            {
                grid.GetControlFromPosition(i, point.X).BackColor = Color.FromArgb(40, (Color)colorConverter.ConvertFromString(theme.highlight_color));
                grid.GetControlFromPosition(point.Y, i).BackColor = Color.FromArgb(40, (Color)colorConverter.ConvertFromString(theme.highlight_color));
            }
        }

        private void pause_Click(object sender, EventArgs e) //обробник поії натискання на кнопку паузи
        {
            if (time.IsRunning)
            {
                time.Stop(); pause.Text = "Зняти \nпаузу";
                for (int i = 0; i < data.size; i++)
                {
                    grid.GetControlFromPosition(data.size, i).Hide();
                    grid.GetControlFromPosition(i, data.size).Hide();
                }
                grid.Enabled = false;
                return;
            }
            else if (!time.IsRunning)
            {
                pause.Text = "Пауза"; time.Start();
                for (int i = 0; i < data.size; i++)
                {
                    grid.GetControlFromPosition(data.size, i).Show();
                    grid.GetControlFromPosition(i, data.size).Show();
                }
                grid.Enabled = true;
                return;
            }
        }

        private void exit_Click(object sender, EventArgs e) //обробник події натискання на кнопку повернення до меню вибору рівня
        {
            TimeSpan elapsed = time.Elapsed;
            data.time_spent += Convert.ToInt32(elapsed.TotalSeconds);
            NonogramData.setData(dataPack);
            levelMenu lvl = new levelMenu(NonogramData.getFilename(data.size));
            this.Hide();
            lvl.Show();
        }

        private void OnBoxMouseClick(object sender, MouseEventArgs e) //обробник події натискання на комірку ігрового поля
        {
            PictureBox box = sender as PictureBox;
            Point point = (Point)box.Tag;
            color_cells(grid);

            if (hintPressed && hints_current > 0)
            {
                if (data.solution[point.X, point.Y] == "0") { box.ImageLocation = "cross.png"; data.progress_matrix[point.X, point.Y] = "2"; }
                if (data.solution[point.X, point.Y] == "1") { box.ImageLocation = "filled.png"; data.progress_matrix[point.X, point.Y] = "1"; }
                hints_current--;
                if (hints_current == 0)
                {
                    hint.BackColor = (Color)colorConverter.ConvertFromString(theme.button_color_1);
                    hintPressed = false;
                    hint.Enabled = false;
                }
                hint.Text = $"Підказки {hints_current}/{hints_all}";
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (data.progress_matrix[point.X, point.Y] == "0" || data.progress_matrix[point.X, point.Y] == "2")
                {
                    data.progress_matrix[point.X, point.Y] = "1";

                    if (settings.current_mark_highlight == true) { highlight(point); }
                    box.ImageLocation = "filled.png";
                }
                else if (data.progress_matrix[point.X, point.Y] == "1") { clear_cell(point, ref box); }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (box.ImageLocation != "cross.png")
                {
                    data.progress_matrix[point.X, point.Y] = "2";
                    box.ImageLocation = "cross.png";
                }
                else { clear_cell(point, ref box); }
            }
            else if (e.Button == MouseButtons.Middle)
            {
                clear_cell(point, ref box);
            }
            data.time_spent = Convert.ToInt32(totalElapsed.TotalSeconds);
            NonogramData.setData(dataPack);
            if (settings.legends_deact) color_legends(point);
            if (settings.autofill) fill_remain(point);
            Counter.updateCounter(target, ref counter, ref data, NonogramData.getFilename(data.size), this, ref dataPack);

        }

        private void clear_cell(Point point, ref PictureBox box) //очищення клітинки
        {
            data.progress_matrix[point.X, point.Y] = "0";
            box.ImageLocation = null;
        }
        private void color_cells(TableLayoutPanel grid) //надання фону клітинок певного кольору
        {
            for (int i = 0; i < data.size; i++)
            {
                for (int j = 0; j < data.size; j++)
                {
                    if ((i < 5 && j < 5) || (i < 5 && j > 9) || (i > 4 && i < 10 && j > 4 && j < 10) || (i > 9 && j < 5) || (i > 9 && j > 9))
                    {
                        grid.GetControlFromPosition(i, j).BackColor = Color.FromArgb(140, (Color)colorConverter.ConvertFromString(theme.accent_color_1));
                    }
                    else if ((i < 5 && j > 4 && j < 10) || (i > 4 && i < 10 && j < 5) || (i > 4 && i < 10 && j > 9) || (i > 9 && j > 4 && j < 10))
                    {
                        grid.GetControlFromPosition(i, j).BackColor = Color.FromArgb(100, (Color)colorConverter.ConvertFromString(theme.accent_color_2));
                    }
                }
            }
        }
        private void fill_remain(Point point) //заповнити пусти клітинки при виконанні умови рядка чи стовпця
        {
            string[] userCol = NonogramData.getCol(Convert.ToInt32(point.Y), level, NonogramData.getFilename(data.size));
            string[] userRow = NonogramData.getRow(Convert.ToInt32(point.X), level, NonogramData.getFilename(data.size));
            int[] condCol = Calculation.getCols(Convert.ToInt32(point.Y));
            int[] condRow = Calculation.getRows(Convert.ToInt32(point.X));
            PictureBox pictureBox;
            if (Calculation.checkLine(userRow, condRow))
            {
                for (int i = 0; i < data.size; i++)
                {
                    pictureBox = (PictureBox)grid.GetControlFromPosition(i, point.X);
                    if (pictureBox.ImageLocation == null) { pictureBox.ImageLocation = "cross.png"; data.progress_matrix[point.X, i] = "2"; }
                }
            }
            if (Calculation.checkLine(userCol, condCol))
            {
                for (int i = 0; i < data.size; i++)
                {
                    pictureBox = (PictureBox)grid.GetControlFromPosition(point.Y, i);
                    if (pictureBox.ImageLocation == null) { pictureBox.ImageLocation = "cross.png"; data.progress_matrix[i, point.Y] = "2"; }
                }
            }
        }
        private void color_legends(Point point) //зафарбувати виконані умови
        {
            string[] userCol = NonogramData.getCol(Convert.ToInt32(point.Y), level, NonogramData.getFilename(data.size));
            string[] userRow = NonogramData.getRow(Convert.ToInt32(point.X), level, NonogramData.getFilename(data.size));
            int[] condCol = Calculation.getCols(Convert.ToInt32(point.Y));
            int[] condRow = Calculation.getRows(Convert.ToInt32(point.X));

            if (Calculation.checkLine(userRow, condRow))
            {
                if (settings.legends_deact) grid.GetControlFromPosition(data.size, Convert.ToInt32(point.X)).ForeColor = Color.DarkSlateGray;
            }
            if (Calculation.checkLine(userCol, condCol))
            {
                if (settings.legends_deact) grid.GetControlFromPosition(Convert.ToInt32(point.Y), data.size).ForeColor = Color.DarkSlateGray;
            }
            if (!Calculation.checkLine(userRow, condRow)) { grid.GetControlFromPosition(data.size, Convert.ToInt32(point.X)).ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_light); }
            if (!Calculation.checkLine(userCol, condCol)) { grid.GetControlFromPosition(Convert.ToInt32(point.Y), data.size).ForeColor = (Color)colorConverter.ConvertFromString(theme.font_color_light); }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e) //обробник події закриття форми
        {
            Application.Exit();
        }
    }
}