namespace Arcade
{
    public abstract class Bonus
    {
        public virtual int bonusX { get; set; }
        public virtual int bonusY { get; set; }

        public virtual void phaseChange(byte b,char[,] grid)
        {
        }
    }
}