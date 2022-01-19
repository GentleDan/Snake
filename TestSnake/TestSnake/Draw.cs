using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TestSnake
{
    public class Draw
    {
        private Random rnd;
		public bool GameOver;
        private int LevelScore = 0;
        private int TotalScore = 0;
        private int DeathCount = 0;
        private int Step = 0;
        private int CurrentLevel = 1;
        private int SnakeLife = 3;
        private List<Wall> WallsCoordinates;
        private Snake snake;
        private List<Food> foods;
		private LoadLevel load;
		private LevelSettings settings;
		private char[,] Field;
		private static string config = "../../../../FirstLevel.txt";

		public void LoadGameSettings()
        {
            rnd = new Random();
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
                GameSpeed = 250,
                Life = SnakeLife
			};
            foods = new List<Food>();
            Field = settings.Level;
            StartFoodPosition();
        }

        private void StartFoodPosition()
        {
            for (int i = 0; i < settings.FoodCount; i++)
            {
                foods.Add(new Food { IsEaten = false });
            }
            foreach (var f in foods)
            {
                f.PosX = rnd.Next(1, settings.Width - 1);
                f.PosY = rnd.Next(1, settings.Height - 1);
                if (f.PosX == 0 || f.PosY == 0 || f.PosX == settings.Width || f.PosY == settings.Height || Field[f.PosY, f.PosX] == '#')
                {
                    StartFoodPosition();
                }
                if (f.PosX == snake.PosX && f.PosY == snake.PosY)
                {
                    StartFoodPosition();
                }
            }
        }

        private bool CheckFail()
        {
            foreach(var wall in WallsCoordinates)
            {
                if(snake.PosY == wall.PosY && snake.PosX == wall.PosX)
                {
                    return true;
                }
                for(int i = 1; i < snake.Size; i++)
                {
                    if(snake.PosX == snake.TailX[i] && snake.PosY == snake.TailY[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void GetWallsCoordinates()
        {
            WallsCoordinates = new List<Wall>();
            for(int i = 0; i < settings.Height; i++)
            {
                for(int j = 0; j < settings.Width; j++)
                {
                    if(Field[i,j] == '#')
                    {
                        WallsCoordinates.Add(new Wall { PosX = j, PosY = i });
                    }
                }
            }
        }

        private void ChangeFoodPosition()
        {
            foreach (var f in foods)
            {
                if (f.IsEaten)
                {
                    f.PosX = rnd.Next(1, settings.Width - 1);
                    f.PosY = rnd.Next(1, settings.Height - 1);
                    if (f.PosX == 0 || f.PosY == 0 || f.PosX == settings.Width || f.PosY == settings.Height || Field[f.PosY, f.PosX] == '#')
                    {
                        ChangeFoodPosition();
                    }
                    if (f.PosX == snake.PosX && f.PosY == snake.PosY)
                    {
                        ChangeFoodPosition();
                    }
                    f.IsEaten = false;
                }
            }
        }

        private void ChangeLevel()
        {
            config = "../../../../SecondLevel.txt";
            LoadGameSettings();
            GetWallsCoordinates();
            LevelScore = 0;
            CurrentLevel++;
        }

        private bool Eat()
        {
            foreach (var f in foods)
            {
                if (snake.PosX == f.PosX && snake.PosY == f.PosY)
                {
                    f.IsEaten = true;
                    return true;
                }
            }
            return false;
        }

        public void DrawField()
        {
            if (!GameOver)
            {
                Console.SetCursorPosition(0,0);
                for (int i = 0; i < settings.Height; i++)
                {
                    for (int j = 0; j < settings.Width; j++)
                    {
                        foreach (var f in foods)
                        {
                            if (i == f.PosY && j == f.PosX && !f.IsEaten)
                            {
                                Field[i, j] = 's';
                            }
                            else if (i == snake.PosY && j == snake.PosX)
                            {
                                Field[i, j] = 'O';
                            }
                        }
                        if (i == snake.PosY && j == snake.PosX && Field[i,j] != 's')
                        {
                            Field[i, j] = 'O';
                        }
                        else
                        {
                            bool print = false;
                            for (int k = 1; k < snake.Size; k++)
                            {
                                if (snake.TailX[k] == j && snake.TailY[k] == i && Field[i, j] != '#' && Field[i, j] != 's')
                                {
                                    Field[i, j] = 'o';
                                    print = true;
                                }
                            }
                            if (!print && Field[i, j] != '#' && Field[i, j] != 's')
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
                Console.WriteLine("Жизни: " + snake.Life);
                Console.WriteLine("Счет: " + LevelScore);
                Console.WriteLine("Необходимо еды собрать: " + settings.RequiredFoodPoints);
                Console.WriteLine("Осталось еды собрать: " + (settings.RequiredFoodPoints - LevelScore));
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Игра окончена!");
                Console.WriteLine("Статистика");
                Console.WriteLine("Всего съедено: " + TotalScore);
                Console.WriteLine("Пройдено клеток: " + Step);
                Console.WriteLine("Количество смертей: " + DeathCount);
            }
        }

        public void StartGame()
        {
            LoadGameSettings();
            GetWallsCoordinates();
            ChangeFoodPosition();
            while (Game.direction != Direction.Stop && !GameOver)
            {
                snake.direction = Game.direction;
                snake.SnakeLogic();
                Step++;
                if (Eat())
                {
                    ChangeFoodPosition();
                    snake.Size++;
                    LevelScore++;
                    TotalScore++;
                }
                if (CheckFail())
                {
                    SnakeLife--;
                    DeathCount++;
                    StartGame();
                }
                if(SnakeLife == 0)
                {
                    GameOver = true;
                }
                if(LevelScore == settings.RequiredFoodPoints)
                {
                    ChangeLevel();
                    ChangeFoodPosition();
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
