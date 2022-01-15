using System;
using System.Threading;

namespace TestSnake
{
    class Game
    {
        private static LoadLevel load;

        private static string config = "../../../../FirstLevel.txt";
        public static void Load() 
        {
            load = new LoadLevel(config);
            load.ReadInfo();
        }


        static void Main(string[] args)
        {
            Load();
        }
    }
}
