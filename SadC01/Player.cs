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
    public class Player : SadConsole.Entities.Entity
    {
        public int pGlyph { get { return Animation.CurrentFrame[0].Glyph; } set { Animation.CurrentFrame[0].Glyph = value; Animation.IsDirty = true; } }
        Point Position { get; set; }
        public int X { get ; set; }
        public int Y { get; set; }

        public int iranyPL { get; set; }
        //! észak: 0 , kelet: 1, dél:2 , nyugat 3
        public Player() : base(Microsoft.Xna.Framework.Color.White, Microsoft.Xna.Framework.Color.Transparent, 5) {
            Animation.CurrentFrame[0].Glyph = 'd';

            iranyPL = 0;
        }
        
    }
}
