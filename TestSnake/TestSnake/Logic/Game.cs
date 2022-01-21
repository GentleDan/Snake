using System;
using System.Threading;

namespace TestSnake
{
    /// <summary>
    /// Класс Game. Основной класс программы,
    /// где осуществляется вызов методов класса
    /// Draw, а также управление змейкой
    /// </summary>
    class Game
    {
        private static Draw draw;
        public static Direction direction;

        /// <summary>
        /// Метод KeyListener(). Листинер нажатия клавиш
        /// пользователем для изменения движения змейки
        /// </summary>
        public static Direction KeyListener()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.W:
                        direction = Direction.Up;
                        break;
                    case ConsoleKey.UpArrow:
                        direction = Direction.Up;
                        break;
                    case ConsoleKey.A:
                        direction = Direction.Left;
                        break;
                    case ConsoleKey.LeftArrow:
                        direction = Direction.Left;
                        break;
                    case ConsoleKey.S:
                        direction = Direction.Down;
                        break;
                    case ConsoleKey.DownArrow:
                        direction = Direction.Down;
                        break;
                    case ConsoleKey.D:
                        direction = Direction.Right;
                        break;
                    case ConsoleKey.RightArrow:
                        direction = Direction.Right;
                        break;
                }
                return direction;
            }
            return Direction.Stop;
        }
        /// <summary>
        /// Метод Main(). Точка входа программы
        /// </summary>
        static void Main(string[] args)
        {
            direction = Direction.Up;
            draw = new Draw();
            new Thread(new ThreadStart(draw.StartGame)).Start(); //запуск нового потока для отрисовки игры
            while (!draw.IsGameOver)
            {
                KeyListener();
            }
        }
    }
}
