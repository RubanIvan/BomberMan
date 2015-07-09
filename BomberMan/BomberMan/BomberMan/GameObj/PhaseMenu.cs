﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberMan
{
    enum MenyEnum
    {
        Play,
        Exit
    }

    class PhaseMenu : GamePhaseObject
    {
        /// <summary>Позиция на экране для вывода заднего фона</summary>
        Rectangle BkgTo = new Rectangle(0, 0, 800, 600);
        /// <summary>Позиция заднего фона в текстуре</summary>
        Rectangle BkgFrom = new Rectangle(16, 0, 800, 600);

        MenyEnum MenuState = MenyEnum.Play;

        private int ElapsTime;

        private Fire Fire;

        public PhaseMenu(Texture2D texture, SpriteBatch spriteBatch,SpriteFont font)
            : base(texture, spriteBatch,font)
        {
            Fire = new Fire();
        }

        public override void Update(GameTime gameTime)
        {
            Fire.Update(gameTime);
            ElapsTime += gameTime.ElapsedGameTime.Milliseconds;
            if (ElapsTime > 500)
            {
                if (InputHelper.KeyPressed(Keys.Left))
                {
                    if (MenuState == MenyEnum.Play) { ChangeMenu(MenyEnum.Exit); return; }
                    if (MenuState == MenyEnum.Exit) { ChangeMenu(MenyEnum.Play); return; }
                }

                if (InputHelper.KeyPressed(Keys.Right))
                {
                    if (MenuState == MenyEnum.Play) { ChangeMenu(MenyEnum.Exit); return; }
                    if (MenuState == MenyEnum.Exit) { ChangeMenu(MenyEnum.Play); return; }
                }

                if (InputHelper.KeyPressed(Keys.Space) || InputHelper.KeyPressed(Keys.Enter))
                {
                    if (MenuState == MenyEnum.Play) { GamePhaseManager.SwitchTo(Phase.PlayGame); return; }
                    if (MenuState == MenyEnum.Exit) { GamePhaseManager.SwitchTo(Phase.Exit); return; }
                }



            }
        }

        public override void Draw()
        {

            //Задний фон
            SpriteBatch.Draw(Texture, BkgTo, BkgFrom, Color.White);

            //Анимация пламени
            SpriteBatch.Draw(Texture, new Rectangle(Fire.PosWorldX, Fire.PosWorldY, 80, 90), Fire.Sprite, Color.White);

        }

        private void ChangeMenu(MenyEnum menuState)
        {
            Fire.ChangePos(menuState);
            MenuState = menuState;
        }

    }

    class PhaseExit : GamePhaseObject
    {
        private Game1 Game;

        public PhaseExit(Game1 game)
            : base(null, null,null)
        {
            Game = game;

        }

        public override void Update(GameTime gameTime)
        {
            Game.Exit();
        }

        public override void Draw()
        {
            Game.Exit();
        }
    }

    class PhaseGameOver : GamePhaseObject
    {
        /// <summary>Позиция на экране для вывода заднего фона</summary>
        Rectangle BkgTo = new Rectangle(0, 0, 800, 600);
        /// <summary>Позиция заднего фона в текстуре</summary>
        Rectangle BkgFrom = new Rectangle(16, 0, 800, 600);

        public PhaseGameOver(Texture2D texture, SpriteBatch spriteBatch,SpriteFont font)
            : base(texture, spriteBatch,font)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (InputHelper.KeyPressed(Keys.Space) || InputHelper.KeyPressed(Keys.Enter))
            {
                GamePhaseManager.SwitchTo(Phase.MainMenu);
            }
        }

        public override void Draw()
        {
            SpriteBatch.Draw(Texture, BkgTo, BkgFrom, Color.White);
        }
    }


    class Fire : GameObject
    {
        public Fire()
            : base(100 - 50, 100 + 35)
        {
            ObjectStates.Add(PlayersEnum.Idle, new State(new Animation(new List<Rectangle>()
            {
                new Rectangle(0 * 80,600+ 0*90, 80, 90),new Rectangle(1 * 80,600+ 0*90, 80, 90),new Rectangle(2 * 80,600+ 0*90, 80, 90),
                new Rectangle(3 * 80,600+ 0*90, 80, 90),new Rectangle(4* 80,600+ 0*90, 80, 90),new Rectangle(5 * 80,600+ 0*90, 80, 90),
                new Rectangle(6 * 80,600+ 0*90, 80, 90),new Rectangle(7 * 80,600+ 0*90, 80, 90),new Rectangle(8 * 80,600+ 0*90, 80, 90),new Rectangle(9 * 80,600+ 0*90, 80, 90),
                new Rectangle(0 * 80,600+ 1*90, 80, 90),new Rectangle(1 * 80,600+ 1*90, 80, 90),new Rectangle(2 * 80,600+ 1*90, 80, 90)
            }, 100, true, true)));

            ChangeState(PlayersEnum.Idle);
        }

        public void ChangePos(MenyEnum state)
        {
            switch (state)
            {
                case MenyEnum.Play:
                    PosWorldX = 50;
                    PosWorldY = 135;
                    break;
                case MenyEnum.Exit:
                    PosWorldX = 650 - 10;
                    PosWorldY = 135;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state", state, null);
            }
        }

        public void Test()
        {

        }
    }




}