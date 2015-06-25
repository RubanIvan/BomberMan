using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BomberMan.GameObj
{
    public enum PlayerEnum
    {
        /// <summary>Стоит в ожидании </summary>
        Idle,
        /// <summary>Идет в лево </summary>
        WalkLeft,
        /// <summary>Идет в право</summary>
        WalkRight,
        /// <summary>Идет вверх </summary>
        WalkUp,
        /// <summary>Идет вниз</summary>
        WalkDown,
        /// <summary>Стоит развернут в лево </summary>
        IdleLeft,
        /// <summary>Стоит развернут в право</summary>
        IdleRight,
        /// <summary>Стоит развернут в вверх </summary>
        IdleUp,
        /// <summary>Горит, подорвался на бомбе</summary>
        Fire,
    }

    class Player : GameObject, Iexterminable
    {

        public int BombPower = 3;

        //Количество однавремен
        //private int MaxBombCount = 1;

        public IBombarda BombGun;

        //конструктор
        public Player(int x, int y, List<GameObject> O)
            : base(x, y)
        {
            GameObjects = O;

            BombGun = new SampleBombGun(GameObjects);

            //Задание состояний
            ObjectStates.Add(PlayerEnum.Idle, new PlayerIdle(this));
            ObjectStates.Add(PlayerEnum.WalkRight, new PlayerWalkRight(this));
            ObjectStates.Add(PlayerEnum.WalkLeft, new PlayerWalkLeft(this));
            ObjectStates.Add(PlayerEnum.IdleRight, new PlayerIdleRight(this));
            ObjectStates.Add(PlayerEnum.IdleLeft, new PlayerIdleLeft(this));
            ObjectStates.Add(PlayerEnum.WalkUp, new PlayerWalkUp(this));
            ObjectStates.Add(PlayerEnum.IdleUp, new PlayerIdleUp(this));
            ObjectStates.Add(PlayerEnum.WalkDown, new PlayerWalkDown(this));
            ObjectStates.Add(PlayerEnum.Fire, new PlayerFire(this));

            ChangeState(PlayerEnum.Idle);

            SMtransition.Add(PlayerEnum.Idle, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.Idle].Add(PlayerEnum.Idle, PlayerEnum.Idle);
            SMtransition[PlayerEnum.Idle].Add(PlayerEnum.WalkRight, PlayerEnum.WalkRight);
            SMtransition[PlayerEnum.Idle].Add(PlayerEnum.WalkLeft, PlayerEnum.WalkLeft);
            SMtransition[PlayerEnum.Idle].Add(PlayerEnum.WalkUp, PlayerEnum.WalkUp);
            SMtransition[PlayerEnum.Idle].Add(PlayerEnum.WalkDown, PlayerEnum.WalkDown);
            SMtransition[PlayerEnum.Idle].Add(PlayerEnum.Fire, PlayerEnum.Fire);


            SMtransition.Add(PlayerEnum.WalkRight, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.WalkRight].Add(PlayerEnum.Idle, PlayerEnum.IdleRight);
            SMtransition[PlayerEnum.WalkRight].Add(PlayerEnum.WalkRight, PlayerEnum.WalkRight);
            SMtransition[PlayerEnum.WalkRight].Add(PlayerEnum.WalkLeft, PlayerEnum.WalkRight);
            SMtransition[PlayerEnum.WalkRight].Add(PlayerEnum.WalkUp, PlayerEnum.WalkRight);
            SMtransition[PlayerEnum.WalkRight].Add(PlayerEnum.WalkDown, PlayerEnum.WalkRight);
            SMtransition[PlayerEnum.WalkRight].Add(PlayerEnum.Fire, PlayerEnum.Fire);

            SMtransition.Add(PlayerEnum.IdleRight, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.IdleRight].Add(PlayerEnum.Idle, PlayerEnum.IdleRight);
            SMtransition[PlayerEnum.IdleRight].Add(PlayerEnum.WalkRight, PlayerEnum.WalkRight);
            SMtransition[PlayerEnum.IdleRight].Add(PlayerEnum.WalkLeft, PlayerEnum.Idle);
            SMtransition[PlayerEnum.IdleRight].Add(PlayerEnum.WalkUp, PlayerEnum.WalkUp);
            SMtransition[PlayerEnum.IdleRight].Add(PlayerEnum.WalkDown, PlayerEnum.WalkDown);
            SMtransition[PlayerEnum.IdleRight].Add(PlayerEnum.Fire, PlayerEnum.Fire);

            SMtransition.Add(PlayerEnum.WalkLeft, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.WalkLeft].Add(PlayerEnum.Idle, PlayerEnum.IdleLeft);
            SMtransition[PlayerEnum.WalkLeft].Add(PlayerEnum.WalkRight, PlayerEnum.WalkLeft);
            SMtransition[PlayerEnum.WalkLeft].Add(PlayerEnum.WalkLeft, PlayerEnum.WalkLeft);
            SMtransition[PlayerEnum.WalkLeft].Add(PlayerEnum.WalkUp, PlayerEnum.WalkLeft);
            SMtransition[PlayerEnum.WalkLeft].Add(PlayerEnum.WalkDown, PlayerEnum.WalkLeft);
            SMtransition[PlayerEnum.WalkLeft].Add(PlayerEnum.Fire, PlayerEnum.Fire);

            SMtransition.Add(PlayerEnum.IdleLeft, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.IdleLeft].Add(PlayerEnum.Idle, PlayerEnum.IdleLeft);
            SMtransition[PlayerEnum.IdleLeft].Add(PlayerEnum.WalkRight, PlayerEnum.Idle);
            SMtransition[PlayerEnum.IdleLeft].Add(PlayerEnum.WalkLeft, PlayerEnum.WalkLeft);
            SMtransition[PlayerEnum.IdleLeft].Add(PlayerEnum.WalkUp, PlayerEnum.WalkUp);
            SMtransition[PlayerEnum.IdleLeft].Add(PlayerEnum.WalkDown, PlayerEnum.WalkDown);
            SMtransition[PlayerEnum.IdleLeft].Add(PlayerEnum.Fire, PlayerEnum.Fire);

            SMtransition.Add(PlayerEnum.WalkUp, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.WalkUp].Add(PlayerEnum.Idle, PlayerEnum.IdleUp);
            SMtransition[PlayerEnum.WalkUp].Add(PlayerEnum.WalkRight, PlayerEnum.WalkUp);
            SMtransition[PlayerEnum.WalkUp].Add(PlayerEnum.WalkLeft, PlayerEnum.WalkUp);
            SMtransition[PlayerEnum.WalkUp].Add(PlayerEnum.WalkUp, PlayerEnum.WalkUp);
            SMtransition[PlayerEnum.WalkUp].Add(PlayerEnum.WalkDown, PlayerEnum.WalkUp);
            SMtransition[PlayerEnum.WalkUp].Add(PlayerEnum.Fire, PlayerEnum.Fire);

            SMtransition.Add(PlayerEnum.IdleUp, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.IdleUp].Add(PlayerEnum.Idle, PlayerEnum.IdleUp);
            SMtransition[PlayerEnum.IdleUp].Add(PlayerEnum.WalkRight, PlayerEnum.WalkRight);
            SMtransition[PlayerEnum.IdleUp].Add(PlayerEnum.WalkLeft, PlayerEnum.WalkLeft);
            SMtransition[PlayerEnum.IdleUp].Add(PlayerEnum.WalkUp, PlayerEnum.WalkUp);
            SMtransition[PlayerEnum.IdleUp].Add(PlayerEnum.WalkDown, PlayerEnum.Idle);
            SMtransition[PlayerEnum.IdleUp].Add(PlayerEnum.Fire, PlayerEnum.Fire);

            SMtransition.Add(PlayerEnum.WalkDown, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.WalkDown].Add(PlayerEnum.Idle, PlayerEnum.Idle);
            SMtransition[PlayerEnum.WalkDown].Add(PlayerEnum.WalkRight, PlayerEnum.WalkDown);
            SMtransition[PlayerEnum.WalkDown].Add(PlayerEnum.WalkLeft, PlayerEnum.WalkDown);
            SMtransition[PlayerEnum.WalkDown].Add(PlayerEnum.WalkUp, PlayerEnum.WalkDown);
            SMtransition[PlayerEnum.WalkDown].Add(PlayerEnum.WalkDown, PlayerEnum.WalkDown);
            SMtransition[PlayerEnum.WalkDown].Add(PlayerEnum.Fire, PlayerEnum.Fire);

            SMtransition.Add(PlayerEnum.Fire, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.Fire].Add(PlayerEnum.Idle, PlayerEnum.Fire);
            SMtransition[PlayerEnum.Fire].Add(PlayerEnum.WalkRight, PlayerEnum.Fire);
            SMtransition[PlayerEnum.Fire].Add(PlayerEnum.WalkLeft, PlayerEnum.Fire);
            SMtransition[PlayerEnum.Fire].Add(PlayerEnum.WalkUp, PlayerEnum.Fire);
            SMtransition[PlayerEnum.Fire].Add(PlayerEnum.WalkDown, PlayerEnum.Fire);
            SMtransition[PlayerEnum.Fire].Add(PlayerEnum.Fire, PlayerEnum.Fire);

        }


        public override void Update(GameTime gameTime)
        {

            State.Update(gameTime);

            //Установка бомбы
            if (InputHelper.KeyPressed(Keys.Space))
            {
                DropBomb();

            }

            if ((PlayerEnum)SMstate == PlayerEnum.WalkRight || (PlayerEnum)SMstate == PlayerEnum.WalkLeft)
            {
                if (PosWorldX % 48 == 0) SMrequest(PlayerEnum.Idle);
            }

            if ((PlayerEnum)SMstate == PlayerEnum.WalkUp || (PlayerEnum)SMstate == PlayerEnum.WalkDown)
            {
                if (PosWorldY % 48 == 0) SMrequest(PlayerEnum.Idle);
            }

            
            for (int i = InputHelper.KeyStatePrioryti.Count-1; i >= 0; i--)
            {
                switch (InputHelper.KeyStatePrioryti[i])
                {
                        case Keys.Left:
                            TryGoLeft(); break;
                        case Keys.Right:
                            TryGoRight(); break;
                        case Keys.Up:
                            TryGoUp(); break;
                        case Keys.Down:
                            TryGoDown(); break;
                }
            }

           
            //Востание из мертвых
            if (InputHelper.IsKeyDown(Keys.R))
            {
                //ObjectStates.Add(PlayerEnum.Fire, new PlayerFire(this));
                ObjectStates[PlayerEnum.Fire].Animation.SpriteCurentFrameNum = 0;
                ObjectStates[PlayerEnum.Fire].Animation.isAnimated = true;
                PosWorldX = 48;
                PosWorldY = 48;

                SMstate = PlayerEnum.Idle;
                ChangeState(PlayerEnum.Idle);
                return;
            }

            //Для тестовых целей 
            if (InputHelper.IsKeyDown(Keys.T))
            {
                //ObjectStates.Add(PlayerEnum.Fire, new PlayerFire(this));
                ObjectStates[PlayerEnum.Fire].Animation.SpriteCurentFrameNum = 0;
                ObjectStates[PlayerEnum.Fire].Animation.isAnimated = true;
                PosWorldX = 48 * 5+30;
                PosWorldY = 48;

                SMstate = PlayerEnum.Idle;
                ChangeState(PlayerEnum.Idle);
                return;
            }

        }

        /// <summary>Хождение по сторонам</summary>
        private void TryGoDown()
        {
            if (PosWorldY % 48 == 0 && PassabilityCheck(PosWorldX, PosWorldY + 48)) SMrequest(PlayerEnum.WalkDown);
        }

        private void TryGoUp()
        {
            if (PosWorldY % 48 == 0 && PassabilityCheck(PosWorldX, PosWorldY - 48)) SMrequest(PlayerEnum.WalkUp);
        }

        private void TryGoLeft()
        {
            if (PosWorldX%48 == 0 && PassabilityCheck(PosWorldX - 48, PosWorldY)) SMrequest(PlayerEnum.WalkLeft);
        }

        private void TryGoRight()
        {
            if (PosWorldX%48 == 0 && PassabilityCheck(PosWorldX + 48, PosWorldY)) SMrequest(PlayerEnum.WalkRight);
        }

        /// <summary>Проверка проходим ли данный квадрат</summary>
        public bool PassabilityCheck(int x, int y)
        {
            //если мы находим в звданном квадрате непроходимый объект то выходим
            foreach (GameObject O in GameObjects)
                if (O.PosWorldX == x && O.PosWorldY == y && O.isPassability == false) return false;
            //иначе квадрат проходим
            return true;
        }

        /// <summary>Игрок пытается установить бомбу</summary>
        public void DropBomb()
        {
            int x, y;

            //Debug.WriteLine("Pos: " + PosWorldX + "  " + PosWorldY);

            //если мы в квадрате 
            if (PosWorldY % 48 == 0 && PosWorldX % 48 == 0)
            {
                //В клетке с бомбой может быть только 2 элемента игрок и пустое поле тогда только ставим бомбу
                if (GameObjects.FindAll(o => o.PosWorldX == PosWorldX && o.PosWorldY == PosWorldY).Count == 2)
                     BombGun.DropBomb(PosWorldX, PosWorldY);
            }
            else //мы не в квадрате
            {
                //если идем в право
                if ((PlayerEnum)SMstate == PlayerEnum.WalkRight)
                {
                    y = PosWorldY;
                    x = PosWorldX - PosWorldX % 48;
                    if (PosWorldX % 48 > 38)
                    {
                        if (isEmptyCell(x + 48, y)) BombGun.DropBomb(x + 48, y);
                    }
                    else
                    {
                        if (isEmptyCell(x, y)) BombGun.DropBomb(x, y);
                    }
                }

                //если идем в низ
                if ((PlayerEnum)SMstate == PlayerEnum.WalkDown)
                {
                    x = PosWorldX;
                    y = PosWorldY - PosWorldY % 48;
                    if (PosWorldY % 48 > 38)
                    {
                        if (isEmptyCell(x, y + 48)) BombGun.DropBomb(x, y + 48);
                    }
                    else
                    {
                        if (isEmptyCell(x, y)) BombGun.DropBomb(x, y);
                    }
                }

                //если идем в лево
                if ((PlayerEnum)SMstate == PlayerEnum.WalkLeft)
                {
                    y = PosWorldY;
                    x = PosWorldX - PosWorldX % 48 + 48;
                    if (PosWorldX % 48 > 10)
                    {
                        if (isEmptyCell(x, y)) BombGun.DropBomb(x, y);
                    }
                    else
                    {
                        if (isEmptyCell(x - 48, y)) BombGun.DropBomb(x - 48, y);
                    }
                }

                //если идем в верх
                if ((PlayerEnum)SMstate == PlayerEnum.WalkUp)
                {
                    x = PosWorldX;
                    y = PosWorldY - PosWorldY % 48 + 48;
                    if (PosWorldY % 48 > 10)
                    {
                        if (isEmptyCell(x, y)) BombGun.DropBomb(x, y);
                    }
                    else
                    {
                        if (isEmptyCell(x, y - 48)) BombGun.DropBomb(x, y - 48);
                    }
                }

            }
        }

        /// <summary>пуста ли клетка ?? проверяется при установки бомбы</summary>
        public bool isEmptyCell(int x, int y)
        {
            return GameObjects.FindAll(o => o.PosWorldX == x && o.PosWorldY == y).Count == 1;
        }

        /// <summary>Игрок подорвался на бомбе</summary>
        public void Blow(BlowSide side)
        {
            ChangeState(PlayerEnum.Fire);
        }
    }


    /// <summary>Игрок стоит на месте</summary>
    public class PlayerIdle : State
    {
        public PlayerIdle(GameObject player)
            : base(player)
        {
            Animation = new Animation(new List<Rectangle>() { new Rectangle(0 * 48, 11 * 48, 48, 48) });
        }
    }

    /// <summary>Игрок стоит повернут в право</summary>
    public class PlayerIdleRight : State
    {
        public PlayerIdleRight(GameObject player)
            : base(player)
        {
            Animation = new Animation(new List<Rectangle>() { new Rectangle(0 * 48, 12 * 48, 48, 48) });
        }
    }

    /// <summary>Игрок стоит повернут в лево</summary>
    public class PlayerIdleLeft : State
    {
        public PlayerIdleLeft(GameObject player)
            : base(player)
        {
            Animation = new Animation(new List<Rectangle>() { new Rectangle(0 * 48, 10 * 48, 48, 48) });
        }
    }

    /// <summary>Игрок стоит повернут в верх</summary>
    public class PlayerIdleUp : State
    {
        public PlayerIdleUp(GameObject player)
            : base(player)
        {
            Animation = new Animation(new List<Rectangle>() { new Rectangle(0 * 48, 9 * 48, 48, 48) });
        }
    }

    /// <summary>Игрок идет в право</summary>
    public class PlayerWalkRight : State
    {
        public PlayerWalkRight(GameObject player)
            : base(player)
        {
            Animation = new Animation(new List<Rectangle>()
            {
                new Rectangle(1 * 48, 12 * 48, 48, 48),new Rectangle(2 * 48, 12 * 48, 48, 48),
                new Rectangle(3 * 48, 12 * 48, 48, 48),new Rectangle(4 * 48, 12 * 48, 48, 48),new Rectangle(5 * 48, 12 * 48, 48, 48),
                new Rectangle(6 * 48, 12 * 48, 48, 48),new Rectangle(7 * 48, 12 * 48, 48, 48),new Rectangle(8 * 48, 12 * 48, 48, 48)
            }, 80, true, true);
        }


        public override void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);

            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (ElapsedTime > 10)
            {
                ElapsedTime = 0;
                GameObject.PosWorldX += 2;
            }
        }

    }

    /// <summary>Игрок идет в лево</summary>
    public class PlayerWalkLeft : State
    {
        public PlayerWalkLeft(GameObject player)
            : base(player)
        {
            Animation = new Animation(new List<Rectangle>()
            {
                new Rectangle(1 * 48, 10 * 48, 48, 48),new Rectangle(2 * 48, 10 * 48, 48, 48),
                new Rectangle(3 * 48, 10 * 48, 48, 48),new Rectangle(4 * 48, 10 * 48, 48, 48),new Rectangle(5 * 48, 10 * 48, 48, 48),
                new Rectangle(6 * 48, 10 * 48, 48, 48),new Rectangle(7 * 48, 10 * 48, 48, 48),new Rectangle(8 * 48, 10 * 48, 48, 48)
            }, 80, true, true);

        }

        public override void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);

            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (ElapsedTime > 10)
            {
                ElapsedTime = 0;
                GameObject.PosWorldX -= 2;
            }
        }


    }

    /// <summary>Игрок идет в вниз</summary>
    public class PlayerWalkDown : State
    {
        public PlayerWalkDown(GameObject player)
            : base(player)
        {
            Animation = new Animation(new List<Rectangle>()
            {
                new Rectangle(1 * 48, 11 * 48, 48, 48),new Rectangle(2 * 48, 11 * 48, 48, 48),
                new Rectangle(3 * 48, 11 * 48, 48, 48),new Rectangle(4 * 48, 11 * 48, 48, 48),new Rectangle(5 * 48, 11 * 48, 48, 48),
                new Rectangle(6 * 48, 11 * 48, 48, 48),new Rectangle(7 * 48, 11 * 48, 48, 48),new Rectangle(8 * 48, 11 * 48, 48, 48)
            }, 80, true, true);

        }

        public override void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);

            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (ElapsedTime > 10)
            {
                ElapsedTime = 0;
                GameObject.PosWorldY += 2;
            }
        }


    }

    /// <summary>Игрок идет в верх</summary>
    public class PlayerWalkUp : State
    {
        public PlayerWalkUp(GameObject player)
            : base(player)
        {
            Animation = new Animation(new List<Rectangle>()
            {
                new Rectangle(1 * 48, 9 * 48, 48, 48),new Rectangle(2 * 48, 9 * 48, 48, 48),
                new Rectangle(3 * 48, 9 * 48, 48, 48),new Rectangle(4 * 48, 9 * 48, 48, 48),new Rectangle(5 * 48, 9 * 48, 48, 48),
                new Rectangle(6 * 48, 9 * 48, 48, 48),new Rectangle(7 * 48, 9 * 48, 48, 48),new Rectangle(8 * 48, 9 * 48, 48, 48)

            }, 80, true, true);

        }

        public override void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);

            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (ElapsedTime > 10)
            {
                ElapsedTime = 0;
                GameObject.PosWorldY -= 2;
            }
        }


    }

    /// <summary>Игрок горит (подорвался на бомбе)</summary>
    public class PlayerFire : State
    {
        public PlayerFire(GameObject player)
            : base(player)
        {
            Animation = new Animation(new List<Rectangle>()
            {
                new Rectangle(0*48, 30*48, 48, 48),new Rectangle(1*48, 30*48, 48, 48),new Rectangle(2*48, 30*48, 48, 48),
                new Rectangle(3*48, 30*48, 48, 48),new Rectangle(4*48, 30*48, 48, 48),new Rectangle(5*48, 30*48, 48, 48),
                new Rectangle(6*48, 30*48, 48, 48),new Rectangle(7*48, 30*48, 48, 48),new Rectangle(8*48, 30*48, 48, 48),
                new Rectangle(9*48, 30*48, 48, 48),new Rectangle(10*48, 30*48, 48, 48),new Rectangle(11*48, 30*48, 48, 48),
                new Rectangle(12*48, 30*48, 48, 48),new Rectangle(13*48, 30*48, 48, 48),new Rectangle(14*48, 30*48, 48, 48),
                new Rectangle(15*48, 30*48, 48, 48),new Rectangle(16*48, 30*48, 48, 48),new Rectangle(17*48, 30*48, 48, 48),
                new Rectangle(18*48, 30*48, 48, 48),new Rectangle(19*48, 30*48, 48, 48),new Rectangle(0*48, 31*48, 48, 48),
                new Rectangle(1*48, 31*48, 48, 48),
            }, 80, true, false);
        }
    }


}

