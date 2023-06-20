//NonogramData.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonogram
{
    public class NonogramData
    {
        private static readonly Dictionary<int, string> _filenames = new Dictionary<int, string>
    {
        { 5, "easy.json" },
        { 10, "medium.json" },
        { 15, "hard.json" }
    };
        public int size { get; set; }
        public int[][] rows { get; set; }
        public int[][] columns { get; set; }
        public string[,] solution { get; set; }
        public string[,] progress_matrix { get; set; }
        public int progress_state { get; set; }
        public int time_spent { get; set; }

        private static NonogramData[] getData(string _filename) //отримати дані з файлу
        {
            if (File.Exists(_filename))
            {
                string jsonData = File.ReadAllText(_filename);
                return JsonConvert.DeserializeObject<NonogramData[]>(jsonData);
            }
            return null;
        }
        public static void setData(NonogramData[] data) //внести дані до файлу
        {
            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_filenames[data[0].size], jsonData);
        }
        public static string getFilename(int size) { return _filenames[size]; } //отримати назву файлу
        public static NonogramData getLevel(int level, string _filename) { return getData(_filename)[level - 1]; } //отримати рівень
        public static NonogramData[] getLevelPack(string _filename) { return getData(_filename); } //отриати пакет рівнів
        public static int getLevelQuantity(string _filename) { return getData(_filename).GetLength(0); } //отримати кількість рівнів
        public static string[] getRow(int row, int level, string filename) //отримати умову рядка
        {
            NonogramData data = NonogramData.getLevel(level, filename);
            string[] userRow = new string[data.size];
            for (int i = 0; i < data.size; i++) { userRow[i] = data.progress_matrix[row, i]; }
            return userRow;
        }
        public static string[] getCol(int col, int level, string filename) //отримтаи умову стовпця
        {
            NonogramData data = NonogramData.getLevel(level, filename);
            string[] userCol = new string[data.size];
            for (int i = 0; i < data.size; i++) { userCol[i] = data.progress_matrix[i, col]; }
            return userCol;
        }
    }
}
