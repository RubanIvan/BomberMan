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
        public List<SoundEffectInstance > SoundList=new List<SoundEffectInstance>();

        public void SoundInit(ContentManager Content)
        {
            SoundList.Add(Content.Load<SoundEffect>("MenuSelect").CreateInstance());
            SoundList.Add(Content.Load<SoundEffect>("MenuChange").CreateInstance());
            
        }

        public SoundEffectInstance this[SoundNames s]
        {
            get { return SoundList[(int) s]; }
        }
    }
}
