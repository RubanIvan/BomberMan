using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Configuration;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberMan
{

    public static class HiScore
    {
        public static int Score;
    }

    class PhaseNewHiScore : GamePhaseObject
    {

        /// <summary>Позиция на экране для вывода заднего фона</summary>
        Rectangle BkgTo = new Rectangle(0, 0, 800, 600);
        /// <summary>Позиция заднего фона в текстуре</summary>
        Rectangle BkgFrom = new Rectangle(16, 0, 800, 600);

        //Dictionary<int,string> HiScore=new Dictionary<int, string>();
        List<KeyValuePair<int, string>> HiScore = new List<KeyValuePair<int, string>>();
        Stream fStream;

        string FileName = "HiScore.table";

        private string Name;


        private int TimeToKey = 100;

        private int TimeToKeyElapse;

        public PhaseNewHiScore(Texture2D texture, SpriteBatch spriteBatch, SpriteFont font) : base(texture, spriteBatch, font)
        {
        }

        public override void Update(GameTime gameTime)
        {
            TimeToKeyElapse += gameTime.ElapsedGameTime.Milliseconds;
            if (TimeToKeyElapse > TimeToKey && InputHelper.CurrentKeyboardState.GetPressedKeys().Length > 0)
            {
                TimeToKeyElapse = 0;

                if (InputHelper.CurrentKeyboardState.IsKeyDown(Keys.Back))
                {
                    if (Name.Length > 1)
                    {
                        Name = Name.Remove(Name.Length - 2);
                        Name += "|";
                    }
                    return;
                }

                if (InputHelper.CurrentKeyboardState.IsKeyDown(Keys.LeftShift) ||
                    InputHelper.CurrentKeyboardState.IsKeyDown(Keys.RightShift))
                {
                    if (InputHelper.CurrentKeyboardState.GetPressedKeys()[0] >= Keys.A &&
                        InputHelper.CurrentKeyboardState.GetPressedKeys()[0] <= Keys.Z) 
                    {
                        Name = Name.Remove(Name.Length - 1);
                        Name += InputHelper.CurrentKeyboardState.GetPressedKeys()[0].ToString();
                        Name += "|";
                    }
                }else if (InputHelper.CurrentKeyboardState.GetPressedKeys()[0] >= Keys.A &&
                        InputHelper.CurrentKeyboardState.GetPressedKeys()[0] <= Keys.Z)
                    {
                        Name = Name.Remove(Name.Length - 1);
                        Name += InputHelper.CurrentKeyboardState.GetPressedKeys()[0].ToString().ToLower();
                        Name += "|";
                    }

                if (InputHelper.CurrentKeyboardState.GetPressedKeys()[0] == Keys.Space)
                {
                    Name = Name.Remove(Name.Length - 1);
                    Name += " ";
                    Name += "|";
                }

                //if (InputHelper.CurrentKeyboardState.IsKeyDown(Keys.LeftShift) ||
                //    InputHelper.CurrentKeyboardState.IsKeyDown(Keys.RightShift))
                //{
                //    Name += InputHelper.CurrentKeyboardState.GetPressedKeys()[0].ToString();
                //}
                //else
                //{
                //    Name += InputHelper.CurrentKeyboardState.GetPressedKeys()[0].ToString().ToLower();
                //}
            }

            

            if (InputHelper.KeyPressed(Keys.Enter))
            {
                Name = Name.Remove(Name.Length - 1);

                HiScore.Add(new KeyValuePair<int, string>(BomberMan.HiScore.Score,Name));

                HiScore.Sort((pair, valuePair) => { 
                    if (pair.Key > valuePair.Key) return -1;
                    else if (pair.Key < valuePair.Key) return 1;
                    else return 0;});

                HiScore.RemoveAt(10);

                try
                {

                    BinaryFormatter binFormat = new BinaryFormatter();
                    fStream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                    binFormat.Serialize(fStream, HiScore);
                }
                finally 
                {
                    fStream.Close();
                    
                }

                
                GamePhaseManager.SwitchTo(Phase.HiScore);
            }
        }

        public override void Draw()
        {
            SpriteBatch.Draw(Texture, BkgTo, BkgFrom, Color.White);

            
            SpriteBatch.DrawString(Font, string.Format("New Hi Score : {0}", BomberMan.HiScore.Score.ToString("D5")),
                                         new Vector2(300-100, 150 + 30), Color.Azure);

            SpriteBatch.DrawString(Font, string.Format("Enter your name", BomberMan.HiScore.Score.ToString("D5")),
                                         new Vector2(300-50-15, 150 + 60), Color.Azure);

            SpriteBatch.DrawString(Font, string.Format(Name, BomberMan.HiScore.Score.ToString("D5")),
                                         new Vector2(300 - 50 - 15, 150 + 90), Color.Azure);
            
        }

        public override void Reset()
        {
            BinaryFormatter binFormat = new BinaryFormatter();

            try
            {
                fStream = File.OpenRead(FileName);
                HiScore = (List<KeyValuePair<int, string>>)binFormat.Deserialize(fStream);
            }
            catch (Exception)
            {
                HiScore = new List<KeyValuePair<int, string>>();
                HiScore.Add(new KeyValuePair<int, string>(100, "Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100, "Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100, "Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100, "Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100, "Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100, "Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100, "Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100, "Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100, "Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100, "Ivan"));


                fStream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                binFormat.Serialize(fStream, HiScore);

            }
            finally
            {
                fStream.Close();
            }

            bool flag = true;

            //Проверяем попадает ли игрок в таблицу
            foreach (KeyValuePair<int, string> KeyValuePair in HiScore)
            {
                if (BomberMan.HiScore.Score > KeyValuePair.Key){flag = false;break;}
            }

            //если не попал то просто выводим таблицу рекордов
            if(flag)GamePhaseManager.SwitchTo(Phase.HiScore);

            Name = "|";


        }
    }
}