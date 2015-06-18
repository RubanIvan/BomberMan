using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberMan
{
    /// <summary>Отвечает за анимацию объекта</summary>
    public class Animation
    {
        /// <summary>Список координат кадров анимации. координаты кадров в текстуре</summary>
        private List<Rectangle> SpriteList;

        /// <summary>Событие вызываемое по завершении не круговой анимации</summary>
        public event Action AnimationEnd;

        /// <summary>Координаты спрайта</summary>
        public Rectangle SpritePos { get { return SpriteList[SpriteCurentFrameNum]; } }

        /// <summary>Есть ли анимация</summary> 
        private bool isAnimated;

        /// <summary>Круговая анимация ?</summary>
        private bool isLoop;
        
        /// <summary>Время показа одного кадра в милисекундах</summary>
        private int SpriteTimeToFrame;

        /// <summary>Сколько прошло времени после последней смены кадра</summary>
        private int SpriteElapsedTime;

        
        private int _spriteCurentFrame;

        /// <summary>Текущий номер кадра анимации</summary>
        public int SpriteCurentFrameNum
        {
            get { return _spriteCurentFrame; }
            set
            {
                //если запрошен кадр больше чем есть и есть повтор по кругу
                if (value >= SpriteList.Count - 1)
                {
                    if (isLoop) _spriteCurentFrame = 0;
                    else _spriteCurentFrame = SpriteList.Count - 1;
                    return;
                }

                if (value < 0)
                {
                    if (isLoop) _spriteCurentFrame = SpriteList.Count - 1;
                    else _spriteCurentFrame = 0;
                    return;
                }


                _spriteCurentFrame = value;

            }
        }


        //Конструктор
        public Animation(List<Rectangle> spriteList, int timeToFrameMilliseconds = 0, 
                         bool isAnimated = false, bool isLoop = false,Action AnimationEnd=null)
        {
            this.SpriteTimeToFrame = timeToFrameMilliseconds;
            this.SpriteList = spriteList;
            this.isAnimated = isAnimated;
            this.isLoop = isLoop;
            this.AnimationEnd += AnimationEnd;
        }

        /// <summary>Анимировать спрайт </summary>
        public void Update(GameTime gameTime)
        {
            if (isAnimated)
            {
                SpriteElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                if (SpriteElapsedTime > SpriteTimeToFrame)
                {
                    SpriteCurentFrameNum++;
                    SpriteElapsedTime = 0;

                    //это последний кадр некруговой анимации ?
                    if (!isLoop && SpriteCurentFrameNum == SpriteList.Count - 1)
                    {
                        isAnimated = false;
                        //запускаем событие
                        if (AnimationEnd != null) AnimationEnd();
                    }
                    
                    
                }
            }
        }

    }

}
