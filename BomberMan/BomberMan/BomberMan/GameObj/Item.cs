using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberMan.GameObj
{

    class Item : GameObject, Iexterminable
    {
        //чтобы пережить взрыв стены которая скрывает объект. считаем взрывы
        protected int BlowCount = 1;

        protected Player Player;

        public Item(int x, int y, List<GameObject> O,Player p)
            : base(x, y,p)
        {
            GameObjects = O;

            Zorder = Zorders.Item;

            isPassability = true;

        }

        public void Blow(BlowSide side)
        {
            BlowCount--;
            if (BlowCount < 0) isAlive = false;
        }
    }


    /// <summary>Увеличение мощности взрыва</summary>
    class ItemBombPower : Item
    {

        public ItemBombPower(int x, int y, List<GameObject> O,Player p)
            : base(x, y, O,p)
        {
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


    }


    /// <summary>Уменьшение времени перезарядки</summary>
    class ItemBombReloadTime : Item
    {
        public ItemBombReloadTime(int x, int y, List<GameObject> O,Player p)
            : base(x, y, O,p)
        {
            ObjectStates.Add(WallEnum.Idle, new State(new Animation(new List<Rectangle>() { new Rectangle(4 * 48, 24 * 48, 48, 48) })));
            ChangeState(WallEnum.Idle);
        }

        public override void Update(GameTime gameTime)
        {
            State.Update(gameTime);
            if (Player.PosWorldX == PosWorldX && Player.PosWorldY == PosWorldY)
            {
                isAlive = false;
                Player.BombTimeReload -= 500;
                if (Player.BombTimeReload < 0) Player.BombTimeReload = 0;
            }

        }

    }


    /// <summary>Дополнительная бомба</summary>
    class ItemBombQuantity : Item
    {
        public ItemBombQuantity(int x, int y, List<GameObject> O,Player p)
            : base(x, y, O,p)
        {
            ObjectStates.Add(WallEnum.Idle, new State(new Animation(new List<Rectangle>() { new Rectangle(0 * 48, 24 * 48, 48, 48) })));
            ChangeState(WallEnum.Idle);
        }

        public override void Update(GameTime gameTime)
        {
            State.Update(gameTime);
            if (Player.PosWorldX == PosWorldX && Player.PosWorldY == PosWorldY)
            {
                isAlive = false;
                Player.MaxBombCount++;
            }

        }

    }


    /// <summary>Дополнительная жизнь</summary>
    class ItemLife : Item
    {
        public ItemLife(int x, int y, List<GameObject> O,Player p)
            : base(x, y, O,p)
        {
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

    }


    
    
}
