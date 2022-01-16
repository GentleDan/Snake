using System;
using System.Threading;

namespace TestSnake
{
    class Game
    {
        private static LoadLevel load;
        private static LevelSettings settings;
        private static int Score;
        private static bool IsStarted;
        private static GameObjectsProps gameObjects;
        private static Direction direction;
        private static readonly string config = "../../../../FirstLevel.txt";

        public static void LoadGameSettings()
        {
            Score = 0;
            Random rnd = new Random();
            load = new LoadLevel(config);
            settings = load.ReadInfo();
            gameObjects = new GameObjectsProps
            {
                SnakePosX = settings.SnakePosX,
                SnakePosY = settings.SnakePosY
            };
            IsStarted = true;
            direction = Direction.Stop;
        }

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

        public static void GameLogic()
        {
            switch (direction)
            {
                case Direction.Up:
                    gameObjects.SnakePosY--;
                    break;
                case Direction.Left:
                    gameObjects.SnakePosX--;
                    break;
                case Direction.Down:
                    gameObjects.SnakePosY++;
                    break;
                case Direction.Right:
                    gameObjects.SnakePosX++;
                    break;
            }
        }

        public static void DrawField()
        {
            Console.Clear();
            char[,] field = settings.Level;
            for (int i = 0; i < settings.Height; i++)
            {
                for (int j = 0; j < settings.Width; j++)
                {
                    if (i == gameObjects.SnakePosY && j == gameObjects.SnakePosX)
                    {
                        field[i, j] = 'O';
                    }
                    else if (settings.Level[i, j] != '#' && field[i, j] != 's')
                    {
                        field[i, j] = '.';
                    }
                    Console.Write(field[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            LoadGameSettings();
            while(IsStarted)
            {
                DrawField();
                KeyListener();
                GameLogic();
                Thread.Sleep(250);
            }
        }
    }
}
