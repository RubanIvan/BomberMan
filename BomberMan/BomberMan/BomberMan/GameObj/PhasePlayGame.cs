using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using BomberMan.GameObj;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberMan
{
    class PhasePlayGame : GamePhaseObject
    {
        /// <summary>ссылка на игрока </summary>
        private Player Player;

        private VievCam VievCam;


        public PhasePlayGame(Texture2D texture, SpriteBatch spriteBatch,SpriteFont font): base(texture, spriteBatch,font)
        {
            
            Point LevSize=LoadLevel(1);
            FindPlayer();
            VievCam=new VievCam(LevSize.X*48,LevSize.Y*48,Player);

            //GameObjects.Add(new Zomby(48, 48 ,GameObjects,Player));


        }

        
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < GameObjects.Count; i++)
            {
                //Обновляем все объекты
                GameObjects[i].Update(gameTime);

                //Удаляем мертвые 
                if (!GameObjects[i].isAlive) GameObjects.RemoveAt(i);

                
            }

            VievCam.Update(gameTime);

        }

        public override void Draw()
        {
            foreach (GameObject O in GameObjects)
            {
                if (O is Player)
                {
                    SpriteBatch.DrawString(Font, "Life: "+((Player)O).Lives, new Vector2(10, 10), Color.Azure);
                    SpriteBatch.DrawString(Font, "Score: " + ((Player)O).Score.ToString("D5"), new Vector2(150, 10), Color.Azure);
                }

                //Проверяем пересечение прямоугольников 
                if (O.PosWorldX + 48 < VievCam.X || O.PosWorldX > VievCam.X + VievCam.DX || O.PosWorldY + 48 < VievCam.Y || O.PosWorldY > VievCam.Y + VievCam.DY) continue;

                //пересчитываем координаты из мировых в экранные для данного объекта и отрисовываем
                SpriteBatch.Draw(Texture, new Rectangle(O.PosWorldX - VievCam.X, O.PosWorldY - VievCam.Y, 48, 48), O.Sprite, Color.White);
            }
        }

        ///<summary>Загрузка уровня</summary>
        private Point LoadLevel(int level)
        {
            GameObjects.Clear();
            TextReader tx = new StreamReader(File.OpenRead(String.Format(".\\Levels\\level{0}.txt", level.ToString())));
            string s;

            //Размер уровня
            int levelDx = 0;
            int levelDy=0;

            for (int j = 0; (s = tx.ReadLine()) != "!"; j++)
            {
                if (levelDy < j) levelDy = j;
                if (levelDx < s.Length) levelDx = s.Length;

                for (int i = 0; i < s.Length; i++)
                {

                    switch (s[i])
                    {
                        case 'S':
                            GameObjects.Add(new EmptyLand(48 * i, 48 * j));
                            GameObjects.Add(new Player(48 * i, 48 * j,GameObjects));
                            break;
                        case 'Z':
                            GameObjects.Add(new EmptyLand(48 * i, 48 * j));
                            GameObjects.Add(new Zomby(48 * i, 48 * j, GameObjects));
                            break;
                        case 'X':
                            GameObjects.Add(new SteelWall(48 * i, 48 * j));
                            break;
                        case '#':
                            GameObjects.Add(new StoneWall(48 * i, 48 * j));
                            break;
                        case ' ':
                            GameObjects.Add(new EmptyLand(48 * i, 48 * j));
                            break;
                        case '.':
                            if (Rnd.Next(100) > 60)
                            {
                                //подкладываем землю под каждую стену
                                GameObjects.Add(new EmptyLand(48 * i, 48 * j));
                                GameObjects.Add(new BrickWall(48 * i, 48 * j));
                            }
                            else GameObjects.Add(new EmptyLand(48 * i, 48 * j));
                            break;

                    }
                }
            }

            tx.Close();

            //Пересортировать в соответсвии с Zorder
            GameObjects.Sort();

            return new Point(levelDx,levelDy+1);

        }

        /// <summary>Найти игрока среди объектов и установить на него ссылку (после загрузки уровня)</summary>
        private void FindPlayer()
        {
            foreach (GameObject O in GameObjects)
                if (O is Player)
                {
                    Player = (Player)O;
                    //GameObjects.Remove(Player);
                    //GameObjects.Add(Player);
                    break;
                }


        }

        public override void Reset()
        {
            base.Reset();
            Player = null;
            GameObjects.Clear();

            Point LevSize = LoadLevel(1);
            FindPlayer();
            VievCam = new VievCam(LevSize.X * 48, LevSize.Y * 48, Player);
        }
    }
}
