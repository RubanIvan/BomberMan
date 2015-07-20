using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberMan
{
    class StoneWall : GameObject
    {
        public StoneWall(int x, int y,CPlayer p)
            : base(x, y,p)
        {

            Zorder = Zorders.StoneWall;
            ObjectStates.Add(WallEnum.Idle, new State(new Animation(new List<Rectangle>() { new Rectangle(2 * 48, 0, 48, 48) })));
            ChangeState(WallEnum.Idle);
        }
    }


    class SteelWall : GameObject
    {
        public SteelWall(int x, int y, CPlayer p)
            : base(x, y,p)
        {
            Zorder = Zorders.SteelWall;
            ObjectStates.Add(WallEnum.Idle, new State(new Animation(new List<Rectangle>() { new Rectangle(3 * 48, 0, 48, 48) })));
            ChangeState(WallEnum.Idle);
        }
    }

    class EmptyLand : GameObject
    {
        public EmptyLand(int x, int y, CPlayer p)
            : base(x, y,p)
        {
            isPassability = true;

            Zorder = Zorders.EmptyLand;
            ObjectStates.Add(WallEnum.Idle, new State(new Animation(new List<Rectangle>() { new Rectangle(4 * 48, 0, 48, 48) })));
            ChangeState(WallEnum.Idle);
        }
    }
}
