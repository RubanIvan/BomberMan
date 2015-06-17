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
            AnimationList.Add(new Animation(new List<Rectangle>() {new Rectangle(2*48, 0, 48, 48)}));
            Animation = AnimationList[0];
        }
    }
}
