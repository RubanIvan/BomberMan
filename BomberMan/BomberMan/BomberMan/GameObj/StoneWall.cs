using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberMan.GameObj
{
    class StoneWall:GameObject
    {
        public StoneWall(int x,int y):base(x,y)
        {
            AnimationStates.Add(ObjectState.Idle , new Animation(new List<Rectangle>() { new Rectangle(2 * 48, 0, 48, 48) }));
            ChangeState(ObjectState.Idle);
        }
    }


    class SteelWall : GameObject
    {
        public SteelWall(int x, int y)
            : base(x, y)
        {
            AnimationStates.Add(ObjectState.Idle, new Animation(new List<Rectangle>() { new Rectangle(3 * 48, 0, 48, 48) }));
            ChangeState(ObjectState.Idle);
        }
    }

    class EmptyLand : GameObject
    {
        public EmptyLand(int x, int y)
            : base(x, y)
        {
            AnimationStates.Add(ObjectState.Idle, new Animation(new List<Rectangle>() { new Rectangle(4 * 48, 0, 48, 48) }));
            ChangeState(ObjectState.Idle);
        }
    }
}
