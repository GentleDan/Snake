using System;
using System.Threading;

namespace TestSnake
{
    class Game
    {
        private static Draw draw;
        public static Direction direction;
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
                    case ConsoleKey.A:
                        direction = Direction.Left;
                        break;
                    case ConsoleKey.S:
                        direction = Direction.Down;
                        break;
                    case ConsoleKey.D:
                        direction = Direction.Right;
                        break;
                }
                return direction;
            }
            return Direction.Stop;
        }

        static void Main(string[] args)
        {
            direction = Direction.Up;
            draw = new Draw();
            new Thread(new ThreadStart(draw.StartGame)).Start();
            while (!draw.GameOver)
            {
                KeyListener();
            }
        }
    }
}
