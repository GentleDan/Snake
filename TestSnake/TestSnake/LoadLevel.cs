using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TestSnake
{
    public class LoadLevel
    {
        private string ConfigFilePath { get; set; }
        private LevelSettings levelSettings;
        public LoadLevel(string config)
        {
            ConfigFilePath = config;
            levelSettings = new LevelSettings();
        }

        public void ReadInfo()
        {
            try
            {
                List<string> fileInfo = new List<string>();
                using (StreamReader streamReader = new StreamReader(ConfigFilePath))
                {
                    string line;
                    while((line = streamReader.ReadLine()) != null)
                    {
                        fileInfo.Add(line);
                    }
                }
                levelSettings.Width = Convert.ToInt32(fileInfo[0].Split(' ')[0]);
                levelSettings.Height = Convert.ToInt32(fileInfo[0].Split(' ')[1]);
                levelSettings.Level = new char[levelSettings.Height, levelSettings.Width];
                for(int i = 0; i < levelSettings.Height; i++)
                {
                    char[] tmp = fileInfo[i+1].ToCharArray();

                    for(int j = 0; j < levelSettings.Width; j++)
                    {
                        levelSettings.Level[i, j] = tmp[j];
                    }
                }

                Console.WriteLine(levelSettings.Width);
                Console.WriteLine(levelSettings.Height);
                for (int i = 0; i < levelSettings.Height; i++)
                {
                    for (int j = 0; j < levelSettings.Width; j++)
                    {
                        Console.Write(levelSettings.Level[i, j]);
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Некорректные данные в файле загрузки");
            }
        }
    }
}