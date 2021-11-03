
namespace Arcade
{
public class BonusEnergy : Bonus
{
    public override int bonusX { get; set; }
        public override int bonusY { get; set; }
    char actualGlyph='X';


    public BonusEnergy (int bex,int bey) {
        this.bonusX = bex;
        this.bonusY = bey;
    }
    public override void phaseChange(byte inb,char[,] grid)
    {
        if (inb == 0)
        {
            this.actualGlyph = 'Z';
        }
        if (inb == 1)
        {
            this.actualGlyph = 'j';
        }
        if (inb == 2)
        {
            this.actualGlyph = 'z';
        }
        grid[this.bonusX, this.bonusY] = actualGlyph;
    }
}
}