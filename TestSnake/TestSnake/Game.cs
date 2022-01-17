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
                var key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case 'w':
                        direction = Direction.Up;
                        break;
                    case 'a':
                        direction = Direction.Left;
                        break;
                    case 's':
                        direction = Direction.Down;
                        break;
                    case 'd':
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
