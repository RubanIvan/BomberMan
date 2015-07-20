using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BomberMan.GameObj;
using Microsoft.Xna.Framework;

namespace BomberMan
{
    public static class ZombyConst
    {
        /// <summary>Скорость передвижения зомби</summary>
        public static int Speed = 30;
    }

    class Zomby : GameObject, Iexterminable
    {
        private MoveStrategy Move;

        public Zomby(int x, int y, List<GameObject> O,CPlayer p)
            : base(x, y,p)
        {
            GameObjects = O;

            Zorder = Zorders.Enemy;

            isPassability = true;

            ObjectStates.Add(PlayersEnum.WalkRight, new ZombyWalkRight(this));
            ObjectStates.Add(PlayersEnum.WalkLeft, new ZombyWalkLeft(this));
            ObjectStates.Add(PlayersEnum.WalkUp, new ZombyWalkUp(this));
            ObjectStates.Add(PlayersEnum.WalkDown, new ZombyWalkDown(this));
            ObjectStates.Add(PlayersEnum.Idle, new ZombyIdle(this));
            ObjectStates.Add(PlayersEnum.Fire, new ZombyFire(this));

            ChangeState(PlayersEnum.WalkRight);

            //Move=new RandomMove(this,GameObjects);
            Move=new AntMove(this,GameObjects);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //если мы в клетке то выбираем куда идти дальше
            if(PosWorldX%48==0 && PosWorldY%48==0) ChangeState(Move.GetMove());
        }

        public void Blow(BlowSide side)
        {
            ChangeState(PlayersEnum.Fire);
        }
    }

    /// <summary> идет в право</summary>
    public class ZombyWalkRight : State
    {
        public ZombyWalkRight(GameObject zomby)
            : base(zomby)
        {
            Animation = new Animation(new List<Rectangle>()
            {
                new Rectangle(1 * 48, 18 * 48, 48, 48),new Rectangle(2 * 48, 18 * 48, 48, 48),
                new Rectangle(3 * 48, 18 * 48, 48, 48),new Rectangle(4 * 48, 18 * 48, 48, 48),
                new Rectangle(5 * 48, 18 * 48, 48, 48),new Rectangle(6 * 48, 18 * 48, 48, 48)
            }, 80, true, true);
        }


        public override void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);

            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (ElapsedTime > ZombyConst.Speed)
            {
                ElapsedTime = 0;
                GameObject.PosWorldX += 2;
            }
        }
    }

    /// <summary> идет в лево</summary>
    public class ZombyWalkLeft : State
    {
        public ZombyWalkLeft(GameObject zomby)
            : base(zomby)
        {
            Animation = new Animation(new List<Rectangle>()
            {
                new Rectangle(1 * 48, 19 * 48, 48, 48),new Rectangle(2 * 48, 19 * 48, 48, 48),
                new Rectangle(3 * 48, 19 * 48, 48, 48),new Rectangle(4 * 48, 19 * 48, 48, 48),
                new Rectangle(5 * 48, 19 * 48, 48, 48),new Rectangle(6 * 48, 19 * 48, 48, 48)
            }, 80, true, true);
        }


        public override void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);

            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (ElapsedTime > ZombyConst.Speed)
            {
                ElapsedTime = 0;
                GameObject.PosWorldX -= 2;
            }
        }

    }

    /// <summary> идет в верх</summary>
    public class ZombyWalkUp : State
    {
        public ZombyWalkUp(GameObject zomby)
            : base(zomby)
        {
            Animation = new Animation(new List<Rectangle>()
            {
                new Rectangle(1 * 48, 20 * 48, 48, 48),new Rectangle(2 * 48, 20 * 48, 48, 48),
                new Rectangle(3 * 48, 20 * 48, 48, 48),new Rectangle(4 * 48, 20 * 48, 48, 48),
                new Rectangle(5 * 48, 20 * 48, 48, 48),new Rectangle(6 * 48, 20 * 48, 48, 48)
            }, 80, true, true);
        }


        public override void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);

            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (ElapsedTime > ZombyConst.Speed)
            {
                ElapsedTime = 0;
                GameObject.PosWorldY -= 2;
            }
        }

    }
    
    /// <summary> идет в вниз</summary>
    public class ZombyWalkDown : State
    {
        public ZombyWalkDown(GameObject zomby)
            : base(zomby)
        {
            Animation = new Animation(new List<Rectangle>()
            {
                new Rectangle(1 * 48, 17 * 48, 48, 48),new Rectangle(2 * 48, 17 * 48, 48, 48),
                new Rectangle(3 * 48, 17 * 48, 48, 48),new Rectangle(4 * 48, 17 * 48, 48, 48),
                new Rectangle(5 * 48, 17 * 48, 48, 48),new Rectangle(6 * 48, 17 * 48, 48, 48)
            }, 80, true, true);
        }


        public override void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);

            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (ElapsedTime > ZombyConst.Speed)
            {
                ElapsedTime = 0;
                GameObject.PosWorldY += 2;
            }
        }

    }

    /// <summary>Игрок стоит на месте</summary>
    public class ZombyIdle : State
    {
        public ZombyIdle(GameObject player)
            : base(player)
        {
            Animation = new Animation(new List<Rectangle>() { new Rectangle(0 * 48, 17 * 48, 48, 48) });
        }
    }

    public class ZombyFire : State
    {
        
        public ZombyFire(GameObject player)
            : base(player)
        {
            Animation = new Animation(new List<Rectangle>()
            {
                new Rectangle(0 * 48, 21 * 48, 48, 48),new Rectangle(1 * 48, 21 * 48, 48, 48),new Rectangle(2 * 48, 21 * 48, 48, 48),
                new Rectangle(3 * 48, 21 * 48, 48, 48),new Rectangle(4 * 48, 21 * 48, 48, 48),
                new Rectangle(0 * 48, 22 * 48, 48, 48),new Rectangle(1 * 48, 22 * 48, 48, 48),
                new Rectangle(2 * 48, 22 * 48, 48, 48),new Rectangle(3 * 48, 22 * 48, 48, 48),
                new Rectangle(4 * 48, 22 * 48, 48, 48),new Rectangle(5 * 48, 22 * 48, 48, 48),
                new Rectangle(6 * 48, 22 * 48, 48, 48),new Rectangle(0 * 48, 0 * 48, 48, 48)


            },80,true,false,ZombyDead);

            
        }

        public void ZombyDead()
        {
            ((Zomby)GameObject).Player.Score += 100;
        }
    }
}
