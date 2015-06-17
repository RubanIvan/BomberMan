using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.GameObj
{
    class GameStatePlayGame:GameStateObject
    {
        public GameStatePlayGame(Texture2D texture, SpriteBatch spriteBatch) : base(texture, spriteBatch)
        {
            GameObjects.Add(new StoneWall(0,0));

        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameObject O in GameObjects)
            {
                O.Update(gameTime);
            }
        }

        public override void Draw()
        {
            foreach (GameObject O in GameObjects)
            {
            
                SpriteBatch.Draw(Texture,new Rectangle(0,0,48,48), O.Sprite, Color.White);
            }

            
        }
    }
}
