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
            ObjectStates.Add(WallEnum.Idle,new State(new Animation(new List<Rectangle>() { new Rectangle(2 * 48, 0, 48, 48) })));
            ChangeState(WallEnum.Idle);
        }
    }


    class SteelWall : GameObject
    {
        public SteelWall(int x, int y)
            : base(x, y)
        {
            ObjectStates.Add(WallEnum.Idle, new State(new Animation(new List<Rectangle>() { new Rectangle(3 * 48, 0, 48, 48) })));
            ChangeState(WallEnum.Idle);
            
        }
    }

    class EmptyLand : GameObject
    {
        public EmptyLand(int x, int y)
            : base(x, y)
        {
            isPassability = true;

            ObjectStates.Add(WallEnum.Idle, new State(new Animation(new List<Rectangle>() { new Rectangle(4 * 48, 0, 48, 48) })));
            ChangeState(WallEnum.Idle);

        }
    }
}
