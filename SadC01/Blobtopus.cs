using System;
using Microsoft.Xna.Framework.Media;
using System.Linq;


namespace Arcade
{
    public class Blobtopus : Monster
    {
        public char actualGlyph = 'J';

        private char actualField;

        public int blobtopusX { get; set; }
        public int blobtopusY { get; set; }
        public override int  monsterX { get; set; }
        public override int monsterY { get; set; }

        public override int plusScore { get; set; }

        public Blobtopus(int bex, int bey)
        {
            this.monsterX = bex;
            this.monsterY = bey;
            blobtopusX = bex;
            blobtopusY = bey;
            plusScore = 15;
        }

        public override void phaseChange(byte inb)
        {
            if (inb == 4)
            {
                this.actualGlyph = 'K';
            }

            if (inb == 0)
            {
                this.actualGlyph = 'J';
            }
        }

        public override char[,] moveOneStep(char[,] grid)
        {
            Random rand = new Random();
            int irany = rand.Next(0, 4);

            //GameLoop._mapscreen.Print(1, 1, "F");

            int coorXmoveTo = 0;
            int coorYmoveTo = 0;

            if (irany == 0)
            {
                coorXmoveTo = blobtopusX;
                if (blobtopusY - 1 >= 0) { coorYmoveTo = blobtopusY - 1; }
                else coorYmoveTo = blobtopusY + 1;
            }

            if (irany == 1)
            {
                if (blobtopusX + 1 < 10) { coorXmoveTo = blobtopusX + 1; }
                else { coorXmoveTo = blobtopusX - 1; }

                coorYmoveTo = blobtopusY;
            }

            if (irany == 2)
            {
                if (blobtopusY + 1 < 9) { coorYmoveTo = blobtopusY + 1; }
                else { coorYmoveTo = blobtopusY - 1; }
                coorXmoveTo = blobtopusX;

            }

            if (irany == 3)
            {
                if (blobtopusX - 1 >= 0) { coorXmoveTo = blobtopusX - 1; }
                else { coorXmoveTo = blobtopusX + 1; }
                coorYmoveTo = blobtopusY;
            }

            //grid[8, 8] = actualField;

            string enableToMove = "zjZ0hX";
            if (enableToMove.Contains(grid[coorXmoveTo, coorYmoveTo]))
            {

                grid[blobtopusX, blobtopusY] = '0';

                grid[coorXmoveTo, coorYmoveTo] = actualGlyph;

                this.blobtopusX = coorXmoveTo;
                this.blobtopusY = coorYmoveTo;

                monsterX = blobtopusX;
                monsterY = blobtopusY;
            }

            return grid;
        }

        public override int whereToShoot() {
            return 10;
        }

    }
}