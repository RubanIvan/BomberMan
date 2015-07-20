using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberMan
{
    /// <summary>Положение камеры в мировых координатах</summary>
    class VievCam
    {
        /// <summary>ссылка на игрока </summary>
        public CPlayer Player;

        //координаты
        public  int X;
        public  int Y;

        //размер
        public const int DX = Const.ScrDx;
        public const int DY = Const.ScrDy;

        public const int halfDX = Const.ScrDx/2;
        public const int halfDY = Const.ScrDy/2;

        public int MaxX;
        public int MaxY;


        /// <summary>Конструктор</summary>
        /// <param name="LevDx">Длинна уровня</param>
        /// <param name="LevDy">Длинна уровня</param>
        /// <param name="player">Ссылка на игрока</param>
        public VievCam(int LevDx,int LevDy, CPlayer player)
        {
            Player = player;
            MaxX = LevDx - DX;
            MaxY = LevDy - DY;

        }

        /// <summary>Пересчитываем камеру</summary>
        public void Update(GameTime gameTime)
        {
            X = Player.PosWorldX - halfDX;
            Y = Player.PosWorldY - halfDY;

            if (X < 0) X = 0;
            if (Y < 0) Y = 0;

            if (X > MaxX) X = MaxX;
            if (Y > MaxY) Y = MaxY;
        }


    }
}
