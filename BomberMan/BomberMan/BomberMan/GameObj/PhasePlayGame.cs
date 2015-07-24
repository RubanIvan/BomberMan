using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private CPlayer Player;

        //текущий загруженный уровень
        private int Level = 1;

        //точка с которой стартует игрок
        private Point StartPoint;

        private VievCam VievCam;

        private UIelemrnt UIelement;

        public bool GoToNextLevel = false;

        public PhasePlayGame(Texture2D texture, SpriteBatch spriteBatch, SpriteFont font)
            : base(texture, spriteBatch, font)
        {
            UIelement = new UIelemrnt(texture, spriteBatch, font);
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

            if (GoToNextLevel)
            {
                GoToNextLevel = false;
                Level++;
                if (Level > 6) Level = 1;
                GameObjects.Clear();
                Point LevSize = LoadLevel(Level);
                GameObjects.Add(Player);
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

            UIelement.Draw(Player);

        }

        ///<summary>Загрузка уровня</summary>
        private Point LoadLevel(int level)
        {
            TextReader tx = new StreamReader(File.OpenRead(String.Format(".\\Levels\\level{0}.txt", level.ToString())));
            string s;

            //Размер уровня
            int levelDx = 0;
            int levelDy = 0;

            for (int j = 0; (s = tx.ReadLine()) != "!"; j++)
            {
                if (levelDy < j) levelDy = j;
                if (levelDx < s.Length) levelDx = s.Length;

                //случайная расстановка
                if (s[0] == ':')
                {
                    GameObjects.Sort();
                    int start_brick = GameObjects.FindIndex(o => { if (o is BrickWall) return true; return false; });
                    int last_brick = GameObjects.FindLastIndex(o => { if (o is BrickWall) return true; return false; });

                    int start_epty = GameObjects.FindIndex(o => { if (o is EmptyLand) return true; return false; });
                    int last_epty = GameObjects.FindLastIndex(o => { if (o is EmptyLand) return true; return false; });

                    for (int i = 1; i < s.Length; i++)
                    {
                        int r = Rnd.Next(start_brick, last_brick);
                        int e = Rnd.Next(start_epty, last_epty);
                        
                        switch (s[i])
                        {
                            case 'L': //Life
                                GameObjects.Add(new ItemLife(GameObjects[r].PosWorldX, GameObjects[r].PosWorldY,
                                    GameObjects, Player));
                                break;
                            case 'P': //BombPower
                                GameObjects.Add(new ItemBombPower(GameObjects[r].PosWorldX, GameObjects[r].PosWorldY,
                                    GameObjects, Player));
                                break;
                            case 'Q': //BombQuantity
                                GameObjects.Add(new ItemBombQuantity(GameObjects[r].PosWorldX, GameObjects[r].PosWorldY,
                                    GameObjects, Player));
                                break;
                            case 'T': //BombReloadTime
                                GameObjects.Add(new ItemBombReloadTime(GameObjects[r].PosWorldX,
                                    GameObjects[r].PosWorldY, GameObjects, Player));
                                break;
                            case 'E': //Exit to next level
                                GameObjects.Add(new ItemExit(GameObjects[r].PosWorldX, GameObjects[r].PosWorldY,
                                    GameObjects, Player, this));
                                break;
                            case 'Z'://Zomby
                                GameObjects.Add(new Zomby(GameObjects[e].PosWorldX, GameObjects[e].PosWorldY,
                                    GameObjects, Player));
                                break;
                        }
                    }
                    levelDy--;

                    tx.Close();
                    //Пересортировать в соответсвии с Zorder
                    GameObjects.Sort();
                    return new Point(levelDx, levelDy + 1);

                }

                //ручная расстановка
                for (int i = 0; i < s.Length; i++)
                {

                    switch (s[i])
                    {
                        case 'S'://Player start point
                            GameObjects.Add(new EmptyLand(48 * i, 48 * j, Player));
                            Player.PosWorldX = 48 * i;
                            Player.PosWorldY = 48 * j;
                            Player.StartPos.X = 48 * i;
                            Player.StartPos.Y = 48 * j;
                            //GameObjects.Add(new Player(48 * i, 48 * j,GameObjects));
                            break;
                        case 'Z'://Zomby
                            GameObjects.Add(new EmptyLand(48 * i, 48 * j, Player));
                            GameObjects.Add(new Zomby(48 * i, 48 * j, GameObjects, Player));
                            break;
                        case 'L'://Life
                            GameObjects.Add(new EmptyLand(48 * i, 48 * j, Player));
                            GameObjects.Add(new BrickWall(48 * i, 48 * j, Player));
                            GameObjects.Add(new ItemLife(48 * i, 48 * j, GameObjects, Player));
                            break;
                        case 'P'://BombPower
                            GameObjects.Add(new EmptyLand(48 * i, 48 * j, Player));
                            GameObjects.Add(new BrickWall(48 * i, 48 * j, Player));
                            GameObjects.Add(new ItemBombPower(48 * i, 48 * j, GameObjects, Player));
                            break;
                        case 'Q'://BombQuantity
                            GameObjects.Add(new EmptyLand(48 * i, 48 * j, Player));
                            GameObjects.Add(new BrickWall(48 * i, 48 * j, Player));
                            GameObjects.Add(new ItemBombQuantity(48 * i, 48 * j, GameObjects, Player));
                            break;
                        case 'T': //BombReloadTime
                            GameObjects.Add(new EmptyLand(48 * i, 48 * j, Player));
                            GameObjects.Add(new BrickWall(48 * i, 48 * j, Player));
                            GameObjects.Add(new ItemBombReloadTime(48 * i, 48 * j, GameObjects, Player));
                            break;
                        case 'E': //Exit to next level
                            GameObjects.Add(new EmptyLand(48 * i, 48 * j, Player));
                            GameObjects.Add(new BrickWall(48 * i, 48 * j, Player));
                            GameObjects.Add(new ItemExit(48 * i, 48 * j, GameObjects, Player, this));
                            break;
                        case 'X'://SteelWall
                            GameObjects.Add(new SteelWall(48 * i, 48 * j, Player));
                            break;
                        case '#'://StoneWall
                            GameObjects.Add(new StoneWall(48 * i, 48 * j, Player));
                            break;
                        case ' ': //EmptyLand
                            GameObjects.Add(new EmptyLand(48 * i, 48 * j, Player));
                            break;
                        case '.'://BrickWall
                            if (Rnd.Next(100) > 60)
                            {
                                //подкладываем землю под каждую стену
                                GameObjects.Add(new EmptyLand(48 * i, 48 * j, Player));
                                GameObjects.Add(new BrickWall(48 * i, 48 * j, Player));
                            }
                            else GameObjects.Add(new EmptyLand(48 * i, 48 * j, Player));
                            break;

                    }
                }
            }

            tx.Close();

            //Пересортировать в соответсвии с Zorder
            GameObjects.Sort();

            return new Point(levelDx, levelDy + 1);

        }

        public override void Reset()
        {
            base.Reset();
            //Player = null;
            Player = new CPlayer(0, 0, GameObjects);
            GameObjects.Clear();
            GameObjects.Add(Player);

            Point LevSize = LoadLevel(1);

            VievCam = new VievCam(LevSize.X * 48, LevSize.Y * 48, Player);
        }

        public override void onLostFocus()
        {
            HiScore.Score = Player.Score;
        }
    }


    public class UIelemrnt
    {

        private Texture2D Texture;
        private SpriteBatch SpriteBatch;
        private SpriteFont Font;

        Vector2 LifePos = new Vector2(10, 10);
        Vector2 ScorePos = new Vector2(150 + 150, 10);
        private Rectangle BombSrc = new Rectangle(0, 25 * 48, 48, 48);

        public UIelemrnt(Texture2D textere, SpriteBatch spriteBatch, SpriteFont font)
        {
            Texture = textere;
            SpriteBatch = spriteBatch;
            Font = font;

        }

        public void Draw(CPlayer Player)
        {
            SpriteBatch.DrawString(Font, "Life: " + Player.Lives, LifePos, Color.Azure);
            SpriteBatch.DrawString(Font, "Score: " + Player.Score.ToString("D5"), ScorePos, Color.Azure);

            //Отображаем все целые бомбы
            for (int i = 0; i < Player.BombCount; i++)
            {
                SpriteBatch.Draw(Texture, new Rectangle(120 + 24 * i, -7, 48, 48), BombSrc, Color.White);
            }

            //если есть бомбы на перезарядке то отображаем процент перезарядки
            if (Player.BombCount != Player.MaxBombCount)
            {
                int persent = (int)(24 * ((Player.BombTime * 1.0f) / Player.BombTimeReload));
                SpriteBatch.Draw(Texture, new Rectangle(120 + 24 * Player.BombCount, -7, 48, 24 + persent), new Rectangle(0, 25 * 48, 48, 24 + persent), Color.White);

            }
        }
    }

}
