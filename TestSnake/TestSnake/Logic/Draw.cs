using System;
using System.Collections.Generic;
using System.Threading;

namespace TestSnake
{
    /// <summary>
    ///  Класс Draw.
    ///  Класс, обрабатывающий логику и отрисовку игры
    /// </summary>
    public class Draw
    {
        private readonly string LevelConfig = "../../../../Levels/GameLvl_"; // файл с игровым полем
        private static readonly string JsonConfig = "../../../../Config/GameConfig.json"; // путь к файлу с конфигурацией размера змейки и количества жизней
        private int LvlCount; // кол-во уровней в папке
        private int TotalScore; //общий игровой счет
        private int DeathCount; //счетчик смертей
        private bool PrintTail; //флаговая переменная для отрисовки хвоста змейки (если false, то отрисовывается пустое пространство)
        private int Step; //счетчик общего количества пройденных клеток
        private int CurrentLevel; //общий игровой счет
        private int SnakeLife; //общий игровой счет
        public bool IsGameOver; //переменная-флаг, обозначающая конец игры
        private int LevelScore; //игровой счет на конкретном уровне
        private char[,] Field; //игровое поле
        private Random rnd; //объект класса Random
        private GameJsonConfig gameJsonConfig; // json конфигурация размера змейки и количества жизней
        private List<Wall> WallsCoordinates; //
        private Snake snake; // змейка
        private List<Food> foods; // список еды
        private LoadLevel load; //объект, вызывающий метод загрузки данных из файла
        private LevelSettings settings; // объект настроек игры


        /// <summary>
        /// Метод LoadGameSettings() загружает данные для отрисовки и обработки
        /// </summary>
        public void LoadGameSettings()
        {
            LvlCount = System.IO.Directory.GetFiles("../../../../Levels").Length;
            PrintTail = false;
            load = new LoadLevel(LevelConfig + CurrentLevel + ".json", JsonConfig);
            //загрузка настроек уровня из файла
            settings = load.ReadLevelInfo();
            gameJsonConfig = load.LoadConfig();
            LevelScore = 0;
            rnd = new Random();
            IsGameOver = false;
            SnakeLife = gameJsonConfig.LifeCount;
            snake = new Snake
            {
                PosX = settings.SnakePosX,
                PosY = settings.SnakePosY,
                Size = gameJsonConfig.SnakeSize,
                TailX = new int[settings.Width],
                TailY = new int[settings.Height],
                direction = Direction.Stop,
                GameSpeed = 250,
                Life = SnakeLife
            };
            foods = new List<Food>();
            Field = load.ParseLevel(settings.Level, settings);
            CreateFood();
            StartFoodPosition();
        }

        /// <summary>
        /// Метод StartFoodPosition() устанавливает 
        /// первоначальные координаты еды
        /// </summary>
        private void StartFoodPosition()
        {

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

        /// <summary>
        /// Метод CreateFood() очищает список еды
        /// и создает новые объекты
        /// </summary>
        private void CreateFood()
        {
            foods.Clear();
            for (int i = 0; i < settings.FoodCount; i++)
            {
                foods.Add(new Food { IsEaten = false });
            }
        }

        /// <summary>
        /// Метод IsFail() проверяет
        /// врезалась ли змейка в стену 
        /// или в свой хвост
        /// </summary>
        private bool IsFail()
        {
            foreach (var wall in WallsCoordinates)
            {
                if (snake.PosY == wall.PosY && snake.PosX == wall.PosX)
                {
                    return true;
                }
                for (int i = 1; i < snake.Size; i++)
                {
                    if (snake.PosX == snake.TailX[i] && snake.PosY == snake.TailY[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Метод GetWallsCoordinates() получает
        /// координаты всех стен на уровне
        /// </summary>
        private void GetWallsCoordinates()
        {
            WallsCoordinates = new List<Wall>();
            for (int i = 0; i < settings.Height; i++)
            {
                for (int j = 0; j < settings.Width; j++)
                {
                    if (Field[i, j] == '#')
                    {
                        WallsCoordinates.Add(new Wall { PosX = j, PosY = i });
                    }
                }
            }
        }

        /// <summary>
        /// Метод ChangeFoodPosition() меняет
        /// координаты съеденной еды
        /// </summary>
        private void ChangeFoodPosition()
        {
            foreach (var f in foods)
            {
                if (f.IsEaten)
                {
                    f.PosX = rnd.Next(1, settings.Width - 1);
                    f.PosY = rnd.Next(1, settings.Height - 1);
                    //если выданные рандомные координаты попадают на змейку или стенку - вызываем метод еще раз
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

        /// <summary>
        /// Метод ChangeLevel() меняет
        /// уровень и вызывает метод подгрузки
        /// данных
        /// </summary>
        private void ChangeLevel()
        {
            Console.Clear();
            CurrentLevel++;
            if (CurrentLevel <= LvlCount)
            {
                LoadGameSettings();
                GetWallsCoordinates();
            }
            SnakeLife -= DeathCount;
        }

        /// <summary>
        /// Метод IsEat() проверяет
        /// были ли съедена еда
        /// </summary>
        private bool IsEat()
        {
            foreach (var f in foods)
            {
                if (snake.PosX == f.PosX && snake.PosY == f.PosY)
                {
                    //если еда съедена - меняем значение свойства IsEaten
                    f.IsEaten = true;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Метод DrawField() отрисовывает игровое поле
        /// и статистику
        /// </summary>
        public void DrawField()
        {
            if (!IsGameOver)
            {
                Console.SetCursorPosition(0, 0);
                //изменение массива игрового поля
                for (int i = 0; i < settings.Height; i++)
                {
                    for (int j = 0; j < settings.Width; j++)
                    {
                        DrawFood(i, j);
                        if (j < foods.Count && i == foods[j].PosY && j == foods[j].PosX && !foods[j].IsEaten)
                        {
                            Field[i, j] = 's';
                        }
                        else if (i == snake.PosY && j == snake.PosX && Field[i, j] != 's')
                        {
                            Field[i, j] = 'O';
                        }
                        else
                        {
                            PrintTail = false;
                            DrawTail(i, j);
                            if (!PrintTail && Field[i, j] != '#' && Field[i, j] != 's')
                            {
                                Field[i, j] = '.';
                            }
                        }
                    }
                }
                //отрисовка массива
                for (int i = 0; i < settings.Height; i++)
                {
                    for (int j = 0; j < settings.Width; j++)
                    {
                        Console.Write(Field[i, j]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Уровень: " + CurrentLevel + "\n" +
                                  "Жизни: " + SnakeLife + "\n" +
                                  "Счет: " + LevelScore + "\n" +
                                  "Необходимо еды собрать: " + settings.RequiredFoodPoints);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Игра окончена!" + "\n" +
                                  "Статистика" + "\n" +
                                  "Всего съедено: " + TotalScore + "\n" +
                                  "Пройдено клеток: " + Step + "\n" +
                                  "Количество смертей: " + DeathCount);
                return;
            }
        }

        /// <summary>
        /// Метод DrawFood() является вспомогательным методом
        /// для метода DrawField(). Отвечает за отрисовку еды
        /// </summary>
        /// <param name="i">Итерация первого цикла отрисовки</param>
        /// <param name="j">Итерация второго цикла отрисовки</param>
        public void DrawFood(int i, int j)
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
        }

        /// <summary>
        /// Метод DrawTail() является вспомогательным методом
        /// для метода DrawField(). Отвечает за отрисовку хвоста змейки
        /// </summary>
        /// <param name="i">Итерация первого цикла отрисовки</param>
        /// <param name="j">Итерация второго цикла отрисовки</param>
        public void DrawTail(int i, int j)
        {
            for (int k = 1; k < snake.Size; k++) //k=1, чтобы не учитывать голову змейки
            {
                if (snake.TailX[k] == j && snake.TailY[k] == i && Field[i, j] != '#' && Field[i, j] != 's')
                {
                    PrintTail = true;
                    Field[i, j] = 'o';
                }
            }
        }

        /// <summary>
        /// Метод CheckGameLogic() - логика игры. Вызывает
        /// методы смены уровней, проверки столкновения,
        /// проверки поедания и отрисовки
        /// </summary>
        public void CheckGameLogic()
        {
            while (!IsGameOver)
            {
                snake.direction = Game.direction;
                snake.SnakeLogic();
                Step++;
                if (IsEat())
                {
                    ChangeFoodPosition();
                    snake.Size++;
                    LevelScore++;
                    TotalScore++;
                }
                if (SnakeLife == 0)
                {
                    StartGame();
                }
                if (IsFail())
                {
                    DeathCount++;
                    LoadGameSettings();
                    SnakeLife -= DeathCount;
                }
                if (LevelScore == settings.RequiredFoodPoints)
                {
                    ChangeLevel();
                    ChangeFoodPosition();
                    if (CurrentLevel == 3)
                    {
                        IsGameOver = true;
                    }
                }
                DrawField();
                //приостановка потока. используется вместо объявления и инициализации объекта таймера
                Thread.Sleep(snake.GameSpeed);
            }
        }

        /// <summary>
        /// Метод StartGame() вызывает методы загрузки настроек уровня,
        /// и статистику
        /// </summary>
        public void StartGame()
        {
            TotalScore = 0;
            DeathCount = 0;
            Step = 0;
            CurrentLevel = 1;
            //отключаем видимость курсора, чтобы картинка не мерцала
            Console.CursorVisible = false;
            LoadGameSettings();
            GetWallsCoordinates();
            CheckGameLogic();
        }
    }
}
