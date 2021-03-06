﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace BomberMan
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

    public abstract class BombObject : GameObject, Iexterminable
    {
        
        /// <summary>Сила взрыва</summary>
        protected int ExplPower = 1;

        /// <summary>Правый-левый рукав взрыва</summary>
        protected SortedSet<Point> ExpListX = new SortedSet<Point>(new SortByX());

        /// <summary>Верхний-нижний рукав взрыва</summary>
        protected SortedSet<Point> ExpListY = new SortedSet<Point>(new SortByY());

        SoundEffectInstance BurnInstance = SoundEngine.GetEffect(SoundNames.BombBurn).CreateInstance();

        public BombObject(int x, int y, List<GameObject> O)
        {
            GameObjects = O;
            
            BurnInstance.Play();

            Zorder = Zorders.Bomb;

            PosWorldX = x;
            PosWorldY = y;

            ObjectStates.Add(BombEnum.Explosion, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(12 * 48, 2 * 48, 48, 48)
            }, 10, true, false, Explosion)));

        }

        /// <summary>Разлет пламени взрыва</summary>
        public void Explosion()
        {
            BurnInstance.Stop();
            SoundEngine.GetEffect(SoundNames.BombBlow).CreateInstance().Play();

            //объект бомба надо удалить
            isAlive = false;

            //конец цепочки взрывов
            bool ExplEnd = false;

            //ставим центр взрыва
            GameObjects.Add(new ExplCentr(PosWorldX, PosWorldY, GameObjects,Player));

            //взрываем центр центр
            foreach (GameObject O in GameObjects)
            {
                if (O.PosWorldX == PosWorldX && O.PosWorldY == PosWorldY)
                    if (O is Iexterminable) ((Iexterminable) (O)).Blow(BlowSide.Right);
            }

            #region Правая ветвь взрыва
            //Проходим по всей возможной длинне взрыва
            for (int i = 0; i < ExplPower && !ExplEnd; i++)
            {
                //Перебираем все объекты
                foreach (GameObject O in GameObjects)
                {
                    if (O.PosWorldX == PosWorldX + (48 * (i + 1)) && O.PosWorldY == PosWorldY)
                    {
                        //Если встретили стену. прерываем
                        if (O is StoneWall || O is SteelWall) { ExplEnd = true; break; }

                        //заносим коордитаы взрыва в список
                        ExpListX.Add(new Point(O.PosWorldX, O.PosWorldY));

                        //взрываем объект который попал в плямя взрыва
                        if (O is Iexterminable) ((Iexterminable)(O)).Blow(BlowSide.Right);

                        if (O is BrickWall) { ExplEnd = true; break; }

                        continue;

                    } //если объект несовсем в сетке
                    else if ((O.PosWorldY - PosWorldY) * (O.PosWorldY - PosWorldY) +
                             (O.PosWorldX - (PosWorldX + (48 * (i + 1)))) * (O.PosWorldX - (PosWorldX + (48 * (i + 1))))
                             < 48 * 48)
                    {
                        if (O is Iexterminable) ((Iexterminable)(O)).Blow(BlowSide.Right);
                    }
                }
            }

            //отображаем получившуюся цепочку взрыва
            if (ExpListX.Count > 0)
            {

                for (int i = 0; i < ExpListX.Count - 1; i++)
                {
                    GameObjects.Add(new ExplRight(ExpListX.ElementAt(i).X, ExpListX.ElementAt(i).Y, GameObjects,Player));
                }
                //ставим концовку взрыва
                GameObjects.Add(new ExplRightEnd(ExpListX.Last().X, ExpListX.Last().Y, GameObjects,Player));
            }
            #endregion

            #region Левая ветвь взрыва

            ExpListX.Clear();
            ExplEnd = false;

            //Проходим по всей возможной длинне взрыва
            for (int i = 0; i < ExplPower && !ExplEnd; i++)
            {
                //Перебираем все объекты
                foreach (GameObject O in GameObjects)
                {
                    if (O.PosWorldX == PosWorldX - (48 * (i + 1)) && O.PosWorldY == PosWorldY)
                    {
                        //Если встретили стену. прерываем
                        if (O is StoneWall || O is SteelWall) { ExplEnd = true; break; }

                        //заносим коордитаы взрыва в список
                        ExpListX.Add(new Point(O.PosWorldX, O.PosWorldY));

                        //взрываем объект который попал в плямя взрыва
                        if (O is Iexterminable) ((Iexterminable)(O)).Blow(BlowSide.Left);

                        if (O is BrickWall) { ExplEnd = true; break; }

                    }//если объект несовсем в сетке
                    else if ((O.PosWorldY - PosWorldY) * (O.PosWorldY - PosWorldY) +
                             (O.PosWorldX - (PosWorldX - (48 * (i + 1)))) * (O.PosWorldX - (PosWorldX - (48 * (i + 1))))
                             < 48 * 48)
                    {
                        if (O is Iexterminable) ((Iexterminable)(O)).Blow(BlowSide.Right);
                    }
                }
            }

            //отображаем получившуюся цепочку взрыва
            if (ExpListX.Count > 0)
            {
                for (int i = ExpListX.Count - 1; i > 0; i--)
                {
                    GameObjects.Add(new ExplLeft(ExpListX.ElementAt(i).X, ExpListX.ElementAt(i).Y, GameObjects,Player));
                }
                //ставим концовку взрыва
                GameObjects.Add(new ExplLeftEnd(ExpListX.First().X, ExpListX.First().Y, GameObjects,Player));
            }
            #endregion

            #region Нижняя ветвь взрыва
            ExplEnd = false;

            //Проходим по всей возможной длинне взрыва
            for (int i = 0; i < ExplPower && !ExplEnd; i++)
            {
                //Перебираем все объекты
                foreach (GameObject O in GameObjects)
                {
                    if (O is CPlayer)
                    {

                    }

                    if (O.PosWorldY == PosWorldY + (48 * (i + 1)) && O.PosWorldX == PosWorldX)
                    {
                        //Если встретили стену. прерываем
                        if (O is StoneWall || O is SteelWall) { ExplEnd = true; break; }

                        //заносим коордитаы взрыва в список
                        ExpListY.Add(new Point(O.PosWorldX, O.PosWorldY));

                        //взрываем объект который попал в плямя взрыва
                        if (O is Iexterminable) ((Iexterminable)(O)).Blow(BlowSide.Down);

                        if (O is BrickWall) { ExplEnd = true; break; }

                    }//если объект несовсем в сетке
                    else if ((O.PosWorldX - PosWorldX) * (O.PosWorldX - PosWorldX) +
                             (O.PosWorldY - (PosWorldY + (48 * (i + 1)))) * (O.PosWorldY - (PosWorldY + (48 * (i + 1))))
                             < 48 * 48)
                    {
                        if (O is Iexterminable) ((Iexterminable)(O)).Blow(BlowSide.Right);
                    }
                }
            }

            //отображаем получившуюся цепочку взрыва
            if (ExpListY.Count > 0)
            {

                for (int i = 0; i < ExpListY.Count - 1; i++)
                {
                    GameObjects.Add(new ExplDown(ExpListY.ElementAt(i).X, ExpListY.ElementAt(i).Y, GameObjects,Player));
                }
                //ставим концовку взрыва
                GameObjects.Add(new ExplDownEnd(ExpListY.Last().X, ExpListY.Last().Y, GameObjects,Player));
            }
            #endregion

            #region Верхняя ветвь взрыва
            ExplEnd = false;
            ExpListY.Clear();

            //Проходим по всей возможной длинне взрыва
            for (int i = 0; i < ExplPower && !ExplEnd; i++)
            {
                //Перебираем все объекты
                foreach (GameObject O in GameObjects)
                {
                    if (O.PosWorldY == PosWorldY - (48 * (i + 1)) && O.PosWorldX == PosWorldX)
                    {
                        //Если встретили стену. прерываем
                        if (O is StoneWall || O is SteelWall) { ExplEnd = true; break; }

                        //заносим коордитаы взрыва в список
                        ExpListY.Add(new Point(O.PosWorldX, O.PosWorldY));

                        //взрываем объект который попал в плямя взрыва
                        if (O is Iexterminable) ((Iexterminable)(O)).Blow(BlowSide.Up);

                        if (O is BrickWall) { ExplEnd = true; break; }

                    }
                    else if ((O.PosWorldX - PosWorldX) * (O.PosWorldX - PosWorldX) +
                             (O.PosWorldY - (PosWorldY - (48 * (i + 1)))) * (O.PosWorldY - (PosWorldY - (48 * (i + 1))))
                             < 48 * 48)
                    {
                        if (O is Iexterminable) ((Iexterminable)(O)).Blow(BlowSide.Right);
                    }
                }
            }

            //отображаем получившуюся цепочку взрыва
            if (ExpListY.Count > 0)
            {

                for (int i = ExpListY.Count - 1; i > 0; i--)
                {
                    GameObjects.Add(new ExplUp(ExpListY.ElementAt(i).X, ExpListY.ElementAt(i).Y, GameObjects,Player));
                }
                //ставим концовку взрыва
                GameObjects.Add(new ExplUpEnd(ExpListY.First().X, ExpListY.First().Y, GameObjects,Player));
            }
            #endregion
        }

        /// <summary>Если бомба попала под другой взрыв </summary>
        public void Blow(BlowSide side)
        {
            ChangeState(BombEnum.Explosion);
        }

    }


    public class SampleBomb : BombObject
    {
        //Конструктор
        public SampleBomb(int x, int y, List<GameObject> O)
            : base(x, y, O)
        {
            //Установка силы взрыва
            ExplPower = 1;

            //добавсляем состояние огненных лучей (одинаковое для всех взрывов)
            ObjectStates.Add(BombEnum.Inflation, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(4 * 48, 6 * 48, 48, 48),new Rectangle(3 * 48, 6 * 48, 48, 48),new Rectangle(2 * 48, 6 * 48, 48, 48),new Rectangle(1 * 48, 6 * 48, 48, 48),new Rectangle(0 * 48, 6 * 48, 48, 48),
                new Rectangle(0 * 48, 5 * 48, 48, 48), new Rectangle(1 * 48, 5 * 48, 48, 48),new Rectangle(2 * 48, 5 * 48, 48, 48),
                new Rectangle(3 * 48, 5 * 48, 48, 48),new Rectangle(4 * 48, 5 * 48, 48, 48),new Rectangle(5 * 48, 5 * 48, 48, 48),
                new Rectangle(6 * 48, 5 * 48, 48, 48),new Rectangle(7 * 48, 5 * 48, 48, 48),new Rectangle(8 * 48, 5 * 48, 48, 48),
                new Rectangle(9 * 48, 5 * 48, 48, 48),new Rectangle(10 * 48, 5 * 48, 48, 48),new Rectangle(11 * 48, 5 * 48, 48, 48)
            }, 120, true, false, Explosion)));

            ChangeState(BombEnum.Inflation);

        }
    }

    public class MidleBomb : BombObject
    {
        //Конструктор
        public MidleBomb(int x, int y, List<GameObject> O)
            : base(x, y, O)
        {
            //Установка силы взрыва
            ExplPower = 2;

            //добавсляем состояние огненных лучей (одинаковое для всех взрывов)
            ObjectStates.Add(BombEnum.Inflation, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(4 * 48, 33 * 48, 48, 48),new Rectangle(3 * 48, 33 * 48, 48, 48),new Rectangle(2 * 48, 33 * 48, 48, 48),new Rectangle(1 * 48, 33 * 48, 48, 48),new Rectangle(0 * 48, 33 * 48, 48, 48),
                new Rectangle(0 * 48, 32 * 48, 48, 48),new Rectangle(1 * 48, 32 * 48, 48, 48),new Rectangle(2 * 48, 32 * 48, 48, 48),
                new Rectangle(3 * 48, 32 * 48, 48, 48),new Rectangle(4 * 48, 32 * 48, 48, 48),new Rectangle(5 * 48, 32 * 48, 48, 48),
                new Rectangle(6 * 48, 32 * 48, 48, 48),new Rectangle(7 * 48, 32 * 48, 48, 48),new Rectangle(8 * 48, 32 * 48, 48, 48),
                new Rectangle(9 * 48, 32 * 48, 48, 48),new Rectangle(10 * 48, 32 * 48, 48, 48),new Rectangle(11 * 48, 32 * 48, 48, 48)
            }, 120, true, false, Explosion)));

            ChangeState(BombEnum.Inflation);

        }


    }


    public class BigBomb : BombObject
    {
        //Конструктор
        public BigBomb(int x, int y, List<GameObject> O)
            : base(x, y, O)
        {
            //Установка силы взрыва
            ExplPower = 3;

            //добавсляем состояние огненных лучей (одинаковое для всех взрывов)
            ObjectStates.Add(BombEnum.Inflation, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(4 * 48, 35 * 48, 48, 48),new Rectangle(3 * 48, 35 * 48, 48, 48),new Rectangle(2 * 48, 35 * 48, 48, 48),new Rectangle(1 * 48, 35 * 48, 48, 48),new Rectangle(0 * 48, 35 * 48, 48, 48),
                new Rectangle(0 * 48, 34 * 48, 48, 48),new Rectangle(1 * 48, 34 * 48, 48, 48),new Rectangle(2 * 48, 34 * 48, 48, 48),
                new Rectangle(3 * 48, 34 * 48, 48, 48),new Rectangle(4 * 48, 34 * 48, 48, 48),new Rectangle(5 * 48, 34 * 48, 48, 48),
                new Rectangle(6 * 48, 34 * 48, 48, 48),new Rectangle(7 * 48, 34 * 48, 48, 48),new Rectangle(8 * 48, 34 * 48, 48, 48),
                new Rectangle(9 * 48, 34 * 48, 48, 48),new Rectangle(10 * 48, 34 * 48, 48, 48),new Rectangle(11 * 48, 34 * 48, 48, 48)
            }, 120, true, false, Explosion)));

            ChangeState(BombEnum.Inflation);

        }


    }



    /// <summary>Общий интерфейс для всех пушек</summary>
    public interface IBombarda
    {
        /// <summary>Установить бомбу</summary>
        void DropBomb(int x, int y);
    }

    /// <summary>Базовый клас пушек</summary>
    public abstract class BombGunObject : IBombarda
    {
        /// <summary>Ссылка на все игровые объекты</summary>
        protected List<GameObject> GameObjects;

        /// <summary>Конструктор</summary>
        public BombGunObject(List<GameObject> O) { GameObjects = O; }

        /// <summary>Установить бомбу</summary>
        public virtual void DropBomb(int x, int y)
        {
            SoundEngine.GetEffect(SoundNames.BombDrop).CreateInstance().Play();
        }
    }

    /// <summary>Пушка устанавливающая простые бомбы</summary>
    public class SampleBombGun : BombGunObject
    {
        public SampleBombGun(List<GameObject> O) : base(O) { }

        public override void DropBomb(int x, int y)
        {
            base.DropBomb(x, y);
            GameObjects.Add(new SampleBomb(x, y, GameObjects));
        }

        
    }

    /// <summary>Пушка устанавливающая средние бомбы</summary>
    public class MidleBombGun : BombGunObject
    {
        public MidleBombGun(List<GameObject> O) : base(O) { }

        public override void DropBomb(int x, int y)
        {
            base.DropBomb(x, y);
            GameObjects.Add(new MidleBomb(x, y, GameObjects));
        }
    }

    /// <summary>Пушка устанавливающая большие бомбы</summary>
    public class BigBombGun : BombGunObject
    {
        public BigBombGun(List<GameObject> O) : base(O) { }

        public override void DropBomb(int x, int y)
        {
            base.DropBomb(x, y);
            GameObjects.Add(new BigBomb(x, y, GameObjects));
        }
    }

    /// <summary>Центр взрыва</summary>
    public class ExplCentr : GameObject
    {

        public ExplCentr(int x, int y, List<GameObject> O,CPlayer p)
            : base(x, y,p)
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

        public ExplUp(int x, int y, List<GameObject> O,CPlayer p)
            : base(x, y,p)
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

        public ExplUpEnd(int x, int y, List<GameObject> O,CPlayer p)
            : base(x, y,p)
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

        public ExplDown(int x, int y, List<GameObject> O,CPlayer p)
            : base(x, y,p)
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

        public ExplDownEnd(int x, int y, List<GameObject> O,CPlayer p)
            : base(x, y,p)
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

        public ExplRight(int x, int y, List<GameObject> O,CPlayer p)
            : base(x, y,p)
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

        public ExplRightEnd(int x, int y, List<GameObject> O,CPlayer p)
            : base(x, y,p)
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

        public ExplLeft(int x, int y, List<GameObject> O,CPlayer p)
            : base(x, y,p)
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

        public ExplLeftEnd(int x, int y, List<GameObject> O,CPlayer p)
            : base(x, y,p)
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

    public class SortByX : IComparer<Point>
    {
        public int Compare(Point x, Point y)
        {
            if (x.X > y.X) return 1;
            if (x.X < y.X) return -1;
            return 0;
        }
    }

    public class SortByY : IComparer<Point>
    {
        public int Compare(Point x, Point y)
        {
            if (x.Y > y.Y) return 1;
            if (x.Y < y.Y) return -1;
            return 0;
        }
    }
}
