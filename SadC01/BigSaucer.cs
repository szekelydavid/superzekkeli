using System;
using Microsoft.Xna.Framework.Media;
using System.Linq;


namespace Arcade
{
    public class BigSaucer : Monster
    {
        public char actualGlyph = ']';

        private char actualField;

        public int bigSaucerX { get; set; }
        public int bigSaucerY { get; set; }
        public override int monsterX { get; set; }
        public override int monsterY { get; set; }

        public override int plusScore { get; set; }

        Random rndlov = new Random();

        private char[,] mozgasMinta = new char[9, 10]
        {
            {'D', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N'},
            {'D', 'D', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'E'},
            {'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'E', 'E'},
            {'D', 'D', 'K', 'K', 'K', 'K', 'K', 'K', 'E', 'E'},
            {'D', 'D', 'K', 'K', 'K', 'K', 'K', 'K', 'E', 'E'},
            {'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'E', 'E'},
            {'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'E', 'E'},
            {'D', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'E', 'E'},
            {'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'E'},

        };

        public BigSaucer (int bex,int bey) {
            this.monsterX = bex;
            this.monsterY = bey;
            bigSaucerX = bex;
            bigSaucerY = bey;
            plusScore = 20;
        }

        public override void phaseChange(byte inb)
        {
            if (inb == 4)
            {
                this.actualGlyph = 'M';
            }

            if (inb == 0)
            {
                this.actualGlyph = ']';
            }

        }

        public override char[,]  moveOneStep  (char[,] grid)
        {

            actualField = mozgasMinta[this.bigSaucerY,this.bigSaucerX];
            //GameLoop._mapscreen.Print(1, 1, "F");

            int coorXmoveTo =0;
            int coorYmoveTo =0;

            if (actualField == 'E')
            {
                coorXmoveTo = bigSaucerX;
                coorYmoveTo = bigSaucerY - 1;
            }
            if (actualField == 'K')
            {
                coorXmoveTo = bigSaucerX+1;
                coorYmoveTo = bigSaucerY;
            }
            if (actualField == 'D')
            {
                coorXmoveTo = bigSaucerX;
                coorYmoveTo = bigSaucerY + 1;
            }
            if (actualField == 'N')
            {
                coorXmoveTo = bigSaucerX-1;
                coorYmoveTo = bigSaucerY;
            }
            string enableToMove = "zjZ0hX";
            //grid[8, 8] = actualField;


            if (enableToMove.Contains(grid[coorXmoveTo, coorYmoveTo]))
            {
                grid[bigSaucerX, bigSaucerY] = '0';

                grid[coorXmoveTo, coorYmoveTo] = actualGlyph;

                this.bigSaucerX = coorXmoveTo;
                this.bigSaucerY = coorYmoveTo;

                monsterX = bigSaucerX;
                monsterY = bigSaucerY;
            }

            return grid;
        }
        public override int whereToShoot()
        {

            int randomlovInt = rndlov.Next(3);
            if (randomlovInt == 0) { return 1; }
            else if (randomlovInt == 1) { return 2; }
            else if (randomlovInt == 2) { return 3; }
            else { return 0; }
        }
    }

}