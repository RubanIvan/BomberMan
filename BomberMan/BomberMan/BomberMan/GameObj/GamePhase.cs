using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan
{
    /// <summary>Фазы игры (игра, настройки, начальное, меню) </summary>
    public enum Phase
    {
        Exit,
        /// <summary>Начальное меню</summary>
        MainMenu,
        /// <summary>Сама игра</summary>
        PlayGame,

        GameOver,

        HiScore
        


    }

    /// <summary>Управляет фазами игры </summary>
    static class GamePhaseManager
    {
        /// <summary>Хранит все возможные фазы игры</summary>
        private static Dictionary<Phase, GamePhaseObject> GamePhases = new Dictionary<Phase, GamePhaseObject>();

        private static GamePhaseObject _CurrentPhase;
        /// <summary>Текущее состояние игры</summary>
        public static GamePhaseObject CurrentPhase
        {
            get { return _CurrentPhase; }
            set { _CurrentPhase = value; }
            
        }

        /// <summary>Добавление фазы </summary>
        public static void Add(Phase phase, GamePhaseObject gamestate)
        {
            GamePhases[phase] = gamestate;
        }

        /// <summary>Переключение состояний</summary>
        public static void SwitchTo(Phase name)
        {
            CurrentPhase = GamePhases[name];
            CurrentPhase.Reset();
        }

    }

    /// <summary>Каждая часть игры (меню,список очков,настройки) должна иметь эти методы и свойства</summary>
    abstract class GamePhaseObject
    {
        //public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public Texture2D Texture;
        public SpriteFont Font;
      

        /// <summary>Список всех объектов</summary>
        protected List<GameObject> GameObjects;
        
        //Конструктор
        public GamePhaseObject(Texture2D texture, SpriteBatch spriteBatch,SpriteFont font)
        {
            GameObjects=new List<GameObject>();
            Texture = texture;
            SpriteBatch = spriteBatch;
            Font = font;
            
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw();

        public virtual void Reset(){}

        
    }

}
