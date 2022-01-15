using System;
using System.Collections.Generic;
using System.Text;

namespace TestSnake
{
    public class LevelSettings
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public char[,] Level { get; set; }
        public int RequiredFoodPoints { get; set; }
        public int FoodCount { get; set; }
        public int SnakePosX { get; set; }
        public int SnakePosY { get; set; }

        public LevelSettings()
        {

        }
    }
}
