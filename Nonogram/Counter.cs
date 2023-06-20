//Counter.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonogram
{
    internal static class Counter
    {
        public static int getCurrentFilled(NonogramData data) //отримати поточну кількість заповнених клітин
        {
            int count = 0;
            foreach (string value in data.progress_matrix)
                if(value == "1") count++;
            return count;
        }
        public static void updateCounter(int target, ref Label counter, ref NonogramData data, string filename, gameForm f1, ref NonogramData[] dataPack)
        //оновити лічильник
        {
            counter.Text = $"{getCurrentFilled(data)}/{target}";
            if (getCurrentFilled(data) == target && !Calculation.compareSolution(ref dataPack)) { MessageBox.Show("У вирішенні знайдені помилки. Виправте їх для успішного проходження рівня."); }
            else if (getCurrentFilled(data) == target && Calculation.compareSolution(ref dataPack) && MessageBox.Show("Рівень успішно пройдено") == DialogResult.OK)
            {
                data.progress_state = 2;
                NonogramData.setData(dataPack);
                UserData.updateData(dataPack);
                levelMenu lvl = new levelMenu(filename);
                lvl.Show();
                f1.Hide();
            }
        }
    }
}
