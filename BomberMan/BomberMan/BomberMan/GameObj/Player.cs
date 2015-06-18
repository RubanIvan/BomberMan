using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberMan.GameObj
{

    class Player : GameObject
    {

        private int ElapsedTime=0;

        public Player(int x, int y)
            : base(x, y)
        {

            #region Задание анимациий
            AnimationStates.Add(ObjectState.Idle, new Animation(new List<Rectangle>() { new Rectangle(0 * 48, 11 * 48, 48, 48) }));

            AnimationStates.Add(ObjectState.WalkUp, new Animation(new List<Rectangle>()
            {
                new Rectangle(1 * 48, 9 * 48, 48, 48),new Rectangle(2 * 48, 9 * 48, 48, 48),
                new Rectangle(3 * 48, 9 * 48, 48, 48),new Rectangle(4 * 48, 9 * 48, 48, 48),new Rectangle(5 * 48, 9 * 48, 48, 48),
                new Rectangle(6 * 48, 9 * 48, 48, 48),new Rectangle(7 * 48, 9 * 48, 48, 48),new Rectangle(8 * 48, 9 * 48, 48, 48)
            }, 80, true, true));
            AnimationStates.Add(ObjectState.IdleUp, new Animation(new List<Rectangle>() { new Rectangle(0 * 48, 9 * 48, 48, 48) }));

            AnimationStates.Add(ObjectState.WalkLeft, new Animation(new List<Rectangle>()
            {
                new Rectangle(1 * 48, 10 * 48, 48, 48),new Rectangle(2 * 48, 10 * 48, 48, 48),
                new Rectangle(3 * 48, 10 * 48, 48, 48),new Rectangle(4 * 48, 10 * 48, 48, 48),new Rectangle(5 * 48, 10 * 48, 48, 48),
                new Rectangle(6 * 48, 10 * 48, 48, 48),new Rectangle(7 * 48, 10 * 48, 48, 48),new Rectangle(8 * 48, 10 * 48, 48, 48)
            }, 80, true, true));
            AnimationStates.Add(ObjectState.IdleLeft, new Animation(new List<Rectangle>() { new Rectangle(0 * 48, 10 * 48, 48, 48) }));


            AnimationStates.Add(ObjectState.WalkRight, new Animation(new List<Rectangle>()
            {
                new Rectangle(1 * 48, 12 * 48, 48, 48),new Rectangle(2 * 48, 12 * 48, 48, 48),
                new Rectangle(3 * 48, 12 * 48, 48, 48),new Rectangle(4 * 48, 12 * 48, 48, 48),new Rectangle(5 * 48, 12 * 48, 48, 48),
                new Rectangle(6 * 48, 12 * 48, 48, 48),new Rectangle(7 * 48, 12 * 48, 48, 48),new Rectangle(8 * 48, 12 * 48, 48, 48)
            }, 80, true, true));
            AnimationStates.Add(ObjectState.IdleRight, new Animation(new List<Rectangle>() { new Rectangle(0 * 48, 12 * 48, 48, 48) }));

            AnimationStates.Add(ObjectState.WalkDown, new Animation(new List<Rectangle>()
            {
                new Rectangle(1 * 48, 11 * 48, 48, 48),new Rectangle(2 * 48, 11 * 48, 48, 48),
                new Rectangle(3 * 48, 11 * 48, 48, 48),new Rectangle(4 * 48, 11 * 48, 48, 48),new Rectangle(5 * 48, 11 * 48, 48, 48),
                new Rectangle(6 * 48, 11 * 48, 48, 48),new Rectangle(7 * 48, 11 * 48, 48, 48),new Rectangle(8 * 48, 11 * 48, 48, 48)
            }, 80, true, true));


            AnimationStates.Add(ObjectState.Fire, new Animation(new List<Rectangle>()
            {
                new Rectangle(0 * 48, 30 * 48, 48, 48),new Rectangle(1 * 48, 30 * 48, 48, 48),new Rectangle(2 * 48, 30 * 48, 48, 48),
                new Rectangle(3 * 48, 30 * 48, 48, 48),new Rectangle(4 * 48, 30 * 48, 48, 48),new Rectangle(5 * 48, 30 * 48, 48, 48),
                new Rectangle(6 * 48, 30 * 48, 48, 48),new Rectangle(7 * 48, 30 * 48, 48, 48),new Rectangle(8 * 48, 30 * 48, 48, 48),
                new Rectangle(9 * 48, 30 * 48, 48, 48),new Rectangle(10 * 48, 30 * 48, 48, 48),new Rectangle(11 * 48, 30 * 48, 48, 48),
                new Rectangle(12 * 48, 30 * 48, 48, 48),new Rectangle(13 * 48, 30 * 48, 48, 48),new Rectangle(14 * 48, 30 * 48, 48, 48),
                new Rectangle(15 * 48, 30 * 48, 48, 48),new Rectangle(16 * 48, 30 * 48, 48, 48),new Rectangle(17 * 48, 30 * 48, 48, 48),
                new Rectangle(18 * 48, 30 * 48, 48, 48),new Rectangle(19 * 48, 30 * 48, 48, 48),
                new Rectangle(0 * 48, 31 * 48, 48, 48),new Rectangle(1 * 48, 31 * 48, 48, 48),
            }, 80, true, false));

             
            #endregion

            ChangeState(ObjectState.WalkDown);

        }

        public void MoveRight(GameTime gameTime)
        {
            ChangeState(ObjectState.WalkRight);
            //Debug.WriteLine(PosWorldX);
            PosWorldX += 1;
            //Debug.WriteLine(PosWorldX);
        }

    }
}
