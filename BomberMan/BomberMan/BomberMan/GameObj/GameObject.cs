using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberMan.GameObj
{
    public class GameObject
    {
        ///<summary>Мировые координаты.</summary>
        public int PosWorldX { get; protected set; }

        ///<summary>Мировые координаты.</summary>
        public int PosWorldY { get; protected set; }

        /// <summary>Список всех анимаций объекта</summary>
        protected List<Animation> AnimationList=new List<Animation>(); 

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
        }

        
    }
}
