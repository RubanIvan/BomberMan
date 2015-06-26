using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberMan
{

    enum WallEnum
    {
        Idle,
        /// <summary>Стена осыпается с лева</summary>
        DestroyLeft,
        /// <summary>Стена осыпается с права</summary>
        DestroyRight,
        /// <summary>Стена осыпается с верху</summary>
        DestroyUp,
        /// <summary>Стена осыпается с низу</summary>
        DestroyDown
    }


    class BrickWall:GameObject,Iexterminable
    {
        //конструктор
        public BrickWall(int x, int y) : base(x, y)
        {
            Zorder = Zorders.BrickWall;

            ObjectStates.Add(WallEnum.Idle, new State(new Animation(new List<Rectangle>() { new Rectangle(1 * 48, 0 * 48, 48, 48) })));

            ObjectStates.Add(WallEnum.DestroyLeft, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(0 * 48, 1 * 48, 48, 48),new Rectangle(1 * 48, 1 * 48, 48, 48),new Rectangle(2 * 48, 1 * 48, 48, 48),
                new Rectangle(3 * 48, 1 * 48, 48, 48),new Rectangle(4 * 48, 1 * 48, 48, 48),new Rectangle(5 * 48, 1 * 48, 48, 48),
                new Rectangle(6 * 48, 1 * 48, 48, 48),new Rectangle(7 * 48, 1 * 48, 48, 48),new Rectangle(8 * 48, 1 * 48, 48, 48),
                new Rectangle(9 * 48, 1 * 48, 48, 48),new Rectangle(0 * 48, 0 * 48, 48, 48)

            }, 100, true, false, Destroy)));


            ObjectStates.Add(WallEnum.DestroyRight, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(0 * 48, 2 * 48, 48, 48),new Rectangle(1 * 48, 2 * 48, 48, 48),new Rectangle(2 * 48, 2 * 48, 48, 48),
                new Rectangle(3 * 48, 2 * 48, 48, 48),new Rectangle(4 * 48, 2 * 48, 48, 48),new Rectangle(5 * 48, 2 * 48, 48, 48),
                new Rectangle(6 * 48, 2 * 48, 48, 48),new Rectangle(7 * 48, 2 * 48, 48, 48),new Rectangle(8 * 48, 2 * 48, 48, 48),
                new Rectangle(9 * 48, 2 * 48, 48, 48),new Rectangle(0 * 48, 0 * 48, 48, 48)

            }, 100, true, false, Destroy)));


            ObjectStates.Add(WallEnum.DestroyUp, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(0 * 48, 3 * 48, 48, 48),new Rectangle(1 * 48, 3 * 48, 48, 48),new Rectangle(2 * 48, 3 * 48, 48, 48),
                new Rectangle(3 * 48, 3 * 48, 48, 48),new Rectangle(4 * 48, 3 * 48, 48, 48),new Rectangle(5 * 48, 3 * 48, 48, 48),
                new Rectangle(6 * 48, 3 * 48, 48, 48),new Rectangle(7 * 48, 3 * 48, 48, 48),new Rectangle(8 * 48, 3 * 48, 48, 48),
                new Rectangle(9 * 48, 3 * 48, 48, 48),new Rectangle(0 * 48, 0 * 48, 48, 48)

            }, 100, true, false, Destroy)));


            ObjectStates.Add(WallEnum.DestroyDown, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(0 * 48, 4 * 48, 48, 48),new Rectangle(1 * 48, 4 * 48, 48, 48),new Rectangle(2 * 48, 4 * 48, 48, 48),
                new Rectangle(3 * 48, 4 * 48, 48, 48),new Rectangle(4 * 48, 4 * 48, 48, 48),new Rectangle(5 * 48, 4 * 48, 48, 48),
                new Rectangle(6 * 48, 4 * 48, 48, 48),new Rectangle(7 * 48, 4 * 48, 48, 48),new Rectangle(8 * 48, 4 * 48, 48, 48),
                new Rectangle(9 * 48, 4 * 48, 48, 48),new Rectangle(0 * 48, 0 * 48, 48, 48)

            }, 100, true, false, Destroy)));

            ChangeState(WallEnum.Idle);
        }

        /// <summary>Функция обратного вызова вызывается при завершении анимации разрушения стены</summary>
        public void Destroy()
        {
            isAlive = false;
        }

        public void Blow(BlowSide side)
        {
            //если стена еще не разрушается
            if ((WallEnum) SMstate == WallEnum.Idle)
            {
                switch (side)
                {
                    case BlowSide.Left:
                        ChangeState(WallEnum.DestroyLeft);
                        break;
                    case BlowSide.Right:
                        ChangeState(WallEnum.DestroyRight);
                        break;
                    case BlowSide.Up:
                        ChangeState(WallEnum.DestroyUp);
                        break;
                    case BlowSide.Down:
                        ChangeState(WallEnum.DestroyDown);
                        break;
                    
                }
            }
        }
    }
}
