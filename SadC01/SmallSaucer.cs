using System;
using Microsoft.Xna.Framework.Media;

using System.Linq;

namespace Arcade
{
    public class SmallSaucer : Monster
    {
        public char actualGlyph = 'L';

        private char actualField;

        public int smallSaucerX { get; set; }
        public int smallSaucerY { get; set; }
        public override int monsterX { get; set; }
        public override int monsterY { get; set; }

        public override int plusScore { get; set; }

        Random rnd = new Random();

        private char[,] mozgasMinta = new char[9, 10]
        {
            {'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D'},
            {'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D'},
            {'K', 'D', 'D', 'N', 'N', 'N', 'N', 'N', 'N', 'D'},
            {'K', 'D', 'N', 'N', 'N', 'N', 'N', 'N', 'E', 'N'},
            {'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'E', 'D'},
            {'K', 'K', 'K', 'K', 'K', 'K', 'K', 'K', 'E', 'N'},
            {'E', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N', 'N'},
            {'D', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E'},
            {'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E'}
        };

        public SmallSaucer(int bex, int bey)
        {
            this.monsterX = bex;
            this.monsterY = bey;
            smallSaucerX = bex;
            smallSaucerY = bey;
            plusScore = 25;

        }

        public override void phaseChange(byte inb)
        {
            if (inb == 4)
            {
                this.actualGlyph = 'L';
            }

            if (inb == 0)
            {
                this.actualGlyph = '\\';
            }

        }

        public override char[,] moveOneStep(char[,] grid)
        {

            actualField = mozgasMinta[this.smallSaucerY, this.smallSaucerX];
            //GameLoop._mapscreen.Print(1, 1, "F");

            int coorXmoveTo = 0;
            int coorYmoveTo = 0;

            if (actualField == 'E')
            {
                coorXmoveTo = smallSaucerX;
                coorYmoveTo = smallSaucerY - 1;
            }
            if (actualField == 'K')
            {
                coorXmoveTo = smallSaucerX + 1;
                coorYmoveTo = smallSaucerY;
            }
            if (actualField == 'D')
            {
                coorXmoveTo = smallSaucerX;
                coorYmoveTo = smallSaucerY + 1;
            }
            if (actualField == 'N')
            {
                coorXmoveTo = smallSaucerX - 1;
                coorYmoveTo = smallSaucerY;
            }
            string enableToMove = "zjZ0hX";
            //grid[8, 8] = actualField;
            if (enableToMove.Contains(grid[coorXmoveTo, coorYmoveTo]))
            {
                grid[smallSaucerX, smallSaucerY] = '0';

                grid[coorXmoveTo, coorYmoveTo] = actualGlyph;

                this.smallSaucerX = coorXmoveTo;
                this.smallSaucerY = coorYmoveTo;

                monsterX = smallSaucerX;
                monsterY = smallSaucerY;


            }
            return grid;
        }
        public override int whereToShoot()
        {
            int randomInt = rnd.Next(4);
            if (randomInt == 0) { return 1; }
            if (randomInt == 1) { return 2; }
            if (randomInt == 2) { return 3; }
            if (randomInt == 3) { return 0; }
            else { return 1; }
        }
    }
}
