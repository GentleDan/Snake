using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TestSnake
{
    public class Draw
    {
		private Random rnd = new Random();
		public bool GameOver;
		public int LevelScore;
        public int Score = 0;
        public int Step = 0;
        public int CurrentLevel = 1;
		private Snake snake;
        //private List<Food> foods;
        private Food food;
		private LoadLevel load;
		private LevelSettings settings;
		private char[,] Field;
		private static string config = "../../../../FirstLevel.txt";

		public void LoadGameSettings()
        {
			LevelScore = 0;
			load = new LoadLevel(config);
			settings = load.ReadInfo();
			GameOver = false;
            snake = new Snake
            {
                PosX = settings.SnakePosX,
                PosY = settings.SnakePosY,
                Size = 1,
                TailX = new int[settings.Width],
                TailY = new int[settings.Height],
                direction = Direction.Stop,
                GameSpeed = 250
			};
            food = new Food();
			Field = settings.Level;
		}

        private void FoodPosition()
        {
            food.PosX = rnd.Next(1, settings.Width - 1);
            food.PosY = rnd.Next(1, settings.Height - 1);
            if (food.PosX == 0 || food.PosY == 0 || food.PosX == settings.Width || food.PosY == settings.Height)
            {
                FoodPosition();
            }
            if(food.PosX == snake.PosX && food.PosY == snake.PosY)
            {
                FoodPosition();
            }
        }

        private void ChangeLevel()
        {
            config = "../../../../SecondLevel.txt";
            LoadGameSettings();
            CurrentLevel++;
        }

        private bool Eat()
        {
            if (snake.PosX == food.PosX && snake.PosY == food.PosY)
            {
                return true;
            }
            return false;
        }

        public void DrawField()
        {
            if (!GameOver)
            {
                Console.Clear();
                for (int i = 0; i < settings.Height; i++)
                {
                    for (int j = 0; j < settings.Width; j++)
                    {
                        if (i == snake.PosY && j == snake.PosX)
                        {
                            Field[i, j] = 'O';
                        }
                        else if (i == food.PosY && j == food.PosX)
                        {
                            Field[i, j] = 's';
                        }
                        else
                        {
                            bool print = false;
                            for (int k = 1; k < snake.Size; k++)
                            {
                                if (snake.TailX[k] == j && snake.TailY[k] == i && Field[i, j] != '#')
                                {
                                    Field[i, j] = 'o';
                                    print = true;
                                }
                            }
                            if (!print && Field[i, j] != '#')
                            {
                                Field[i, j] = '.';
                            }
                        }
                    }
                }
                for (int i = 0; i < settings.Height; i++)
                {
                    for (int j = 0; j < settings.Width; j++)
                    {
                        Console.Write(Field[i, j]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Уровень: " + CurrentLevel);
                Console.WriteLine("Счет: " + LevelScore);
                Console.WriteLine("Необходимо еды собрать: " + settings.RequiredFoodPoints);
                Console.WriteLine("Осталось еды собрать: " + (settings.RequiredFoodPoints - LevelScore));
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Игра окончена!");
                Console.WriteLine("Статистика: ");
                Console.WriteLine("Всего съедено: " + Score);
                Console.WriteLine("Пройдено клеток: " + Step);
                Console.WriteLine("Количество смертей: ");
            }
        }

        public void StartGame()
        {
            LoadGameSettings();
            FoodPosition();
            while (Game.direction != Direction.Stop && !GameOver)
            {
                snake.direction = Game.direction;
                snake.SnakeLogic();
                Step++;
                if (Eat())
                {
                    FoodPosition();
                    snake.Size++;
                    LevelScore++;
                    Score++;
                }
                if(LevelScore == settings.RequiredFoodPoints)
                {
                    ChangeLevel();
                    FoodPosition();
                    if(CurrentLevel == 3)
                    {
                        GameOver = true;
                    }
                }
                DrawField();
                Thread.Sleep(snake.GameSpeed);
            }
        }
    }
}
