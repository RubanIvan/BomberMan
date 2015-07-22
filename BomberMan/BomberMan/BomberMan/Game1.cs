using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BomberMan
{
    /// <summary>Основные константы</summary>
    public static class Const
    {
        /// <summary>Размер экрана</summary>
        public const int ScrDx = 800 - 32;
        public const int ScrDy = 600;
    }

    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;
        Texture2D Texture;
        SpriteFont Font;
        SoundEngine SoundEngine=new SoundEngine();

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            Graphics.PreferredBackBufferHeight = Const.ScrDy;
            Graphics.PreferredBackBufferWidth = Const.ScrDx;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>LoadContent will be called once per game and is the place to load all of your content.</summary>
        protected override void LoadContent()
        {
            SoundEngine.SoundInit(Content);

            Font = Content.Load<SpriteFont>("font");

            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            //фаза меню
            GamePhaseManager.Add(Phase.MainMenu, new PhaseMenu(Content.Load<Texture2D>("menu bkg"), SpriteBatch,Font,SoundEngine));
            
            //фаза игры
            GamePhaseManager.Add(Phase.PlayGame, new PhasePlayGame(Content.Load<Texture2D>("spritesheet"), SpriteBatch,Font,SoundEngine));

            GamePhaseManager.Add(Phase.GameOver, new PhaseGameOver(Content.Load<Texture2D>("GameOver"), SpriteBatch,Font,SoundEngine));

            GamePhaseManager.Add(Phase.HiScore, new PhaseHiScore(Content.Load<Texture2D>("highscores"), SpriteBatch, Font, SoundEngine));

            GamePhaseManager.Add(Phase.Exit, new PhaseExit(this));

            GamePhaseManager.SwitchTo(Phase.HiScore);

            
        }

        /// <summary>UnloadContent will be called once per game and is the place to unload all content.</summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>Allows the game to run logic such as updating the world, checking for collisions, gathering input, and playing audio.</summary>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();
            if (InputHelper.IsKeyDown(Keys.Escape)) this.Exit();

            //Обновляем состояние игры
            GamePhaseManager.CurrentPhase.Update(gameTime);

            //Обновляем состояние клавиатуры
            InputHelper.Update();

            base.Update(gameTime);
            
        }

        /// <summary>Отрисовываем текущее состояние</summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin();

            GamePhaseManager.CurrentPhase.Draw();
            
            SpriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}

