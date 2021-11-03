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

    public class SuperZekkeliScreen : Console
    {
        public SuperZekkeliScreen() : base(100, 40)
        {

            var _hatterinfo = new Console(100, 40);
            {
                DefaultBackground = Color.Black;
                DefaultForeground = Color.Black;
            };

            Children.Add(_hatterinfo);
            var docinf = new SadConsole.Ansi.Document(@"supezekkeli02.ans");
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
           
        }
    }
}

