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
        MenuChange=1

    }

    public class SoundEngine
    {
        public List<SoundEffect> SoundList=new List<SoundEffect>();

        public void SoundInit(ContentManager Content)
        {
            SoundList.Add(Content.Load<SoundEffect>("MenuSelect"));
            SoundList.Add(Content.Load<SoundEffect>("MenuChange"));
            
        }

        public SoundEffect this[SoundNames s]
        {
            get { return SoundList[(int) s]; }
        }
    }
}
