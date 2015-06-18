using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberMan.GameObj
{

    class BrickWall:GameObject
    {
        //конструктор
        public BrickWall(int x, int y) : base(x, y)
        {
            AnimationStates.Add(ObjectState.Idle, new Animation(new List<Rectangle>() { new Rectangle(1 * 48, 0 * 48, 48, 48) }));

            AnimationStates.Add(ObjectState.WallDestroyRight, new Animation(new List<Rectangle>()
            {
                new Rectangle(0 * 48, 1 * 48, 48, 48),new Rectangle(1 * 48, 1 * 48, 48, 48),new Rectangle(2 * 48, 1 * 48, 48, 48),
                new Rectangle(3 * 48, 1 * 48, 48, 48),new Rectangle(4 * 48, 1 * 48, 48, 48),new Rectangle(5 * 48, 1 * 48, 48, 48),
                new Rectangle(6 * 48, 1 * 48, 48, 48),new Rectangle(7 * 48, 1 * 48, 48, 48),new Rectangle(8 * 48, 1 * 48, 48, 48),
                new Rectangle(9 * 48, 1 * 48, 48, 48),new Rectangle(0 * 48, 0 * 48, 48, 48)

            }, 100, true, false, Destroy));


            AnimationStates.Add(ObjectState.WallDestroyLeft, new Animation(new List<Rectangle>()
            {
                new Rectangle(0 * 48, 2 * 48, 48, 48),new Rectangle(1 * 48, 2 * 48, 48, 48),new Rectangle(2 * 48, 2 * 48, 48, 48),
                new Rectangle(3 * 48, 2 * 48, 48, 48),new Rectangle(4 * 48, 2 * 48, 48, 48),new Rectangle(5 * 48, 2 * 48, 48, 48),
                new Rectangle(6 * 48, 2 * 48, 48, 48),new Rectangle(7 * 48, 2 * 48, 48, 48),new Rectangle(8 * 48, 2 * 48, 48, 48),
                new Rectangle(9 * 48, 2 * 48, 48, 48),new Rectangle(0 * 48, 0 * 48, 48, 48)

            }, 100, true, false, Destroy));


            AnimationStates.Add(ObjectState.WallDestroyDown, new Animation(new List<Rectangle>()
            {
                new Rectangle(0 * 48, 3 * 48, 48, 48),new Rectangle(1 * 48, 3 * 48, 48, 48),new Rectangle(2 * 48, 3 * 48, 48, 48),
                new Rectangle(3 * 48, 3 * 48, 48, 48),new Rectangle(4 * 48, 3 * 48, 48, 48),new Rectangle(5 * 48, 3 * 48, 48, 48),
                new Rectangle(6 * 48, 3 * 48, 48, 48),new Rectangle(7 * 48, 3 * 48, 48, 48),new Rectangle(8 * 48, 3 * 48, 48, 48),
                new Rectangle(9 * 48, 3 * 48, 48, 48),new Rectangle(0 * 48, 0 * 48, 48, 48)

            }, 100, true, false, Destroy));


            AnimationStates.Add(ObjectState.WallDestroyUp, new Animation(new List<Rectangle>()
            {
                new Rectangle(0 * 48, 4 * 48, 48, 48),new Rectangle(1 * 48, 4 * 48, 48, 48),new Rectangle(2 * 48, 4 * 48, 48, 48),
                new Rectangle(3 * 48, 4 * 48, 48, 48),new Rectangle(4 * 48, 4 * 48, 48, 48),new Rectangle(5 * 48, 4 * 48, 48, 48),
                new Rectangle(6 * 48, 4 * 48, 48, 48),new Rectangle(7 * 48, 4 * 48, 48, 48),new Rectangle(8 * 48, 4 * 48, 48, 48),
                new Rectangle(9 * 48, 4 * 48, 48, 48),new Rectangle(0 * 48, 0 * 48, 48, 48)

            }, 100, true, false, Destroy));

            ChangeState(ObjectState.Idle);
        }

        /// <summary>Функция обратного вызова вызывается при завершении анимации разрушения стены</summary>
        public void Destroy()
        {
            isAlive = false;
        }

    }
}
