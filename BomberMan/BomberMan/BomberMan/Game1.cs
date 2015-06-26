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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;
        Texture2D Texture;

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
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Texture = Content.Load<Texture2D>("spritesheet");

            GamePhaseManager.Add(Phase.PlayGame, new PhasePlayGame(Texture, SpriteBatch));
            GamePhaseManager.SwitchTo(Phase.PlayGame);

            
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
