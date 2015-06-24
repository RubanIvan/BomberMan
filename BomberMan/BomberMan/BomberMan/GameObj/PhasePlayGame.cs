using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberMan.GameObj
{
    class PhasePlayGame : GamePhaseObject
    {
        /// <summary>ссылка на игрока </summary>
        public Player Player;

        public PhasePlayGame(Texture2D texture, SpriteBatch spriteBatch): base(texture, spriteBatch)
        {
            //GameObjects.Add(new StoneWall(0, 0));
            //GameObjects.Add(new Player(48, 48));
            //GameObjects.Add(new BrickWall(48, 48 * 2));
            LoadLevel(1);
            FindPlayer();

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

        }

        public override void Draw()
        {
            foreach (GameObject O in GameObjects)
            {
                //Проверяем пересечение прямоугольников 
                if (O.PosWorldX + 48 < VievCam.X || O.PosWorldX > VievCam.X + VievCam.DX || O.PosWorldY + 48 < VievCam.Y || O.PosWorldY > VievCam.Y + VievCam.DY) continue;

                //пересчитываем координаты из мировых в экранные для данного объекта и отрисовываем
                SpriteBatch.Draw(Texture, new Rectangle(O.PosWorldX - VievCam.X, O.PosWorldY - VievCam.Y, 48, 48), O.Sprite, Color.White);
            }
        }

        ///<summary>Загрузка уровня</summary>
        private void LoadLevel(int level)
        {
            GameObjects.Clear();
            TextReader tx = new StreamReader(File.OpenRead(String.Format(".\\Levels\\level{0}.txt", level.ToString())));
            string s;

            //while ((s=tx.ReadLine()) !="!" )

            for (int j = 0; (s = tx.ReadLine()) != "!"; j++)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    switch (s[i])
                    {
                        case 'S':
                            GameObjects.Add(new EmptyLand(48 * i, 48 * j));
                            GameObjects.Add(new Player(48 * i, 48 * j,GameObjects));
                            break;
                        case 'X':
                            GameObjects.Add(new EmptyLand(48 * i, 48 * j));
                            GameObjects.Add(new SteelWall(48 * i, 48 * j));
                            break;
                        case '#':
                            GameObjects.Add(new EmptyLand(48 * i, 48 * j));
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

        }

        /// <summary>Найти игрока среди объектов и установить на него ссылку (после загрузки уровня)</summary>
        private void FindPlayer()
        {
            foreach (GameObject O in GameObjects)
                if (O is Player)
                {
                    Player = (Player)O;
                    GameObjects.Remove(Player);
                    GameObjects.Add(Player);
                    break;
                }


        }

    }
}
