using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TestSnake
{
    public class LoadLevel
    {
        private string ConfigFilePath { get; set; }
        public LoadLevel(string config)
        {
            ConfigFilePath = config;
        }

        public LevelSettings ReadInfo()
        {
            LevelSettings level = new LevelSettings();
            try
            {
                List<string> fileInfo = new List<string>();
                using (StreamReader streamReader = new StreamReader(ConfigFilePath))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        fileInfo.Add(line);
                    }
                }
                level.Width = Convert.ToInt32(fileInfo[0].Split(' ')[0]);
                level.Height = Convert.ToInt32(fileInfo[0].Split(' ')[1]);
                level.Level = new char[level.Height, level.Width];
                for (int i = 0; i < level.Height; i++)
                {
                    char[] tmp = fileInfo[i + 1].ToCharArray();

                    for (int j = 0; j < level.Width; j++)
                    {
                        level.Level[i, j] = tmp[j];
                    }
                }
                level.RequiredFoodPoints = Convert.ToInt32(fileInfo[level.Height + 1]);
                level.FoodCount = Convert.ToInt32(fileInfo[level.Height + 2]);
                level.SnakePosX = Convert.ToInt32(fileInfo[level.Height + 3].Split(' ')[0]);
                level.SnakePosY = Convert.ToInt32(fileInfo[level.Height + 3].Split(' ')[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Некорректные данные в файле загрузки");
            }
            return level;
        }
    }
}