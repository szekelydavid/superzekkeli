using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;
using Console = SadConsole.Console;
using SadConsole.Ansi;
using System.IO;


namespace Arcade
{

    public class VictoryScreen : Console
    {
        public VictoryScreen() : base(100, 40)
        {
            DefaultBackground = Color.Black;
            DefaultForeground = Color.Black;

        }
        public void loadAnsiVic(int score)
        {

            var _hatterinfo = new Console(100, 40);
            {
                DefaultBackground = Color.Black;
                DefaultForeground = Color.Black;
            };

            Children.Add(_hatterinfo);
            var docinf = new SadConsole.Ansi.Document(@"victory.ans");
            var ansiinfoswr = new AnsiWriter(docinf, _hatterinfo);
            ansiinfoswr.CharactersPerSecond = 720;
            var ansitimer02 = new Timer(TimeSpan.FromSeconds(1));
            double ansitime = 0;
            ansitimer02.TimerElapsed += (timer, e) =>
            {
                ansitime++;
                ansiinfoswr.Process(ansitime);
            };
            Components.Add(ansitimer02);

            var fontmasterIBM = SadConsole.Global.LoadFont("Fonts/IBM.font");
            var doublesizedfontIBM = fontmasterIBM.GetFont(Font.FontSizes.Two);
            var victoryScoreConsole = new Console(12, 1, doublesizedfontIBM)
            {
                DefaultBackground = Color.Black,
                DefaultForeground = Color.Yellow,
            };
            victoryScoreConsole.Position = new Point(32, 15);
            victoryScoreConsole.Print(0, 0, "SCORE: "+ score,Color.Yellow);
            _hatterinfo.Children.Add(victoryScoreConsole);

        }

    }
}
