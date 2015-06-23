using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.GameObj
{
    

    /// <summary>Интерфейс для объектов которые могут быть уничтожены взрывом</summary>
    interface Iexterminable
    {
        void Blow(BlowSide side);
    }

    /// <summary>Состояние объекта </summary>
    public class State
    {
        /// <summary>Текущая анимация объекта</summary>
        public Animation Animation;

        /// <summary>Объект которому принадлежит это состояние</summary>
        protected GameObject GameObject;

        /// <summary>Время в тиках прошедшее после последнего действия</summary>
        protected int ElapsedTime = 0;

        //Конструктор
        public State(Animation animation)
        {
            Animation = animation;
        }

        /// <summary>Конструктор</summary>
        /// <param name="gameObject">кому принадлежит это состояние</param>
        public State(GameObject gameObject)
        {
            GameObject = gameObject;
        }

       
        public virtual void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);
        }

    }


    /// <summary>Обобщенный игровой объект</summary>
    public class GameObject
    {
        /// <summary>Ссылка на все объекты на карте</summary>
        protected List<GameObject> GameObjects;

        ///<summary>Мировые координаты.</summary>
        public int PosWorldX;

        ///<summary>Мировые координаты.</summary>
        public int PosWorldY;

        /// <summary>Жив ли объект</summary>
        public bool isAlive;

        /// <summary>Хранит все состояния объекта</summary>
        protected Dictionary<System.Enum, State> ObjectStates = new Dictionary<System.Enum, State>();

        /// <summary>Текущее состояние объекта</summary>
        protected State State;

        /// <summary>Текущего состояние конечного автомата</summary>
        protected System.Enum SMstate;

        /// <summary>Таблица переходов конечного автомата</summary>
        protected Dictionary<System.Enum, Dictionary<System.Enum, System.Enum>> SMtransition = new Dictionary<Enum, Dictionary<Enum, Enum>>();

        /// <summary>Текущее изображение объекта (координаты в текстуре)</summary>
        public Rectangle Sprite { get { return State.Animation.SpritePos; } }

        /// <summary>Проходимо ли поле </summary>
        public bool isPassability=false;

        public virtual void Update(GameTime gameTime)
        {
            State.Update(gameTime);
        }

        //Конструктор
        public GameObject(int x,int y)
        {
            PosWorldX = x;
            PosWorldY = y;
            isAlive = true;
            
        }

        /// <summary>Сменить состояние объекта</summary>
        public void ChangeState(System.Enum state)
        {
            State = ObjectStates[state];
            SMstate = state;
        }

        /// <summary>Переход в новое состояние конечного автомата</summary>
        public void SMrequest(System.Enum newstate)
        {
            ChangeState(SMtransition[SMstate][newstate]);
        }
        
    }

   
}
