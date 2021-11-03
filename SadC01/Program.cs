using System;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Arcade;
using SadConsole.Instructions;

namespace Arcade
{
    class GameLoop
    {
        //! §01

        public const int Width = 100;
        public const int Height = 40;


        public static MapScreen _mapscreen{ get; set; }
        public static SuperZekkeliScreen _zekkeliscreen { get; set; }
        public static Console startingConsole;


        public static Player _player { get; set; }
        public static int PLvillogkezd { get; set; }

        public bool gamestart = false;

        static void Main(string[] args)
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create(Width, Height);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Hook the update event that happens each frame so we can trap keys and respond.
            SadConsole.Game.OnUpdate = Update;

            // Start the game.
            SadConsole.Game.Instance.Run();

            //
            // Code here will not run until the game window closes.
            //

            SadConsole.Game.Instance.Dispose();
        }

        private static void Update(GameTime time)
        {
            //! START THE GAME
            if (_mapscreen.gamestart == false) {
                checkStart();
            }
            if (_mapscreen.gameover == true) {
                _player.Animation.CurrentFrame[0].Foreground = Color.Black;
                _player.IsVisible = false;
            }
            if (_mapscreen.victory == true)
            {
                _player.Animation.CurrentFrame[0].Foreground = Color.Black;
                _player.IsVisible = false;
            }

            if (_mapscreen.timeSum == 28) {
                {
                    _mapscreen.gamestart = true;
                    _player.Animation.CurrentFrame[0].Foreground = Color.White;
                    _zekkeliscreen.IsVisible = false;
                    _zekkeliscreen.IsFocused = false;
                    _mapscreen.Children.Remove(_zekkeliscreen);
                }
            }

            if (_mapscreen.gamestart == true)
            {
                checkPlayerButton();
                PlayerAnim();
                villogAnim(_mapscreen.timeSum);

                // !átadja a játékos koordinátáit
                _mapscreen.localMSPlayerX = _player.X;
                _mapscreen.localMSPlayerY = _player.Y;


                _mapscreen.renderTheGrid();

                playerHitsMonster();
                playerHitsBonus();

                _mapscreen.monsterBonusKill();

            }
        }
        private static void checkStart() {
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                _mapscreen.gamestart = true;
                _player.Animation.CurrentFrame[0].Foreground = Color.White;
                _zekkeliscreen.IsVisible = false;
                _zekkeliscreen.IsFocused = false;
                _mapscreen.Children.Remove(_zekkeliscreen);
            }


        }

        private static void checkPlayerButton()
        {

            if (_mapscreen.gamestart == true)
            {
                // Called each logic update.
                // As an example, we'll use the F5 key to make the game full screen
                if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5))
                {
                    SadConsole.Settings.ToggleFullScreen();
                }

                // Keyboard movement for Player character: Up arrow
                // Decrement player's Y coordinate by 1
                if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up))
                {
                    _player.iranyPL = 0;

                    Point newPoint = _player.Position + new Point(0, -1);
                    ;
                    if (!(newPoint.Y < 0))
                    {
                        _player.Position = newPoint;
                        _player.Y--;
                    }
                }

                // Keyboard movement for Player character: DOWN arrow
                // Increment player's Y coordinate by 1
                if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down))
                {
                    _player.iranyPL = 2;
                    Point newPoint = _player.Position + new Point(0, 1);

                    if (!(newPoint.Y > 8))
                    {
                        _player.Position = newPoint;
                        _player.Y++;
                    }
                }

                // Keyboard movement for Player character: Left arrow
                // Decrement player's X coordinate by 1
                if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left))
                {
                    _player.iranyPL = 3;
                    Point newPoint = _player.Position + new Point(-1, 0);

                    if (!(newPoint.X < 0))
                    {
                        _player.Position = newPoint;
                        _player.X--;
                    }
                }

                // Keyboard movement for Player character: RIGHT arrow
                // Increment player's X coordinate by 1
                if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
                {
                    _player.iranyPL = 1;
                    Point newPoint = _player.Position + new Point(1, 0);

                    if (!(newPoint.X > 9))
                    {
                        _player.Position = newPoint;
                        _player.X++;
                    }

                }

                if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
                {

                    if (_mapscreen.playerEnergyCount > 0)
                    {
                        playerLaserGenerate(_player.X, _player.Y, _player.iranyPL);
                        _mapscreen.playerEnergyCount--;
                    }

                }
            }

        }

        public static void playerHitsMonster()
        {
            string monstersString = "FVJKL\\M]N";
            if (monstersString.Contains(_mapscreen.gamegrid[_player.X, _player.Y]))
            {
                _mapscreen.removeMonster(_player.X, _player.Y);
                _mapscreen.playerLifeCount--;
                //! TESZT
                PLvillogkezd = _mapscreen.timeSum;

                _player.Animation.CurrentFrame[0].Foreground = Color.IndianRed;
            }
        }
        public static void playerHitsBonus()
        {
            string energystring = "Zjz";
            if (energystring.Contains(_mapscreen.gamegrid[_player.X, _player.Y]))
            {
                _mapscreen.removeBonus(_player.X, _player.Y);
                _mapscreen.playerEnergyCount += 4;
                //! TESZT
                PLvillogkezd = _mapscreen.timeSum;
                _player.Animation.CurrentFrame[0].Foreground = Color.LightGreen;
            }
            string healthstring = "xh";
            if (healthstring.Contains(_mapscreen.gamegrid[_player.X, _player.Y]))
            {
                _mapscreen.removeBonus(_player.X, _player.Y);
                _mapscreen.playerLifeCount += 1;
                //! TESZT
                PLvillogkezd = _mapscreen.timeSum;
                _player.Animation.CurrentFrame[0].Foreground = Color.LightGreen;
            }
        }

        public static void villogAnim(int INtimesum)
        {

            if (INtimesum - PLvillogkezd > 4)
            {
                _player.Animation.CurrentFrame[0].Foreground = Color.White;
            }
        }

            public static void playerLaserGenerate(int bex, int bey, int direction) {

            int Direction;
            int playerlaserX = 0;
            int playerlaserY = 0;

            playerlaserX = bex;
            playerlaserY = bey;
            Direction = direction;

            if (Direction == 0)
            {
                if ((bey ) >= 1) { playerlaserY = bey - 1; }
            }
            if (Direction == 1)
            {
                if ((bex) <= 8) { playerlaserX = bex + 1; }
            }
            if (Direction == 2)
            {
                if ((bey) <= 7) { playerlaserY = bey + 1; }
            }
            if (Direction == 3)
            {
                if ((bex ) >= 1) { playerlaserX = bex - 1; }
            }
            _mapscreen.playerLaserGrid[playerlaserX, playerlaserY] = Direction;

        }




        public static void PlayerAnim()
        {
            if ((_player.iranyPL == 1)&&((_mapscreen.timeSum%4==1)||(_mapscreen.timeSum%4==2)))
            {
                _player.pGlyph = 'D';
            }
            if ((_player.iranyPL == 1)&&((_mapscreen.timeSum%4==3)||(_mapscreen.timeSum%4==0)))
            {
                _player.pGlyph = 'T';
            }

            if ((_player.iranyPL == 2)&&((_mapscreen.timeSum%4==1)||(_mapscreen.timeSum%4==2)))
            {
                _player.pGlyph = 'C';
            }
            if ((_player.iranyPL == 2)&&((_mapscreen.timeSum%4==3)||(_mapscreen.timeSum%4==0)))
            {
                _player.pGlyph = 'S';
            }



            if ((_player.iranyPL == 3)&&((_mapscreen.timeSum%4==1)||(_mapscreen.timeSum%4==2)))
            {
                _player.pGlyph = 'E';
            }
            if ((_player.iranyPL == 3)&&((_mapscreen.timeSum%4==3)||(_mapscreen.timeSum%4==0)))
            {
                _player.pGlyph = 'U';
            }

            if ((_player.iranyPL == 0)&&((_mapscreen.timeSum%4==1)||(_mapscreen.timeSum%4==2)))
            {
                _player.pGlyph = 'B';
            }
            if ((_player.iranyPL == 0)&&((_mapscreen.timeSum%4==3)||(_mapscreen.timeSum%4==0)))
            {
                _player.pGlyph = 'R';
            }

        }





        private static void Init()
        {

            var fontMaster = SadConsole.Global.LoadFont("Fonts/chess.font");

            var bigSizedFont = fontMaster.GetFont(Font.FontSizes.Four);

            startingConsole = new Console(Width, Height, bigSizedFont);
            //startingConsole.SetGlyph(3, 3, 4);

            _mapscreen = new MapScreen();
            startingConsole.Children.Add(_mapscreen);


            // Set our new console as the thing to render and process
            SadConsole.Global.CurrentScreen = startingConsole;
            var fontMasterPL = SadConsole.Global.LoadFont("Fonts/chess.font");
            var normalSizedFontPL = fontMasterPL.GetFont(Font.FontSizes.Four);

            //! GAMESTARt
            _mapscreen.gamestart = false;

            //! váltoZÁS
            // create an instance of player

            _player = new Player();
            _player.Font = normalSizedFontPL;
            _player.Position = new Point(2, 2);
            _player.X = 2;
            _player.Y = 2;
            _player.pGlyph = '5';
            _player.Animation.CurrentFrame[0].Background = Color.Transparent;
            _player.Animation.CurrentFrame[0].Foreground = Color.Black;
            // add the player Entity to our only console
            // so it will display on screen
            startingConsole.Children.Add(_player);
            _zekkeliscreen = new SuperZekkeliScreen();
            _mapscreen.Children.Add(_zekkeliscreen);

        }
    }
}
