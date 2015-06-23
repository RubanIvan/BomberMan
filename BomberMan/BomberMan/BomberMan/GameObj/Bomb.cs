using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberMan.GameObj
{
    /// <summary>В какой стороне прогримел взрыв</summary>
    public enum BlowSide
    {
        Left, Right, Up, Down
    }

    public enum BombEnum
    {
        /// <summary>Предвзрывное раздутие </summary>
        Inflation,
        /// <summary>Взрыв</summary>
        Explosion
    }

    public class Bomb : GameObject
    {

        protected int ExplPower = 3;

        public Bomb(int x, int y, List<GameObject> O)
            : base(x, y)
        {
            GameObjects = O;

            ObjectStates.Add(BombEnum.Inflation, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(0 * 48, 5 * 48, 48, 48),new Rectangle(0 * 48, 5 * 48, 48, 48),new Rectangle(1 * 48, 5 * 48, 48, 48),new Rectangle(2 * 48, 5 * 48, 48, 48),
                new Rectangle(3 * 48, 5 * 48, 48, 48),new Rectangle(4 * 48, 5 * 48, 48, 48),new Rectangle(5 * 48, 5 * 48, 48, 48),
                new Rectangle(6 * 48, 5 * 48, 48, 48),new Rectangle(7 * 48, 5 * 48, 48, 48),new Rectangle(8 * 48, 5 * 48, 48, 48),
                new Rectangle(9 * 48, 5 * 48, 48, 48),new Rectangle(10 * 48, 5 * 48, 48, 48),new Rectangle(11 * 48, 5 * 48, 48, 48)
            }, 100, true, false, Explosion)));

            ChangeState(BombEnum.Inflation);
        }

        public void Explosion()
        {
            //конец цепочки взрывов
            bool ExplEnd = false;

            //координаты правого рукова взрыва
            //List<Point> ExpListRight = new List<Point>();
            List<GameObject> ExpListRight = new List<GameObject>();

            for (int i = 0; i < ExplPower && !ExplEnd; i++)
            {
                foreach (GameObject O in GameObjects)
                {
                    if (O.PosWorldX == PosWorldX + (48 * i) && O.PosWorldY == PosWorldY)
                    {
                        if (O is StoneWall || O is SteelWall) { ExplEnd = true; break; }

                        if (O is BrickWall)
                        {
                            ExplEnd = true;
                            ExpListRight.Add(O);
                            break;
                        }
                        ExpListRight.Add(O);
                    }
                }
            }


            if (ExpListRight.Count > 0)
            {
                //for (int i = 0; i < ExpListRight.Count-2; i++)
                //{
                //    GameObjects.Add(new ExplRight(ExpListRight[i].X, ExpListRight[i].Y,GameObjects));
                //}

                //GameObjects.Add(new ExplRight(ExpListRight.Last().X, ExpListRight.Last().Y, GameObjects));
            }

        }

        //public void Explosion()
        //{
        //    //объект бомба надо удалить
        //    isAlive = false;

        //    //Ставим центр взрыва
        //    GameObjects.Add(new ExplCentr(PosWorldX, PosWorldY, GameObjects));

        //    //флаг если концовка у взрыва
        //    bool ExplEnd = true;

        //    //расставляем взрывы по длинне
        //    for (int i = 0; i < ExplPower; i++)
        //    {
        //        GameObjects.Find(o => 
        //        {
        //            if (o.PosWorldX == PosWorldX + (48*i) && o.PosWorldY == PosWorldY)
        //            {
        //                if (o is StoneWall || o is SteelWall ){ExplEnd = false;return true;}

        //                if (o is BrickWall)
        //                {
        //                    o.ChangeState(WallEnum.DestroyLeft);
        //                    //концовки взрыва не будет уперлись в сену
        //                    ExplEnd = false;
        //                    GameObjects.Add(new ExplRightEnd(PosWorldX + (48 * i), PosWorldY, GameObjects));
        //                    return true;
        //                }
        //            }
        //            return false;
        //        });
        //        //если в предыдущих итерациях наткнулись на стену то выходим
        //        if(!ExplEnd)break;

        //        GameObjects.Add(new ExplRight(PosWorldX + (48 * i), PosWorldY, GameObjects));
        //    }

        //    //расставляем концовку взрыва
        //    if (ExplEnd)
        //    {
        //        GameObjects.Find(o =>
        //            {
        //                if (o.PosWorldX == PosWorldX + (48 * ExplPower) && o.PosWorldY == PosWorldY)
        //                {
        //                    if (o is StoneWall || o is SteelWall) { ExplEnd = false; return true; }

        //                    if (o is BrickWall)
        //                    {
        //                        o.ChangeState(WallEnum.DestroyLeft);
        //                        GameObjects.Add(new ExplRightEnd(PosWorldX + (48 * ExplPower), PosWorldY, GameObjects));
        //                        ExplEnd = false;
        //                        return true;
        //                    }

        //                }
        //                return false;
        //            });

        //        if (ExplEnd) GameObjects.Add(new ExplRightEnd(PosWorldX + (48 * ExplPower), PosWorldY, GameObjects));

        //    }
        //}
    }

    /// <summary>Центр взрыва</summary>
    public class ExplCentr : GameObject
    {

        public ExplCentr(int x, int y, List<GameObject> O)
            : base(x, y)
        {
            GameObjects = O;

            ObjectStates.Add(BombEnum.Inflation, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(12 * 48, 2 * 48, 48, 48),
                new Rectangle(14 * 48, 5 * 48, 48, 48),
                new Rectangle(12 * 48, 8 * 48, 48, 48),
                new Rectangle(14 * 48, 11 * 48, 48, 48),
                new Rectangle(12 * 48, 14 * 48, 48, 48),
                new Rectangle(13 * 48, 17 * 48, 48, 48),
                new Rectangle(12 * 48, 20 * 48, 48, 48),
                new Rectangle(13 * 48, 23 * 48, 48, 48),
                new Rectangle(12 * 48, 26 * 48, 48, 48)
                
            }, 100, true, false, Final)));

            ChangeState(BombEnum.Inflation);
        }

        public void Final()
        {
            isAlive = false;
        }
    }

    /// <summary>Верх взрыва (не конец) </summary>
    public class ExplUp : GameObject
    {

        public ExplUp(int x, int y, List<GameObject> O)
            : base(x, y)
        {
            GameObjects = O;

            ObjectStates.Add(BombEnum.Inflation, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(12 * 48, 1 * 48, 48, 48),
                new Rectangle(14 * 48, 4 * 48, 48, 48),
                new Rectangle(12 * 48, 7 * 48, 48, 48),
                new Rectangle(14 * 48, 10 * 48, 48, 48),
                new Rectangle(12 * 48, 13 * 48, 48, 48),
                new Rectangle(13 * 48, 16 * 48, 48, 48),
                new Rectangle(12 * 48, 19 * 48, 48, 48),
                new Rectangle(13 * 48, 22 * 48, 48, 48),
                new Rectangle(12 * 48, 25 * 48, 48, 48)
                
            }, 100, true, false, Final)));

            ChangeState(BombEnum.Inflation);
        }

        public void Final()
        {
            isAlive = false;
        }
    }

    /// <summary>Верх взрыва конец </summary>
    public class ExplUpEnd : GameObject
    {

        public ExplUpEnd(int x, int y, List<GameObject> O)
            : base(x, y)
        {
            GameObjects = O;

            ObjectStates.Add(BombEnum.Inflation, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(12 * 48, 0 * 48, 48, 48),
                new Rectangle(14 * 48, 3 * 48, 48, 48),
                new Rectangle(12 * 48, 6 * 48, 48, 48),
                new Rectangle(14 * 48, 9 * 48, 48, 48),
                new Rectangle(12 * 48, 12 * 48, 48, 48),
                new Rectangle(13 * 48, 15 * 48, 48, 48),
                new Rectangle(12 * 48, 18 * 48, 48, 48),
                new Rectangle(13 * 48, 21 * 48, 48, 48),
                new Rectangle(12 * 48, 24 * 48, 48, 48)
                
            }, 100, true, false, Final)));

            ChangeState(BombEnum.Inflation);
        }

        public void Final()
        {
            isAlive = false;
        }
    }

    /// <summary>Низ взрыва не конец</summary>
    public class ExplDown : GameObject
    {

        public ExplDown(int x, int y, List<GameObject> O)
            : base(x, y)
        {
            GameObjects = O;

            ObjectStates.Add(BombEnum.Inflation, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(12 * 48, 3 * 48, 48, 48),
                new Rectangle(14 * 48, 6 * 48, 48, 48),
                new Rectangle(12 * 48, 9 * 48, 48, 48),
                new Rectangle(14 * 48, 12 * 48, 48, 48),
                new Rectangle(12 * 48, 15 * 48, 48, 48),
                new Rectangle(13 * 48, 18 * 48, 48, 48),
                new Rectangle(12 * 48, 21 * 48, 48, 48),
                new Rectangle(13 * 48, 24 * 48, 48, 48),
                new Rectangle(12 * 48, 27 * 48, 48, 48)
                
            }, 100, true, false, Final)));

            ChangeState(BombEnum.Inflation);
        }

        public void Final()
        {
            isAlive = false;
        }
    }

    /// <summary>Низ взрыва конец</summary>
    public class ExplDownEnd : GameObject
    {

        public ExplDownEnd(int x, int y, List<GameObject> O)
            : base(x, y)
        {
            GameObjects = O;

            ObjectStates.Add(BombEnum.Inflation, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(12 * 48, 4 * 48, 48, 48),
                new Rectangle(14 * 48, 7 * 48, 48, 48),
                new Rectangle(12 * 48, 10 * 48, 48, 48),
                new Rectangle(14 * 48, 13 * 48, 48, 48),
                new Rectangle(12 * 48, 16 * 48, 48, 48),
                new Rectangle(13 * 48, 19 * 48, 48, 48),
                new Rectangle(12 * 48, 22 * 48, 48, 48),
                new Rectangle(13 * 48, 25 * 48, 48, 48),
                new Rectangle(12 * 48, 28 * 48, 48, 48)
                
            }, 100, true, false, Final)));

            ChangeState(BombEnum.Inflation);
        }

        public void Final()
        {
            isAlive = false;
        }
    }

    /// <summary>Правая часть взрыва</summary>
    public class ExplRight : GameObject
    {

        public ExplRight(int x, int y, List<GameObject> O)
            : base(x, y)
        {
            GameObjects = O;

            ObjectStates.Add(BombEnum.Inflation, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(13 * 48, 2 * 48, 48, 48),
                new Rectangle(15 * 48, 5 * 48, 48, 48),
                new Rectangle(13 * 48, 8 * 48, 48, 48),
                new Rectangle(15 * 48, 11 * 48, 48, 48),
                new Rectangle(13 * 48, 14 * 48, 48, 48),
                new Rectangle(14 * 48, 17 * 48, 48, 48),
                new Rectangle(13 * 48, 20 * 48, 48, 48),
                new Rectangle(14 * 48, 23 * 48, 48, 48),
                new Rectangle(13 * 48, 26 * 48, 48, 48)
                
            }, 100, true, false, Final)));

            ChangeState(BombEnum.Inflation);
        }

        public void Final()
        {
            isAlive = false;
        }
    }

    /// <summary>Правая часть взрыва концовка</summary>
    public class ExplRightEnd : GameObject
    {

        public ExplRightEnd(int x, int y, List<GameObject> O)
            : base(x, y)
        {
            GameObjects = O;

            ObjectStates.Add(BombEnum.Inflation, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(14 * 48, 2 * 48, 48, 48),
                new Rectangle(16 * 48, 5 * 48, 48, 48),
                new Rectangle(14 * 48, 8 * 48, 48, 48),
                new Rectangle(16 * 48, 11 * 48, 48, 48),
                new Rectangle(14 * 48, 14 * 48, 48, 48),
                new Rectangle(15 * 48, 17 * 48, 48, 48),
                new Rectangle(14 * 48, 20 * 48, 48, 48),
                new Rectangle(15 * 48, 23 * 48, 48, 48),
                new Rectangle(14 * 48, 26 * 48, 48, 48)
                
            }, 100, true, false, Final)));

            ChangeState(BombEnum.Inflation);
        }

        public void Final()
        {
            isAlive = false;
        }
    }

    /// <summary>Левая часть взрыва</summary>
    public class ExplLeft : GameObject
    {

        public ExplLeft(int x, int y, List<GameObject> O)
            : base(x, y)
        {
            GameObjects = O;

            ObjectStates.Add(BombEnum.Inflation, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(11 * 48, 2 * 48, 48, 48),
                new Rectangle(13 * 48, 5 * 48, 48, 48),
                new Rectangle(11 * 48, 8 * 48, 48, 48),
                new Rectangle(13 * 48, 11 * 48, 48, 48),
                new Rectangle(11 * 48, 14 * 48, 48, 48),
                new Rectangle(12 * 48, 17 * 48, 48, 48),
                new Rectangle(11 * 48, 20 * 48, 48, 48),
                new Rectangle(12 * 48, 23 * 48, 48, 48),
                new Rectangle(11 * 48, 26 * 48, 48, 48)
                
            }, 100, true, false, Final)));

            ChangeState(BombEnum.Inflation);
        }

        public void Final()
        {
            isAlive = false;
        }
    }

    /// <summary>Левая часть концовка </summary>
    public class ExplLeftEnd : GameObject
    {

        public ExplLeftEnd(int x, int y, List<GameObject> O)
            : base(x, y)
        {
            GameObjects = O;

            ObjectStates.Add(BombEnum.Inflation, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(10 * 48, 2 * 48, 48, 48),
                new Rectangle(12 * 48, 5 * 48, 48, 48),
                new Rectangle(10 * 48, 8 * 48, 48, 48),
                new Rectangle(12 * 48, 11 * 48, 48, 48),
                new Rectangle(10 * 48, 14 * 48, 48, 48),
                new Rectangle(11 * 48, 17 * 48, 48, 48),
                new Rectangle(10 * 48, 20 * 48, 48, 48),
                new Rectangle(11 * 48, 23 * 48, 48, 48),
                new Rectangle(10 * 48, 26 * 48, 48, 48)
                
            }, 100, true, false, Final)));

            ChangeState(BombEnum.Inflation);
        }

        public void Final()
        {
            isAlive = false;
        }
    }
}
