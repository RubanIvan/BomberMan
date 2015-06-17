using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan
{

    /// <summary>Стороны объекта</summary>
    public enum Side
    {
        Left,Right,Up,Down
    }

    /// <summary>Состояния игры</summary>
    public enum State
    {
        MainMenu,PlayGame
    }

    public static class Rnd
    {
        static Random R = new Random(DateTime.Now.Millisecond);

        static Rnd()
        {
            for (int i = 0; i < 500; i++)
            {
                R.Next();
            }
        }

        public static int Next(int Max)
        {
            return R.Next(Max);
        }

        public static int Next()
        {
            return R.Next();
        }

        public static int Next(int Min, int Max)
        {
            return R.Next(Min, Max);
        }
    }
}
