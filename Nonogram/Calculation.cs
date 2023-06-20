//Calculation.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nonogram
{
    internal class Calculation
    {
        private static int target { get; set; }
        private static NonogramData data;
        private static gameForm f1;
        private static int[][] columns;
        private static int[][] rows;
        private static string filename;
        public Calculation(ref NonogramData _data, gameForm _f1, string _filename) //конструктор
        {
            data = _data;           
            columns = data.columns;
            rows = data.rows;
            filename = _filename;
            f1 = _f1;
        }
        public int targetFilled() //необхідна кількість заповнених клітинок
        {
            target = 0;
            for (int i = 0; i < data.columns.Length; i++)
            {
                for (int j = 0; j < data.columns[i].Length; j++)
                    target += data.columns[i][j];
            }
            return target;
        }
        public static bool checkIfFilled(Point point) //перевірка заповнення клітини
        {
            return (data.progress_matrix[point.X, point.Y] == "0") ? false : true;
        }

        public static bool compareSolution(ref NonogramData[] package) //перевірка на відповідність еталонному масиву
        {
            for (int i = 0; i < data.size; i++)
            {
                for (int j = 0; j < data.size; j++)
                    if (data.progress_matrix[i, j] == "1" && data.progress_matrix[i, j] != data.solution[i, j]) { return false; }
            }
            return true;
        }

        public static int[] getCols(int col) //отримати користувацький стовпецб
        {
            int[] userCol = new int[columns[col].Length];
            for (int i = 0; i < columns[col].Length; i++) { userCol[i] = columns[col][i]; }
            return userCol;
        }
        public static int[] getRows(int row) //отримати користувацький рядок
        {
            int[] userRow = new int[rows[row].Length];
            for (int i = 0; i < rows[row].Length; i++) { userRow[i] = rows[row][i]; }
            return userRow;
        }

        public static bool checkLine(string[] userRow, int[] nonogramCondition) //перевірити, чи відповідає рядок або стовпець умові
        {
            List<int> blockSizes = new List<int>();
            int currentBlockSize = 0;

            foreach (string cell in userRow)
            {
                if (cell == "1") { currentBlockSize++; }
                else if (currentBlockSize > 0)
                {
                    blockSizes.Add(currentBlockSize);
                    currentBlockSize = 0;
                }
            }

            if (currentBlockSize > 0) { blockSizes.Add(currentBlockSize); }

            if (blockSizes.Count != nonogramCondition.Length) { return false; }

            for (int i = 0; i < blockSizes.Count; i++)
                if (blockSizes[i] != nonogramCondition[i]) { return false; }
            return true;
        }
    }
}
