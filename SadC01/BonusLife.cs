namespace Arcade
{
    public class BonusLife : Bonus
    {
    public override int bonusX { get; set; }
    public override int bonusY { get; set; }
    public char actualGlyph='X';
    public BonusLife (int bex,int bey) {
                this.bonusX = bex;
                this.bonusY = bey;
            }
        public override void phaseChange(byte inb,char[,] grid)
                {
                    if (inb ==1 || inb==3)
                    {
                        this.actualGlyph = 'h';
                    }
                    if (inb==0 || inb==2 )
                    {
                        this.actualGlyph = 'X';
                    }
                    grid[this.bonusX, this.bonusY] = actualGlyph;
                }
    }
}