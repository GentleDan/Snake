namespace TestSnake
{
    /// <summary>
    /// Класс GameJsonConfig. Модель объекта конфигурации,
    /// которую читаем из JSON файла
    /// </summary>
    public class GameJsonConfig
    {
        public int LifeCount { get; set; }
        public int SnakeSize { get; set; }
    }
}
