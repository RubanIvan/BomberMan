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

    class Player : GameObject
    {

        /// <summary>Время в тиках прошедшее после последнего действия</summary>
        private int ElapsedTime = 0;

        //Количество однавремен
        //private int MaxBombCount = 1;

        //конструктор
        public Player(int x, int y, List<GameObject> O)
            : base(x, y)
        {
            GameObjects = O;

            //Задание состояний
            ObjectStates.Add(PlayerEnum.Idle, new PlayerIdle(this));
            ObjectStates.Add(PlayerEnum.WalkRight, new PlayerWalkRight(this));
            ObjectStates.Add(PlayerEnum.WalkLeft, new PlayerWalkLeft(this));
            ObjectStates.Add(PlayerEnum.IdleRight, new PlayerIdleRight(this));
            ObjectStates.Add(PlayerEnum.IdleLeft, new PlayerIdleLeft(this));
            ObjectStates.Add(PlayerEnum.WalkUp, new PlayerWalkUp(this));
            ObjectStates.Add(PlayerEnum.IdleUp, new PlayerIdleUp(this));
            ObjectStates.Add(PlayerEnum.WalkDown, new PlayerWalkDown(this));


            ChangeState(PlayerEnum.Idle);

            SMtransition.Add(PlayerEnum.Idle, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.Idle].Add(PlayerEnum.Idle, PlayerEnum.Idle);
            SMtransition[PlayerEnum.Idle].Add(PlayerEnum.WalkRight, PlayerEnum.WalkRight);
            SMtransition[PlayerEnum.Idle].Add(PlayerEnum.WalkLeft, PlayerEnum.WalkLeft);
            SMtransition[PlayerEnum.Idle].Add(PlayerEnum.WalkUp, PlayerEnum.WalkUp);
            SMtransition[PlayerEnum.Idle].Add(PlayerEnum.WalkDown, PlayerEnum.WalkDown);

            SMtransition.Add(PlayerEnum.WalkRight, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.WalkRight].Add(PlayerEnum.Idle, PlayerEnum.IdleRight);
            SMtransition[PlayerEnum.WalkRight].Add(PlayerEnum.WalkRight, PlayerEnum.WalkRight);
            SMtransition[PlayerEnum.WalkRight].Add(PlayerEnum.WalkLeft, PlayerEnum.WalkRight);
            SMtransition[PlayerEnum.WalkRight].Add(PlayerEnum.WalkUp, PlayerEnum.WalkRight);
            SMtransition[PlayerEnum.WalkRight].Add(PlayerEnum.WalkDown, PlayerEnum.WalkRight);

            SMtransition.Add(PlayerEnum.IdleRight, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.IdleRight].Add(PlayerEnum.Idle, PlayerEnum.IdleRight);
            SMtransition[PlayerEnum.IdleRight].Add(PlayerEnum.WalkRight, PlayerEnum.WalkRight);
            SMtransition[PlayerEnum.IdleRight].Add(PlayerEnum.WalkLeft, PlayerEnum.Idle);
            SMtransition[PlayerEnum.IdleRight].Add(PlayerEnum.WalkUp, PlayerEnum.IdleUp);
            SMtransition[PlayerEnum.IdleRight].Add(PlayerEnum.WalkDown, PlayerEnum.Idle);

            SMtransition.Add(PlayerEnum.WalkLeft, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.WalkLeft].Add(PlayerEnum.Idle, PlayerEnum.IdleLeft);
            SMtransition[PlayerEnum.WalkLeft].Add(PlayerEnum.WalkRight, PlayerEnum.WalkLeft);
            SMtransition[PlayerEnum.WalkLeft].Add(PlayerEnum.WalkLeft, PlayerEnum.WalkLeft);
            SMtransition[PlayerEnum.WalkLeft].Add(PlayerEnum.WalkUp, PlayerEnum.WalkLeft);
            SMtransition[PlayerEnum.WalkLeft].Add(PlayerEnum.WalkDown, PlayerEnum.WalkLeft);

            SMtransition.Add(PlayerEnum.IdleLeft, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.IdleLeft].Add(PlayerEnum.Idle, PlayerEnum.IdleLeft);
            SMtransition[PlayerEnum.IdleLeft].Add(PlayerEnum.WalkRight, PlayerEnum.Idle);
            SMtransition[PlayerEnum.IdleLeft].Add(PlayerEnum.WalkLeft, PlayerEnum.WalkLeft);
            SMtransition[PlayerEnum.IdleLeft].Add(PlayerEnum.WalkUp, PlayerEnum.IdleUp);
            SMtransition[PlayerEnum.IdleLeft].Add(PlayerEnum.WalkDown, PlayerEnum.Idle);

            SMtransition.Add(PlayerEnum.WalkUp, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.WalkUp].Add(PlayerEnum.Idle, PlayerEnum.IdleUp);
            SMtransition[PlayerEnum.WalkUp].Add(PlayerEnum.WalkRight, PlayerEnum.WalkUp);
            SMtransition[PlayerEnum.WalkUp].Add(PlayerEnum.WalkLeft, PlayerEnum.WalkUp);
            SMtransition[PlayerEnum.WalkUp].Add(PlayerEnum.WalkUp, PlayerEnum.WalkUp);
            SMtransition[PlayerEnum.WalkUp].Add(PlayerEnum.WalkDown, PlayerEnum.WalkUp);

            SMtransition.Add(PlayerEnum.IdleUp, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.IdleUp].Add(PlayerEnum.Idle, PlayerEnum.IdleUp);
            SMtransition[PlayerEnum.IdleUp].Add(PlayerEnum.WalkRight, PlayerEnum.Idle);
            SMtransition[PlayerEnum.IdleUp].Add(PlayerEnum.WalkLeft, PlayerEnum.Idle);
            SMtransition[PlayerEnum.IdleUp].Add(PlayerEnum.WalkUp, PlayerEnum.WalkUp);
            SMtransition[PlayerEnum.IdleUp].Add(PlayerEnum.WalkDown, PlayerEnum.Idle);

            SMtransition.Add(PlayerEnum.WalkDown, new Dictionary<Enum, Enum>());
            SMtransition[PlayerEnum.WalkDown].Add(PlayerEnum.Idle, PlayerEnum.Idle);
            SMtransition[PlayerEnum.WalkDown].Add(PlayerEnum.WalkRight, PlayerEnum.WalkDown);
            SMtransition[PlayerEnum.WalkDown].Add(PlayerEnum.WalkLeft, PlayerEnum.WalkDown);
            SMtransition[PlayerEnum.WalkDown].Add(PlayerEnum.WalkUp, PlayerEnum.WalkDown);
            SMtransition[PlayerEnum.WalkDown].Add(PlayerEnum.WalkDown, PlayerEnum.WalkDown);

        }


        public override void Update(GameTime gameTime)
        {

            State.Update(gameTime);

            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (ElapsedTime > 10)
            {
                ElapsedTime = 0;


                if ((PlayerEnum)SMstate == PlayerEnum.WalkRight || (PlayerEnum)SMstate == PlayerEnum.WalkLeft)
                {
                    if (PosWorldX % 48 == 0) SMrequest(PlayerEnum.Idle);
                    return;
                }

                if ((PlayerEnum)SMstate == PlayerEnum.WalkUp || (PlayerEnum)SMstate == PlayerEnum.WalkDown)
                {
                    if (PosWorldY % 48 == 0) SMrequest(PlayerEnum.Idle);
                    return;
                }


                if (InputHelper.IsKeyDown(Keys.Right))
                {
                    //если мы не в квадрате то выходим
                    if (PosWorldX % 48 != 0) return;

                    //Проверка на возможность входа в квадрат
                    foreach (GameObject O in GameObjects)
                        if (O.PosWorldX == PosWorldX + 48 && O.PosWorldY == PosWorldY && O.isPassability == false) return;
                    
                    SMrequest(PlayerEnum.WalkRight);

                }


                if (InputHelper.IsKeyDown(Keys.Left))
                {
                    //если мы не в квадрате то выходим
                    if (PosWorldX % 48 != 0) return;

                    foreach (GameObject O in GameObjects)
                        if (O.PosWorldX == PosWorldX - 48 && O.PosWorldY == PosWorldY && O.isPassability == false) return;
                    
                    SMrequest(PlayerEnum.WalkLeft);
                    
                }

                if (InputHelper.IsKeyDown(Keys.Up))
                {
                    //если мы не в квадрате то выходим
                    if (PosWorldY % 48 != 0) return;

                    foreach (GameObject O in GameObjects)
                        if (O.PosWorldX == PosWorldX  && O.PosWorldY+48 == PosWorldY && O.isPassability == false) return;

                    SMrequest(PlayerEnum.WalkUp);
                    
                }

                if (InputHelper.IsKeyDown(Keys.Down))
                {
                    //если мы не в квадрате то выходим
                    if (PosWorldY % 48 != 0) return;

                    foreach (GameObject O in GameObjects)
                        if (O.PosWorldX == PosWorldX && O.PosWorldY - 48 == PosWorldY && O.isPassability == false) return;

                    SMrequest(PlayerEnum.WalkDown);
                    //return;
                }

                //Установка бомбы
                if (InputHelper.IsKeyDown(Keys.Space))
                {
                    //если мы не в квадрате то выходим
                    if (PosWorldY%48 != 0) return;

                    //В клетке с бомбой может быть только 2 элемента игрок и пустое поле тогда только ставим бомбу
                    if( GameObjects.FindAll(o => o.PosWorldX == PosWorldX && o.PosWorldY == PosWorldY).Count==2)
                        GameObjects.Add(new Bomb(PosWorldX, PosWorldY, GameObjects));

                    return;
                }
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

}




//            ObjectStates.Add(PlayerEnum.Fire, new State(new Animation(new List<Rectangle>()
//            {
//                new Rectangle(0 * 48, 30 * 48, 48, 48),new Rectangle(1 * 48, 30 * 48, 48, 48),new Rectangle(2 * 48, 30 * 48, 48, 48),
//                new Rectangle(3 * 48, 30 * 48, 48, 48),new Rectangle(4 * 48, 30 * 48, 48, 48),new Rectangle(5 * 48, 30 * 48, 48, 48),
//                new Rectangle(6 * 48, 30 * 48, 48, 48),new Rectangle(7 * 48, 30 * 48, 48, 48),new Rectangle(8 * 48, 30 * 48, 48, 48),
//                new Rectangle(9 * 48, 30 * 48, 48, 48),new Rectangle(10 * 48, 30 * 48, 48, 48),new Rectangle(11 * 48, 30 * 48, 48, 48),
//                new Rectangle(12 * 48, 30 * 48, 48, 48),new Rectangle(13 * 48, 30 * 48, 48, 48),new Rectangle(14 * 48, 30 * 48, 48, 48),
//                new Rectangle(15 * 48, 30 * 48, 48, 48),new Rectangle(16 * 48, 30 * 48, 48, 48),new Rectangle(17 * 48, 30 * 48, 48, 48),
//                new Rectangle(18 * 48, 30 * 48, 48, 48),new Rectangle(19 * 48, 30 * 48, 48, 48),
//                new Rectangle(0 * 48, 31 * 48, 48, 48),new Rectangle(1 * 48, 31 * 48, 48, 48),
//            }, 80, true, false)));