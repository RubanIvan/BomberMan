using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public bool PassabilityCheck(int x, int y)
        {
            //если мы находим в звданном квадрате непроходимый объект то выходим
            foreach (GameObject O in GameObjects)
                if (O.PosWorldX == x && O.PosWorldY == y && O.isPassability == false) return false;
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

            if ((PlayersEnum)Enamy.SMstate == PlayersEnum.Fire)return PlayersEnum.Fire;

            //if (Rnd.Next(100) > 90) return PlayersEnum.WalkLeft;

            //шли в сторону и там свободно то идем в туже сторону
            if ((PlayersEnum)Enamy.SMstate == PlayersEnum.WalkRight && PassabilityCheck(Enamy.PosWorldX + 48, Enamy.PosWorldY)) return PlayersEnum.WalkRight;
            if ((PlayersEnum)Enamy.SMstate == PlayersEnum.WalkLeft && PassabilityCheck(Enamy.PosWorldX - 48, Enamy.PosWorldY)) return PlayersEnum.WalkLeft;
            if ((PlayersEnum)Enamy.SMstate == PlayersEnum.WalkUp && PassabilityCheck(Enamy.PosWorldX, Enamy.PosWorldY - 48)) return PlayersEnum.WalkUp;
            if ((PlayersEnum)Enamy.SMstate == PlayersEnum.WalkDown && PassabilityCheck(Enamy.PosWorldX + 48, Enamy.PosWorldY + 48)) return PlayersEnum.WalkDown;


            return (PlayersEnum)PlayersEnum.Idle;
        }
    }

}
