using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberMan.GameObj
{
    /// <summary>Состояние игровых объектов </summary>
    public enum ObjectState
    {
        /// <summary>Стоит в ожидании </summary>
        Idle,
        /// <summary>Стоит развернут в вниз</summary>
        //IdleDown = Idle,

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

        /// <summary>Стена осыпается с лева</summary>
        WallDestroyLeft,
        /// <summary>Стена осыпается с права</summary>
        WallDestroyRight,
        /// <summary>Стена осыпается с верху</summary>
        WallDestroyUp,
        /// <summary>Стена осыпается с низу</summary>
        WallDestroyDown,

    }


    public class GameObject
    {
        ///<summary>Мировые координаты.</summary>
        public int PosWorldX;

        ///<summary>Мировые координаты.</summary>
        public int PosWorldY;

        /// <summary>Жив ли объект</summary>
        public bool isAlive;

        /// <summary>Список всех анимаций объекта</summary>
        //protected List<Animation> AnimationList=new List<Animation>();

        /// <summary>Хранит все анимации объекта</summary>
        protected Dictionary<ObjectState, Animation> AnimationStates = new Dictionary<ObjectState, Animation>();

        /// <summary>Текущая анимация объекта</summary>
        protected Animation Animation;

        /// <summary>Текущее изображение объекта (координаты в текстуре)</summary>
        public Rectangle Sprite { get { return Animation.SpritePos; } }

        public virtual void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);
        }

        
        //Конструктор
        public GameObject(int x,int y)
        {
            PosWorldX = x;
            PosWorldY = y;
            isAlive = true;
        }

        /// <summary>Сменить состояние объекта</summary>
        public void ChangeState(ObjectState state)
        {
            Animation = AnimationStates[state];
        }
        
    }
}
