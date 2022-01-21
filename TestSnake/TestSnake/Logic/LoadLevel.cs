using Newtonsoft.Json;
using System;
using System.IO;

namespace TestSnake
{
    /// <summary>
    /// Класс LoadLevel. Отвечает за чтение данных уровня
    /// и конфигурации из JSON
    /// </summary>
    public class LoadLevel
    {
        public string ConfigFilePath { get; set; } // путь к файлу с настройками уровня
        public string JsonConfigFilePath { get; set; } // путь к файлу с дополнительной конфигурацией

        public LoadLevel(string config, string jsonConfig)
        {
            ConfigFilePath = config;
            JsonConfigFilePath = jsonConfig;
        }

        /// <summary>
        /// Метод ReadInfo(). Отвечает за чтение данных уровня
        /// и конфигурации из JSON
        /// </summary>
        public LevelSettings ReadLevelInfo()
        {
            LevelSettings level = new LevelSettings();
            try
            {
                var data = JsonConvert.DeserializeObject<LevelSettings>(File.ReadAllText(ConfigFilePath)); //десериализация объекта
                level.Width = data.Width;
                level.Height = data.Height;
                level.Level = data.Level;
                level.RequiredFoodPoints = data.RequiredFoodPoints;
                level.FoodCount = data.FoodCount;
                level.SnakePosX = data.SnakePosX;
                level.SnakePosY = data.SnakePosY;
                CheckInputData(level.RequiredFoodPoints, level.FoodCount);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return level;
        }


        /// <summary>
        /// Метод ParseLevel(). Преобразовывает тип массива с уровнем со string на char
        /// </summary>
        /// <param name="array">Массив, считываемый из JSON</param>
        /// <param name="level">Объект класса игровых настроек</param>
        public char[,] ParseLevel(string[,] array, LevelSettings level)
        {
            int firstColumn = 0; // указываем, что нужно бегать по первому столбцу из массива уровня, после получение объекта из json
            char[,] parsedLevel = new char[level.Width, level.Height];
            for (int i = 0; i < level.Height; i++)
            {
                char[] tmp = array[i, firstColumn].ToCharArray();

                for (int j = 0; j < level.Width; j++)
                {
                    parsedLevel[i, j] = tmp[j];
                }
            }
            return parsedLevel;
        }

        /// <summary>
        /// Метод CheckInputData(). Проверяет входящие данные из файла
        /// </summary>
        /// <param name="reqFood">Требуемое кол-во еды для прохождения уровня из файла</param>
        /// <param name="foodCount">Кол-во еды из файла, одновременно находящееся на игровом поле</param>
        public void CheckInputData(int reqFood, int foodCount)
        {
            if (reqFood < 1)
            {
                throw new Exception("Минимально требуемое количество еды должно быть не меньше 1");
            }
            if (foodCount < 1)
            {
                throw new Exception("Минимально количество еды на поле должно быть не меньше 1");
            }
        }

        /// <summary>
        /// Метод LoadConfig(). Подгружает доп конфигурацию из JSON 
        /// </summary>
        public GameJsonConfig LoadConfig()
        {
            GameJsonConfig gameJsonConfig = new GameJsonConfig();
            try
            {
                var data = JsonConvert.DeserializeObject<GameJsonConfig>(File.ReadAllText(JsonConfigFilePath));
                gameJsonConfig.LifeCount = data.LifeCount;
                gameJsonConfig.SnakeSize = data.SnakeSize;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return gameJsonConfig;
        }
    }
}