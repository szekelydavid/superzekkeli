using System;
using Microsoft.Xna.Framework.Media;
using System.Linq;

namespace Arcade
{
    public class Mosquito : Monster
    {
        public char actualGlyph = 'V';
        private char actualField;
        public int mosquitoX { get; set; }
        public int mosquitoY { get; set; }
        public override int monsterX { get; set; }
        public override int monsterY { get; set; }

        public override int plusScore { get; set; }

        private char[,] mozgasMinta = new char[9, 10]
        {
            {'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D'},
            {'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D'},
            {'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'D'},
            {'D', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N'},
            {'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'D'},
            {'D', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N'},
            {'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'D'},
            {'D', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N'},
            {'X', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D'}

        };

        public Mosquito(int bex, int bey)
        {
            this.monsterX = bex;
            this.monsterY = bey;
            mosquitoX = bex;
            mosquitoY = bey;
            plusScore = 10;

        }

        public override void phaseChange(byte inb)
        {
            if (inb == 0)
            {
                this.actualGlyph = 'F';
            }
            if (inb == 4)
            {
                this.actualGlyph = 'V';
            }

        }

        public override char[,] moveOneStep(char[,] grid)
        {

            actualField = mozgasMinta[this.mosquitoY, this.mosquitoX];
            //GameLoop._mapscreen.Print(1, 1, "F");

            int coorXmoveTo = 0;
            int coorYmoveTo = 0;

            if (actualField == 'E')
            {
                coorXmoveTo = mosquitoX;
                coorYmoveTo = mosquitoY - 1;
            }
            if (actualField == 'K')
            {
                coorXmoveTo = mosquitoX + 1;
                coorYmoveTo = mosquitoY;
            }
            if (actualField == 'D')
            {
                coorXmoveTo = mosquitoX;
                coorYmoveTo = mosquitoY + 1;
            }
            if (actualField == 'N')
            {
                coorXmoveTo = mosquitoX - 1;
                coorYmoveTo = mosquitoY;
            }

            if (actualField == 'X')
            {
                coorXmoveTo = mosquitoX;
                coorYmoveTo = mosquitoY;

            }

            if (actualField == 'X')
            {
                grid[mosquitoX, mosquitoY] = '?';
            }
            else
            {
                string enableToMove = "zjZ0hX";
                if (enableToMove.Contains(grid[coorXmoveTo, coorYmoveTo]))
                {
                    grid[mosquitoX, mosquitoY] = '0';
                    grid[coorXmoveTo, coorYmoveTo] = actualGlyph;


                    this.mosquitoX = coorXmoveTo;
                    this.mosquitoY = coorYmoveTo;

                    monsterX = mosquitoX;
                    monsterY = mosquitoY;
                }
            }
            return grid;

        }
        public override int whereToShoot()
        {
            return 2;
        }
    }
}