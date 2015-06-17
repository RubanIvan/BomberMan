using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.GameObj
{
    /// <summary>Управляет состоянием игры</summary>
    static class GameStateManager
    {
        /// <summary>Хранит все возможные состояния игры</summary>
        private static Dictionary<State, GameStateObject> GameStates = new Dictionary<State, GameStateObject>();

        /// <summary>Текущее состояние игры</summary>
        public static GameStateObject CurrentState = null;

        /// <summary>Добавление состояния </summary>
        public static void Add(State state, GameStateObject gamestate)
        {
            GameStates[state] = gamestate;
        }

        /// <summary>Переключение состояний</summary>
        public static void SwitchTo(State name)
        {
#if DEBUG
            if (!GameStates.ContainsKey(name))
            {
                Debug.WriteLine("Could not find game state: " + name);
                throw new KeyNotFoundException("Could not find game state: " + name);
            }
#endif

            CurrentState = GameStates[name];
        }

    }

    /// <summary>Каждая часть игры (меню,список очков,настройки) должна иметь эти методы и свойства</summary>
    abstract class GameStateObject
    {
        //public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public Texture2D Texture;

        /// <summary>Список всех объектов</summary>
        protected List<GameObject> GameObjects;

        public GameStateObject(Texture2D texture, SpriteBatch spriteBatch)
        {
            GameObjects=new List<GameObject>();
            Texture = texture;
            SpriteBatch = spriteBatch;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw();

    }

}
