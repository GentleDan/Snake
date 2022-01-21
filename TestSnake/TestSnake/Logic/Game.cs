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
                        draw.CheckGameLogic();
                        break;
                    case ConsoleKey.UpArrow:
                        direction = Direction.Up;
                        draw.CheckGameLogic();
                        break;
                    case ConsoleKey.A:
                        direction = Direction.Left;
                        draw.CheckGameLogic();
                        break;
                    case ConsoleKey.LeftArrow:
                        direction = Direction.Left;
                        draw.CheckGameLogic();
                        break;
                    case ConsoleKey.S:
                        direction = Direction.Down;
                        draw.CheckGameLogic();
                        break;
                    case ConsoleKey.DownArrow:
                        direction = Direction.Down;
                        draw.CheckGameLogic();
                        break;
                    case ConsoleKey.D:
                        direction = Direction.Right;
                        draw.CheckGameLogic();
                        break;
                    case ConsoleKey.RightArrow:
                        direction = Direction.Right;
                        draw.CheckGameLogic();
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
            direction = Direction.Stop;
            draw = new Draw();
            draw.StartGame();
            while (!draw.IsGameOver)
            {
                KeyListener();
            }
        }
    }
}
