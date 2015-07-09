using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberMan.GameObj
{
    /// <summary>Увеличение мощности взрыва</summary>
    class ItemBombPower : GameObject, Iexterminable
    {
        //чтобы пережить взрыв стены которая скрывает объект. считаем взрывы
        private int BlowCount = 1;

        private Player Player;

        public ItemBombPower(int x, int y, List<GameObject> O)
            : base(x, y)
        {
            //найти игрока
            foreach (GameObject G in O) if (G is Player) Player = (Player)G;

            GameObjects = O;

            Zorder = Zorders.Item;

            isPassability = true;

            ObjectStates.Add(WallEnum.Idle, new State(new Animation(new List<Rectangle>() { new Rectangle(1 * 48, 24 * 48, 48, 48) })));
            ChangeState(WallEnum.Idle);
        }

        public override void Update(GameTime gameTime)
        {
            State.Update(gameTime);
            if (Player.PosWorldX == PosWorldX && Player.PosWorldY == PosWorldY)
            {
                isAlive = false;
                if (Player.BombGun is SampleBombGun) Player.BombGun = new MidleBombGun(GameObjects);
                else if (Player.BombGun is MidleBombGun) Player.BombGun = new BigBombGun(GameObjects);
                else Player.Score += 1000;
                
            }

        }

        public void Blow(BlowSide side)
        {
            BlowCount--;
            if (BlowCount < 0) isAlive = false;
        }
    }

    /// <summary>Дополнительная жизнь</summary>
    class ItemLife : GameObject, Iexterminable
    {
        //чтобы пережить взрыв стены которая скрывает объект. считаем взрывы
        private int BlowCount = 1;

        private Player Player;

        public ItemLife(int x, int y, List<GameObject> O)
            : base(x, y)
        {
            //найти игрока
            foreach (GameObject G in O) if (G is Player) Player = (Player)G;

            Zorder = Zorders.Item;

            isPassability = true;

            ObjectStates.Add(WallEnum.Idle, new State(new Animation(new List<Rectangle>() { new Rectangle(2 * 48, 24 * 48, 48, 48) })));
            ChangeState(WallEnum.Idle);
        }

        public override void Update(GameTime gameTime)
        {
            State.Update(gameTime);
            if (Player.PosWorldX == PosWorldX && Player.PosWorldY == PosWorldY)
            {
                isAlive = false;
                Player.Lives++;
            }

        }

        public void Blow(BlowSide side)
        {
            BlowCount--;
            if(BlowCount<0) isAlive = false;
        }
    }
}
