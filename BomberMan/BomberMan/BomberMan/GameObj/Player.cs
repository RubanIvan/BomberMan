using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BomberMan
{
    public enum PlayersEnum
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
        /// <summary>Игока догнал противник</summary>
        Dead
    }

    public class CPlayer : GameObject, Iexterminable
    {
        /// <summary>Текущее количество бомб в обойме</summary>
        public int BombCount = 3;

        /// <summary>Максимапльное количество бомб в обойме</summary>
        public int MaxBombCount = 3;

        /// <summary>Время перезарядки бомбы</summary>
        public int BombTimeReload=3000;

        /// <summary>Cколько прошло времени с последней перезарядки</summary>
        public int BombTime;

        public IBombarda BombGun;

        /// <summary>Количество жизней</summary>
        public int Lives = 3;

        public int Score = 0;

        /// <summary>стартовая позиция игрока </summary>
        public Point StartPos;

        //конструктор
        public CPlayer(int x, int y, List<GameObject> O)
            : base(x, y,null)
        {
            GameObjects = O;

            Zorder = Zorders.Player;

            BombGun = new SampleBombGun(GameObjects);
            //BombGun = new MidleBombGun(GameObjects);
            //BombGun = new BigBombGun(GameObjects);

            isPassability = true;

            StartPos.X = x;
            StartPos.Y = y;

            //Задание состояний
            ObjectStates.Add(PlayersEnum.Idle, new PlayerIdle(this));
            ObjectStates.Add(PlayersEnum.WalkRight, new PlayerWalkRight(this));
            ObjectStates.Add(PlayersEnum.WalkLeft, new PlayerWalkLeft(this));
            ObjectStates.Add(PlayersEnum.IdleRight, new PlayerIdleRight(this));
            ObjectStates.Add(PlayersEnum.IdleLeft, new PlayerIdleLeft(this));
            ObjectStates.Add(PlayersEnum.WalkUp, new PlayerWalkUp(this));
            ObjectStates.Add(PlayersEnum.IdleUp, new PlayerIdleUp(this));
            ObjectStates.Add(PlayersEnum.WalkDown, new PlayerWalkDown(this));
            ObjectStates.Add(PlayersEnum.Fire, new PlayerFire(this));
            ObjectStates.Add(PlayersEnum.Dead, new PlayerDead(this));

            ChangeState(PlayersEnum.Idle);

            SMtransition.Add(PlayersEnum.Idle, new Dictionary<Enum, Enum>());
            SMtransition[PlayersEnum.Idle].Add(PlayersEnum.Idle, PlayersEnum.Idle);
            SMtransition[PlayersEnum.Idle].Add(PlayersEnum.WalkRight, PlayersEnum.WalkRight);
            SMtransition[PlayersEnum.Idle].Add(PlayersEnum.WalkLeft, PlayersEnum.WalkLeft);
            SMtransition[PlayersEnum.Idle].Add(PlayersEnum.WalkUp, PlayersEnum.WalkUp);
            SMtransition[PlayersEnum.Idle].Add(PlayersEnum.WalkDown, PlayersEnum.WalkDown);
            SMtransition[PlayersEnum.Idle].Add(PlayersEnum.Fire, PlayersEnum.Fire);
            SMtransition[PlayersEnum.Idle].Add(PlayersEnum.Dead, PlayersEnum.Dead);


            SMtransition.Add(PlayersEnum.WalkRight, new Dictionary<Enum, Enum>());
            SMtransition[PlayersEnum.WalkRight].Add(PlayersEnum.Idle, PlayersEnum.IdleRight);
            SMtransition[PlayersEnum.WalkRight].Add(PlayersEnum.WalkRight, PlayersEnum.WalkRight);
            SMtransition[PlayersEnum.WalkRight].Add(PlayersEnum.WalkLeft, PlayersEnum.WalkRight);
            SMtransition[PlayersEnum.WalkRight].Add(PlayersEnum.WalkUp, PlayersEnum.WalkRight);
            SMtransition[PlayersEnum.WalkRight].Add(PlayersEnum.WalkDown, PlayersEnum.WalkRight);
            SMtransition[PlayersEnum.WalkRight].Add(PlayersEnum.Fire, PlayersEnum.Fire);
            SMtransition[PlayersEnum.WalkRight].Add(PlayersEnum.Dead, PlayersEnum.Dead);

            SMtransition.Add(PlayersEnum.IdleRight, new Dictionary<Enum, Enum>());
            SMtransition[PlayersEnum.IdleRight].Add(PlayersEnum.Idle, PlayersEnum.IdleRight);
            SMtransition[PlayersEnum.IdleRight].Add(PlayersEnum.WalkRight, PlayersEnum.WalkRight);
            SMtransition[PlayersEnum.IdleRight].Add(PlayersEnum.WalkLeft, PlayersEnum.Idle);
            SMtransition[PlayersEnum.IdleRight].Add(PlayersEnum.WalkUp, PlayersEnum.WalkUp);
            SMtransition[PlayersEnum.IdleRight].Add(PlayersEnum.WalkDown, PlayersEnum.WalkDown);
            SMtransition[PlayersEnum.IdleRight].Add(PlayersEnum.Fire, PlayersEnum.Fire);
            SMtransition[PlayersEnum.IdleRight].Add(PlayersEnum.Dead, PlayersEnum.Dead);

            SMtransition.Add(PlayersEnum.WalkLeft, new Dictionary<Enum, Enum>());
            SMtransition[PlayersEnum.WalkLeft].Add(PlayersEnum.Idle, PlayersEnum.IdleLeft);
            SMtransition[PlayersEnum.WalkLeft].Add(PlayersEnum.WalkRight, PlayersEnum.WalkLeft);
            SMtransition[PlayersEnum.WalkLeft].Add(PlayersEnum.WalkLeft, PlayersEnum.WalkLeft);
            SMtransition[PlayersEnum.WalkLeft].Add(PlayersEnum.WalkUp, PlayersEnum.WalkLeft);
            SMtransition[PlayersEnum.WalkLeft].Add(PlayersEnum.WalkDown, PlayersEnum.WalkLeft);
            SMtransition[PlayersEnum.WalkLeft].Add(PlayersEnum.Fire, PlayersEnum.Fire);
            SMtransition[PlayersEnum.WalkLeft].Add(PlayersEnum.Dead, PlayersEnum.Dead);

            SMtransition.Add(PlayersEnum.IdleLeft, new Dictionary<Enum, Enum>());
            SMtransition[PlayersEnum.IdleLeft].Add(PlayersEnum.Idle, PlayersEnum.IdleLeft);
            SMtransition[PlayersEnum.IdleLeft].Add(PlayersEnum.WalkRight, PlayersEnum.Idle);
            SMtransition[PlayersEnum.IdleLeft].Add(PlayersEnum.WalkLeft, PlayersEnum.WalkLeft);
            SMtransition[PlayersEnum.IdleLeft].Add(PlayersEnum.WalkUp, PlayersEnum.WalkUp);
            SMtransition[PlayersEnum.IdleLeft].Add(PlayersEnum.WalkDown, PlayersEnum.WalkDown);
            SMtransition[PlayersEnum.IdleLeft].Add(PlayersEnum.Fire, PlayersEnum.Fire);
            SMtransition[PlayersEnum.IdleLeft].Add(PlayersEnum.Dead, PlayersEnum.Dead);

            SMtransition.Add(PlayersEnum.WalkUp, new Dictionary<Enum, Enum>());
            SMtransition[PlayersEnum.WalkUp].Add(PlayersEnum.Idle, PlayersEnum.IdleUp);
            SMtransition[PlayersEnum.WalkUp].Add(PlayersEnum.WalkRight, PlayersEnum.WalkUp);
            SMtransition[PlayersEnum.WalkUp].Add(PlayersEnum.WalkLeft, PlayersEnum.WalkUp);
            SMtransition[PlayersEnum.WalkUp].Add(PlayersEnum.WalkUp, PlayersEnum.WalkUp);
            SMtransition[PlayersEnum.WalkUp].Add(PlayersEnum.WalkDown, PlayersEnum.WalkUp);
            SMtransition[PlayersEnum.WalkUp].Add(PlayersEnum.Fire, PlayersEnum.Fire);
            SMtransition[PlayersEnum.WalkUp].Add(PlayersEnum.Dead, PlayersEnum.Dead);

            SMtransition.Add(PlayersEnum.IdleUp, new Dictionary<Enum, Enum>());
            SMtransition[PlayersEnum.IdleUp].Add(PlayersEnum.Idle, PlayersEnum.IdleUp);
            SMtransition[PlayersEnum.IdleUp].Add(PlayersEnum.WalkRight, PlayersEnum.WalkRight);
            SMtransition[PlayersEnum.IdleUp].Add(PlayersEnum.WalkLeft, PlayersEnum.WalkLeft);
            SMtransition[PlayersEnum.IdleUp].Add(PlayersEnum.WalkUp, PlayersEnum.WalkUp);
            SMtransition[PlayersEnum.IdleUp].Add(PlayersEnum.WalkDown, PlayersEnum.Idle);
            SMtransition[PlayersEnum.IdleUp].Add(PlayersEnum.Fire, PlayersEnum.Fire);
            SMtransition[PlayersEnum.IdleUp].Add(PlayersEnum.Dead, PlayersEnum.Dead);

            SMtransition.Add(PlayersEnum.WalkDown, new Dictionary<Enum, Enum>());
            SMtransition[PlayersEnum.WalkDown].Add(PlayersEnum.Idle, PlayersEnum.Idle);
            SMtransition[PlayersEnum.WalkDown].Add(PlayersEnum.WalkRight, PlayersEnum.WalkDown);
            SMtransition[PlayersEnum.WalkDown].Add(PlayersEnum.WalkLeft, PlayersEnum.WalkDown);
            SMtransition[PlayersEnum.WalkDown].Add(PlayersEnum.WalkUp, PlayersEnum.WalkDown);
            SMtransition[PlayersEnum.WalkDown].Add(PlayersEnum.WalkDown, PlayersEnum.WalkDown);
            SMtransition[PlayersEnum.WalkDown].Add(PlayersEnum.Fire, PlayersEnum.Fire);
            SMtransition[PlayersEnum.WalkDown].Add(PlayersEnum.Dead, PlayersEnum.Dead);

            SMtransition.Add(PlayersEnum.Fire, new Dictionary<Enum, Enum>());
            SMtransition[PlayersEnum.Fire].Add(PlayersEnum.Idle, PlayersEnum.Fire);
            SMtransition[PlayersEnum.Fire].Add(PlayersEnum.WalkRight, PlayersEnum.Fire);
            SMtransition[PlayersEnum.Fire].Add(PlayersEnum.WalkLeft, PlayersEnum.Fire);
            SMtransition[PlayersEnum.Fire].Add(PlayersEnum.WalkUp, PlayersEnum.Fire);
            SMtransition[PlayersEnum.Fire].Add(PlayersEnum.WalkDown, PlayersEnum.Fire);
            SMtransition[PlayersEnum.Fire].Add(PlayersEnum.Fire, PlayersEnum.Fire);
            SMtransition[PlayersEnum.Fire].Add(PlayersEnum.Dead, PlayersEnum.Dead);

            SMtransition.Add(PlayersEnum.Dead, new Dictionary<Enum, Enum>());
            SMtransition[PlayersEnum.Dead].Add(PlayersEnum.Idle, PlayersEnum.Dead);
            SMtransition[PlayersEnum.Dead].Add(PlayersEnum.WalkRight, PlayersEnum.Dead);
            SMtransition[PlayersEnum.Dead].Add(PlayersEnum.WalkLeft, PlayersEnum.Dead);
            SMtransition[PlayersEnum.Dead].Add(PlayersEnum.WalkUp, PlayersEnum.Dead);
            SMtransition[PlayersEnum.Dead].Add(PlayersEnum.WalkDown, PlayersEnum.Dead);
            SMtransition[PlayersEnum.Dead].Add(PlayersEnum.Fire, PlayersEnum.Dead);
            SMtransition[PlayersEnum.Dead].Add(PlayersEnum.Dead, PlayersEnum.Dead);
        }



        public override void Update(GameTime gameTime)
        {
            if (BombCount != MaxBombCount)
            {
                BombTime += gameTime.ElapsedGameTime.Milliseconds;
                if (BombTime > BombTimeReload)
                {
                    BombCount++;
                    BombTime = 0;
                }
            }

            State.Update(gameTime);

            //Установка бомбы
            if (InputHelper.KeyPressed(Keys.Space))
            {
                TryDropBomb();

            }

            if ((PlayersEnum)SMstate == PlayersEnum.WalkRight || (PlayersEnum)SMstate == PlayersEnum.WalkLeft)
            {
                if (PosWorldX % 48 == 0) SMrequest(PlayersEnum.Idle);
            }

            if ((PlayersEnum)SMstate == PlayersEnum.WalkUp || (PlayersEnum)SMstate == PlayersEnum.WalkDown)
            {
                if (PosWorldY % 48 == 0) SMrequest(PlayersEnum.Idle);
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
                ObjectStates[PlayersEnum.Fire].Animation.SpriteCurentFrameNum = 0;
                ObjectStates[PlayersEnum.Fire].Animation.isAnimated = true;
                PosWorldX = 48;
                PosWorldY = 48;

                SMstate = PlayersEnum.Idle;
                ChangeState(PlayersEnum.Idle);
                return;
            }

            //Для тестовых целей 
            if (InputHelper.IsKeyDown(Keys.T))
            {
                //ObjectStates.Add(PlayerEnum.Fire, new PlayerFire(this));
                ObjectStates[PlayersEnum.Fire].Animation.SpriteCurentFrameNum = 0;
                ObjectStates[PlayersEnum.Fire].Animation.isAnimated = true;
                PosWorldX = 48 * 5+30;
                PosWorldY = 48;

                SMstate = PlayersEnum.Idle;
                ChangeState(PlayersEnum.Idle);
                return;
            }

        }

        /// <summary>Хождение по сторонам</summary>
        private void TryGoDown()
        {
            if (PosWorldY % 48 == 0 && PassabilityCheck(PosWorldX, PosWorldY + 48)) SMrequest(PlayersEnum.WalkDown);
        }

        private void TryGoUp()
        {
            if (PosWorldY % 48 == 0 && PassabilityCheck(PosWorldX, PosWorldY - 48)) SMrequest(PlayersEnum.WalkUp);
        }

        private void TryGoLeft()
        {
            if (PosWorldX%48 == 0 && PassabilityCheck(PosWorldX - 48, PosWorldY)) SMrequest(PlayersEnum.WalkLeft);
        }

        private void TryGoRight()
        {
            if (PosWorldX%48 == 0 && PassabilityCheck(PosWorldX + 48, PosWorldY)) SMrequest(PlayersEnum.WalkRight);
        }

        /// <summary>Игрок пытается установить бомбу</summary>
        public void TryDropBomb()
        {
            int x, y;

            //Debug.WriteLine("Pos: " + PosWorldX + "  " + PosWorldY);

            //если мы в квадрате 
            if (PosWorldY % 48 == 0 && PosWorldX % 48 == 0)
            {
                //Проверяем пуста ли клетка ставим бомбу
                if (isEmptyCell(PosWorldX, PosWorldY)) DropBomb(PosWorldX, PosWorldY);

            }
            else //мы не в квадрате
            {
                //если идем в право
                if ((PlayersEnum)SMstate == PlayersEnum.WalkRight)
                {
                    y = PosWorldY;
                    x = PosWorldX - PosWorldX % 48;
                    if (PosWorldX % 48 > 38)
                    {
                        if (isEmptyCell(x + 48, y)) DropBomb(x + 48, y);
                    }
                    else
                    {
                        if (isEmptyCell(x, y)) DropBomb(x, y);
                    }
                }

                //если идем в низ
                if ((PlayersEnum)SMstate == PlayersEnum.WalkDown)
                {
                    x = PosWorldX;
                    y = PosWorldY - PosWorldY % 48;
                    if (PosWorldY % 48 > 38)
                    {
                        if (isEmptyCell(x, y + 48)) DropBomb(x, y + 48);
                    }
                    else
                    {
                        if (isEmptyCell(x, y)) DropBomb(x, y);
                    }
                }

                //если идем в лево
                if ((PlayersEnum)SMstate == PlayersEnum.WalkLeft)
                {
                    y = PosWorldY;
                    x = PosWorldX - PosWorldX % 48 + 48;
                    if (PosWorldX % 48 > 10)
                    {
                        if (isEmptyCell(x, y)) DropBomb(x, y);
                    }
                    else
                    {
                        if (isEmptyCell(x - 48, y)) DropBomb(x - 48, y);
                    }
                }

                //если идем в верх
                if ((PlayersEnum)SMstate == PlayersEnum.WalkUp)
                {
                    x = PosWorldX;
                    y = PosWorldY - PosWorldY % 48 + 48;
                    if (PosWorldY % 48 > 10)
                    {
                        if (isEmptyCell(x, y)) DropBomb(x, y);
                    }
                    else
                    {
                        if (isEmptyCell(x, y - 48)) DropBomb(x, y - 48);
                    }
                }

            }
        }

        //установка бомбы по ранее расшитаным координатам
        private void DropBomb(int x,int y)
        {
            if (BombCount == MaxBombCount) BombTime = 0;
            
            if (BombCount > 0)
            {
                BombCount--;
                BombGun.DropBomb(x, y);
            }
        }

        /// <summary>пуста ли клетка ?? проверяется при установки бомбы</summary>
        public bool isEmptyCell(int x, int y)
        {
            //return GameObjects.FindAll(o => o.PosWorldX == x && o.PosWorldY == y).Count == 1;
            foreach (GameObject O in GameObjects)
                if (O.PosWorldX == x && O.PosWorldY == y && O.isPassability == false) return false;
            //иначе квадрат проходим
            return true;
        }

        /// <summary>Игрок подорвался на бомбе</summary>
        public void Blow(BlowSide side)
        {
            ChangeState(PlayersEnum.Fire);
        }

        /// <summary>Воскрешение послесмерти</summary>
        public void Resurrection()
        {
            Lives--;
            BombCount = 3;
            MaxBombCount = 3;
            BombTimeReload=3000;
            BombGun=new SampleBombGun(GameObjects);

            if(Lives<0)GamePhaseManager.SwitchTo(Phase.GameOver);
            else
            {
                SMstate = PlayersEnum.WalkLeft;
                ChangeState(PlayersEnum.Idle);
                PosWorldX = StartPos.X;
                PosWorldY = StartPos.Y;
            }
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
            }, 80, true, false,Resurrection);

            
        }

        /// <summary>Воскрешение послесмерти</summary>
        private void Resurrection()
        {
            Animation.SpriteCurentFrameNum = 0;
            Animation.isAnimated = true;
            ((CPlayer)GameObject).Resurrection();
        }

        public override void OnChange()
        {
            SoundEngine.GetEffect(SoundNames.PlayerBurn).CreateInstance().Play();
        }
    }

    /// <summary>Игрок горит (подорвался на бомбе)</summary>
    public class PlayerDead : State
    {
        public PlayerDead(GameObject player)
            : base(player)
        {
            Animation = new Animation(new List<Rectangle>()
            {
                new Rectangle(0*48, 8*48, 48, 48),new Rectangle(1*48, 8*48, 48, 48),new Rectangle(2*48, 8*48, 48, 48),
                new Rectangle(3*48, 8*48, 48, 48),new Rectangle(4*48, 8*48, 48, 48),new Rectangle(5*48, 8*48, 48, 48),
           }, 80, true, false, Resurrection);


        }

        /// <summary>Воскрешение послесмерти</summary>
        private void Resurrection()
        {
            Animation.SpriteCurentFrameNum = 0;
            Animation.isAnimated = true;
            ((CPlayer)GameObject).Resurrection();
        }
    }
}

