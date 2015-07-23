using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace BomberMan
{

    public enum SoundNames
    {
        MenuSelect=0,
        MenuChange=1,
        BombBlow,
        BombBurn,
        BombDrop,
        ItemPickUp,
        ZombyBurn,
        GameOverSound,
        PlayerBurn

    }

    public static class SoundEngine
    {
        public static  List<SoundEffect> SoundList=new List<SoundEffect>();

        public static void SoundInit(ContentManager Content)
        {
            SoundList.Add(Content.Load<SoundEffect>("MenuSelect"));
            SoundList.Add(Content.Load<SoundEffect>("MenuChange"));
            SoundList.Add(Content.Load<SoundEffect>("BombBlow"));
            SoundList.Add(Content.Load<SoundEffect>("BombBurn"));
            SoundList.Add(Content.Load<SoundEffect>("BombDrop"));
            SoundList.Add(Content.Load<SoundEffect>("ItemPickUp"));
            SoundList.Add(Content.Load<SoundEffect>("ZombyBurn"));
            SoundList.Add(Content.Load<SoundEffect>("GameOverSound"));
            SoundList.Add(Content.Load<SoundEffect>("PlayerBurn"));
        }

        public static SoundEffect GetEffect(SoundNames s)
        {
             return SoundList[(int) s];
        }
    }
}
