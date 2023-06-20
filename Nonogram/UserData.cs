//userData.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonogram
{
    public class UserData
    {
        private static UserData userData = new UserData();
        public string name { get; set; }
        public int[] completed { get; set; }
        public int[] time { get; set; }
        public int[] average { get; set; }

        public static UserData getUserData() //отримати дані користувача
        {
            if (File.Exists("user.json"))
            {
                string jsonData = File.ReadAllText("user.json");
                return userData = JsonConvert.DeserializeObject<UserData>(jsonData);
            }
            return null;
        }
        public static void setData(UserData userData) //записати дані користувача
        {
            string jsonData = JsonConvert.SerializeObject(userData, Formatting.Indented);
            File.WriteAllText("user.json", jsonData);
        }
        public static void updateData(NonogramData[] levelPack) //оновити дані користувача
        {
            int difficulty = (levelPack[0].size / 5) - 1;
            int completed = 0, total_time = 0, avg_time = 0;
            userData = getUserData();
            int last = userData.completed.GetLength(0) - 1;
            for (int i = 0; i < levelPack.GetLength(0); i++)
            {
                if (levelPack[i].progress_state == 2)
                {
                    completed++;
                    total_time += levelPack[i].time_spent;
                }
            }
            if (completed > 0) { avg_time = (int)total_time / completed; }
            userData.completed[difficulty] = completed;
            userData.time[difficulty] = total_time;
            userData.average[difficulty] = avg_time;
            userData.completed[last] = 0;
            userData.time[last] = 0;
            userData.average[last] = 0;

            int all_comp = 0, all_total_time = 0, all_avg_time = 0;

            for (int i = 0; i < last + 1; i++)
            {
                all_comp += userData.completed[i];
                all_total_time += userData.time[i];
                all_avg_time += userData.average[i];
            }
            userData.completed[last] = all_comp;
            userData.time[last] = all_total_time;
            if (all_comp != 0) { userData.average[last] = (int)all_avg_time / all_comp;  }
            setData(userData); 
        }
    }
}
