using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberMan
{
    class PhaseHiScore : GamePhaseObject
    {
        /// <summary>Позиция на экране для вывода заднего фона</summary>
        Rectangle BkgTo = new Rectangle(0, 0, 800, 600);
        /// <summary>Позиция заднего фона в текстуре</summary>
        Rectangle BkgFrom = new Rectangle(16, 0, 800, 600);

        //Dictionary<int,string> HiScore=new Dictionary<int, string>();
        List<KeyValuePair<int,string>>HiScore =new List<KeyValuePair<int, string>>();
        Stream fStream;

        string FileName = "HiScore.table";


        /// <summary>Время до переключения на экран меню</summary>
        private int TimeToShowMenu = 10000;

        private int TimeToShowMenuElapse;

        public PhaseHiScore(Texture2D texture, SpriteBatch spriteBatch, SpriteFont font)
            : base(texture, spriteBatch, font)
        {
        }

        public override void Update(GameTime gameTime)
        {
            TimeToShowMenuElapse += gameTime.ElapsedGameTime.Milliseconds;
            if (TimeToShowMenuElapse > TimeToShowMenu) GamePhaseManager.SwitchTo(Phase.MainMenu);

            if (InputHelper.KeyPressed(Keys.Space) || InputHelper.KeyPressed(Keys.Enter))
            {
                GamePhaseManager.SwitchTo(Phase.MainMenu);
            }
            
        }

        public override void Draw()
        {
            SpriteBatch.Draw(Texture, BkgTo, BkgFrom, Color.White);

            for (int i = 0; i < 10; i++)
            {
                SpriteBatch.DrawString(Font, string.Format("{0}:  {1}   {2}",i,HiScore[i].Key.ToString("D5"),HiScore[i].Value) , new Vector2(300,150+30*i), Color.Azure);
            }

        }


        public override void Reset()
        {
            TimeToShowMenuElapse = 0;
            BinaryFormatter binFormat = new BinaryFormatter();

            try
            {
                fStream = File.OpenRead(FileName);
                HiScore = (List<KeyValuePair<int, string>>)binFormat.Deserialize(fStream);
            }
            catch (Exception)
            {
                HiScore = new List<KeyValuePair<int, string>>();
                HiScore.Add(new KeyValuePair<int, string>(100,"Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100,"Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100,"Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100,"Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100,"Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100,"Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100,"Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100,"Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100,"Ivan"));
                HiScore.Add(new KeyValuePair<int, string>(100,"Ivan"));


                fStream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                binFormat.Serialize(fStream, HiScore);
                
            }
            finally
            {
               fStream.Close();
            }
            
        }
    }
}