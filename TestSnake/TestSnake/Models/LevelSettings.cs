namespace TestSnake
{
    /// <summary>
    /// Класс LevelSettings. Модель игровых настроек, которые
    /// читаем из JSON файла
    /// </summary>
    public class LevelSettings
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string[,] Level { get; set; }
        public int RequiredFoodPoints { get; set; }
        public int FoodCount { get; set; }
        public int SnakePosX { get; set; }
        public int SnakePosY { get; set; }
    }
}
