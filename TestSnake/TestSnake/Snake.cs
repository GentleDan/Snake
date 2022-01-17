using System;
using System.Collections.Generic;
using System.Text;

namespace TestSnake
{
    public class Snake
    {
        public Direction direction;
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Size { get; set; }
        public int GameSpeed { get; set; }
        public int Life { get; set; }
        public int[] TailX;
        public int[] TailY;

        public void SnakeLogic()
        {
            TailX[0] = PosX;
            TailY[0] = PosY;
            int prevTailX = TailX[0];
            int prevTailY = TailY[0];
            int tmpX, tmpY;
            for (int i = 1; i < Size; i++)
            {
                tmpX = TailX[i];
                tmpY = TailY[i];
                TailX[i] = prevTailX;
                TailY[i] = prevTailY;
                prevTailX = tmpX;
                prevTailY = tmpY;
            }
            switch (direction)
            {
                case Direction.Up:
                    PosY--;
                    break;
                case Direction.Left:
                    PosX--;
                    break;
                case Direction.Down:
                    PosY++;
                    break;
                case Direction.Right:
                    PosX++;
                    break;
            }
        }
    }
}
