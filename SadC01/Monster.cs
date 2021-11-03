using System;
using System.Linq;
using System.Collections.Generic;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadConsole.Themes;

namespace Arcade
{
    public abstract class Monster
    {
        public char actualGlyph{ get; set; }
        public virtual int monsterX { get; set; }
        public virtual int monsterY { get; set; }
        public virtual int plusScore { get; set; }
        

        public virtual void moveOneStep()
        {
        }

        public  virtual void phaseChange(byte b)
        {
        }

        public int iranyMon { get; set; }
        //! észak: E , kelet: K, dél:D , nyugat: N
        public virtual char[,]  moveOneStep  (char[,] grid)
        {
            return grid;
        }
        public virtual int whereToShoot() {
            return 10;
        }
    }
}