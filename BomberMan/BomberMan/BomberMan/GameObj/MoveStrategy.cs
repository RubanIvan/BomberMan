using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberMan.GameObj
{
    abstract class MoveStrategy
    {
        /// <summary>Ссылка на игровой мир</summary>
        private List<GameObject> GameObjects;

        /// <summary>Ссылка кто будет пользоватся стратегией</summary>
        protected GameObject Enamy;

        /// <summary>Ссылка на игрока</summary>
        private GameObject Player;

        public MoveStrategy(GameObject enamy, List<GameObject> O)
        {
            Enamy = enamy;
            GameObjects = O;
        }

        /// <summary>Вызывается каждую полную клетку для опредиления следующего хода</summary>
        public abstract PlayersEnum GetMove();

        /// <summary>Проверка проходим ли данный квадрат</summary>
        public bool PassabilityCheck(Point p)
        {
            //если мы находим в звданном квадрате непроходимый объект то выходим
            foreach (GameObject O in GameObjects)
                if (O.PosWorldX == p.X && O.PosWorldY == p.Y && O.isPassability == false) return false;
            //иначе квадрат проходим
            return true;
        }


    }


    class RandomMove : MoveStrategy
    {
        public RandomMove(GameObject enamy, List<GameObject> O)
            : base(enamy, O)
        {
        }

        public override PlayersEnum GetMove()
        {

            if ((PlayersEnum)Enamy.SMstate == PlayersEnum.Fire) return PlayersEnum.Fire;

            //if (Rnd.Next(100) > 90) return PlayersEnum.WalkLeft;

            //шли в сторону и там свободно то идем в туже сторону
            //if ((PlayersEnum)Enamy.SMstate == PlayersEnum.WalkRight && PassabilityCheck(Enamy.PosWorldX + 48, Enamy.PosWorldY)) return PlayersEnum.WalkRight;
            //if ((PlayersEnum)Enamy.SMstate == PlayersEnum.WalkLeft && PassabilityCheck(Enamy.PosWorldX - 48, Enamy.PosWorldY)) return PlayersEnum.WalkLeft;
            //if ((PlayersEnum)Enamy.SMstate == PlayersEnum.WalkUp && PassabilityCheck(Enamy.PosWorldX, Enamy.PosWorldY - 48)) return PlayersEnum.WalkUp;
            //if ((PlayersEnum)Enamy.SMstate == PlayersEnum.WalkDown && PassabilityCheck(Enamy.PosWorldX + 48, Enamy.PosWorldY + 48)) return PlayersEnum.WalkDown;


            return (PlayersEnum)PlayersEnum.Idle;
        }
    }


    class AntMove : MoveStrategy
    {
        struct CellFlavor
        {
            public int Flavor;
            public PlayersEnum MoveEnum;
        }

        /// <summary>Запах клетки</summary>
        private Dictionary<Point, int> flavour = new Dictionary<Point, int>();

        private CellFlavor[] MoveVariant = new CellFlavor[4];
        
        private Point p;

        public AntMove(GameObject enamy, List<GameObject> O)
            : base(enamy, O)
        {

        }

        public override PlayersEnum GetMove()
        {
            //Если загорелись то горим дальще
            if ((PlayersEnum)Enamy.SMstate == PlayersEnum.Fire) return PlayersEnum.Fire;

            p = new Point(Enamy.PosWorldX , Enamy.PosWorldY);
            

            //Move Right
            p.X += 48;
            MoveVariant[0] = new CellFlavor() { Flavor = GetCellFlover(p), MoveEnum = PlayersEnum.WalkRight };

            // Move Left
            p.X -= 96;
            MoveVariant[1] = new CellFlavor() { Flavor = GetCellFlover(p), MoveEnum = PlayersEnum.WalkLeft };


            // Move Down
            p.X += 48;
            p.Y += 48;
            MoveVariant[2] = new CellFlavor() { Flavor = GetCellFlover(p), MoveEnum = PlayersEnum.WalkDown };


            // Move Up
            p.Y -= 96;
            MoveVariant[3] = new CellFlavor() { Flavor = GetCellFlover(p), MoveEnum = PlayersEnum.WalkUp };

            p.Y += 48;
            if (flavour.ContainsKey(p))flavour[p]++;
            else flavour.Add(p,1);
            

            int j = 0;
            for (int i = 1; i < 4; i++)
            {
                if (MoveVariant[j].Flavor > MoveVariant[i].Flavor) j = i;
            }

            if (MoveVariant[j].Flavor==Int32.MaxValue)return PlayersEnum.Idle;
            else
            return MoveVariant[j].MoveEnum;

        }


        protected int GetCellFlover(Point p)
        {
            if (PassabilityCheck(p))
            {
                //и ее запах есть в базе то возвращаем запах
                if (flavour.ContainsKey(p)) return flavour[p];
                else
                {
                    flavour.Add(p, 0);
                    return 0;
                }

            }
            return Int32.MaxValue;
        }

        
    }

}
