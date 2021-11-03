using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SadConsole;
using Console = SadConsole.Console;


namespace Arcade
{

    public class MapScreen : Console
    {
        public readonly SadConsole.Timer progressTimer;
        public SadConsole.ScrollingConsole mapConsole;
        public SadConsole.ScrollingConsole gridConsole;
        public SadConsole.ScrollingConsole statsConsole;
        public SadConsole.ScrollingConsole playerLaserConsole;
        public SadConsole.ScrollingConsole monsterLaserConsole;
        public SadConsole.ScrollingConsole scoreConsole;
        public SadConsole.ScrollingConsole msgLogConsole;

        public int timeSum { get; set; }
        public int animtimer { get; set; }
        //!rács, amin a szörnyek vannak
        public char[,] gamegrid { get; set; }
        public int[,] playerLaserGrid { get; set; }

        public int[,] monsterLaserGrid { get; set; }
        // szörnyek
        public List<Monster> monsterList { get; set; }
        // bónuszok
        public List<Bonus> bonusList { get; set; }
        //játékos lövései
        //public List<PlayerLaser> playerLaserList { get; set; }
        //! A Játékos koordinátái (átadott érték)
        public int localMSPlayerX { get; set; }
        public int localMSPlayerY { get; set; }


        //! HP
        public int playerLifeCount { get; set; }
        //! ENERGIA
        public int playerEnergyCount { get; set; }
        //! PLAYER SCORE
        public int playerScore { get; set; }

        public string theMessageInLog { get; set; }

        public bool gamestart { get; set; }

        public bool gameover { get; set; }

        public bool victory { get; set; }
        public MapScreen() : base(100, 40)
        {
            //! Számláló nullázása
            timeSum = 0;

            mapConsole = new ScrollingConsole(100, 40)
            {
                DefaultBackground = Color.Black,
                DefaultForeground = Color.White,
            };

            mapConsole.Position = new Point(0, 0);

            Children.Add(mapConsole);
            IsVisible = true;
            IsFocused = true;
            Parent = SadConsole.Global.CurrentScreen;


            var fontMasterPL = SadConsole.Global.LoadFont("Fonts/chess.font");
            var normalSizedFontPL = fontMasterPL.GetFont(Font.FontSizes.Four);

            //! a szörnyeket megjelenítő és kezelelő konzol inicializálása
            gridConsole = new ScrollingConsole(10, 9, normalSizedFontPL)
            {
                DefaultBackground = Color.Transparent,
                DefaultForeground = Color.White,
            };
            mapConsole.Children.Add(gridConsole);

            //! játékos lézerlövéseit megjelenítő konzol
            playerLaserConsole = new ScrollingConsole(10, 9, normalSizedFontPL)
            {
                DefaultBackground = Color.Transparent,
                DefaultForeground = Color.White,
            };
            mapConsole.Children.Add(playerLaserConsole);

            //! szörnyek lézerlövéseit megjelenítő konzol
            monsterLaserConsole = new ScrollingConsole(10, 9, normalSizedFontPL)
            {
                DefaultBackground = Color.Transparent,
                DefaultForeground = Color.White,
            };
            playerLaserConsole.Children.Add(monsterLaserConsole);


            //! játékos statisztikákat megjelenítő konzol incializálása
            var normalSizedFontST = fontMasterPL.GetFont(Font.FontSizes.Two);
            statsConsole = new ScrollingConsole(6, 20, normalSizedFontST)
            {
                DefaultBackground = Color.Transparent,
                DefaultForeground = Color.White,
            };
            statsConsole.Position = new Point(19, 3);

            mapConsole.Children.Add(statsConsole);

            //! Pontokat megjelenítő konzol 💰
            var fontmasterIBM = SadConsole.Global.LoadFont("Fonts/IBM.font");
            var doublesizedfontIBM = fontmasterIBM.GetFont(Font.FontSizes.Two);
            var scoreConsole = new ScrollingConsole(10, 2, doublesizedfontIBM)
            {
                DefaultBackground = Color.Black,
                DefaultForeground = Color.White,
            };
            scoreConsole.Position = new Point(2, 5);
            statsConsole.Children.Add(scoreConsole);
            scoreConsole.Print(0, 0, "  SCORE:");
            //! MESSAGE LOG CONSOLE ✉️

            var msgLogConsole = new ScrollingConsole(16, 2, doublesizedfontIBM)
            { UsePrintProcessor = true,
                DefaultBackground = Color.Black,
                DefaultForeground = Color.White,
            };
            msgLogConsole.Position = new Point(4, 18);
            mapConsole.Children.Add(msgLogConsole);

            //! unvisible
            var testkonzol01 = new Console(100, 40)
            {
                DefaultBackground = Color.Black,
                DefaultForeground = Color.Black,
            };
            mapConsole.Children.Add(testkonzol01);
            testkonzol01.SetGlyph(2, 2, 'C', Color.White);
            var _gameoverscreen = new GameOverScreen();
            _gameoverscreen.IsVisible = false;
            testkonzol01.IsVisible = false;
            testkonzol01.Children.Add(_gameoverscreen);

            var _victoryscreen = new VictoryScreen();
            _victoryscreen.IsVisible = false;
            testkonzol01.Children.Add(_victoryscreen);


            //! TESZZT


            //! _____ISMÉTLŐDŐ CIKLUS___
            //! idő-számláló kezelése ⌛
            //! _____UPDATEK_______

            progressTimer = new Timer(TimeSpan.FromSeconds(0.25));
            progressTimer.TimerElapsed += (timer, e) =>
            {
                if (gameover == true) {
                    testkonzol01.IsVisible = true;
                    _gameoverscreen.IsVisible = true;
                    _gameoverscreen.loadAnsi();
                    GameLoop._player.Animation.CurrentFrame[0].Foreground = Color.Black;
                }
                if (timeSum == 360) {
                    theMessageInLog = "ALMOST WON!";
                }


                if((timeSum==400) &&(!victory))
                {
                    gameover = false;
                    victory = true;
                    testkonzol01.IsVisible = true;
                    _victoryscreen.IsVisible = true;
                    _victoryscreen.loadAnsiVic(playerScore);
                    GameLoop._player.Animation.CurrentFrame[0].Foreground = Color.Black;

                }

                if (timeSum == 1)
                {
                    theMessageInLog = "[c:r f:DodgerBlue]KNOW YOUR ENEMY!";
                }
                timeSum++;
                if (timeSum % 1 == 0)
                {
                    updateTheGridLasers();
                }
                if (timeSum % 2 == 0) {
                    updateTheGridMonsters();
                    updateTheGridMonsterLasers();
                    scoreConsole.Print(0, 1, "  " + playerScore.ToString());
                }

                //! MONSTEREK SPAWNJA
                if (timeSum > 50)
                {
                    //! PHASE ONE
                    if ((timeSum > 50) && timeSum < 180)
                    {
                        if (timeSum % 16 == 0) {
                            if (valszamDobas(4)) {
                                spawnBlobptosaurus();
                            }
                        }
                        if (timeSum % 12 == 0)
                        {
                            if (valszamDobas(3))
                            {
                                spawnMosquito();
                            }
                        }
                        if (timeSum % 20 == 0)
                        {
                            if (valszamDobas(3))
                            {
                                spawnSmallSaucer();
                            }
                        }
                        if (timeSum % 24 == 0)
                        {
                            if (valszamDobas(2))
                            {
                                spawnBigSaucer();
                            }
                        }
                    }

                    //!PHASE TWO
                    if ((timeSum > 180) && (timeSum < 330))
                    {
                        if (timeSum % 12 == 0)
                        {
                            if (valszamDobas(3))
                            {
                                spawnBlobptosaurus();
                            }
                        }
                        if (timeSum % 10 == 0)
                        {
                            if (valszamDobas(3))
                            {
                                spawnMosquito();
                            }
                        }
                        if (timeSum % 12 == 0)
                        {
                            if (valszamDobas(3))
                            {
                                spawnSmallSaucer();
                            }
                        }
                        if (timeSum % 16 == 0)
                        {
                            if (valszamDobas(2))
                            {
                                spawnBigSaucer();
                            }
                        }
                    }
                    if ((timeSum > 330) && (timeSum < 400))
                    {
                        if (timeSum % 15 == 0)
                        {
                            if (valszamDobas(3))
                            {
                                spawnBonusEnergy();
                            }
                        }

                        if (timeSum % 10 == 0)
                        {
                            if (valszamDobas(3))
                            {
                                spawnBlobptosaurus();
                            }
                        }
                        if (timeSum % 8 == 0)
                        {
                            if (valszamDobas(3))
                            {
                                spawnMosquito();
                            }
                        }
                        if (timeSum % 10 == 0)
                        {
                            if (valszamDobas(3))
                            {
                                spawnSmallSaucer();
                            }
                        }
                        if (timeSum % 14 == 0)
                        {
                            if (valszamDobas(2))
                            {
                                spawnBigSaucer();
                            }
                        }
                    }

                }

                //!BONUSZOK SPAWNJA
                if (timeSum > 30)
                {
                    if (timeSum % 16 == 0)
                    {
                        if (valszamDobas(3))
                        {
                            spawnBonusEnergy();
                        }
                    }
                    if (timeSum % 18 == 0)
                    {
                        if (valszamDobas(3))
                        {
                            spawnBonusLife();
                        }
                    }
                }
                if (timeSum % 100 == 0) {
                    spawnBonusEnergy();
                }

                    if (timeSum % 2 == 0)
                {
                    //! szörnyek animációja
                    foreach (Monster m in monsterList)
                    {
                        byte beB = (byte)(timeSum % 8);
                        m.phaseChange(beB);
                    }
                }

                    if (timeSum % 3 == 0)
                {
                    //! szörnyek lövése
                    foreach (Monster m in monsterList)
                    {
                        if (valszamDobas(2)){
                            int dirToShoot = m.whereToShoot();
                            monsterLaserGenerate(m.monsterX, m.monsterY, dirToShoot);
                        }
                    }
                    monsterBonusKill();
                }

                //!Bónuszok animációja
                foreach (Bonus b in bonusList)
                {
                    byte beB = (byte)(timeSum % 4);
                    b.phaseChange(beB, gamegrid);
                }
                //! robbanások animációja
                if (timeSum % 8 == 0)
                {
                    updateTheGridExplosions();

                }

                if (timeSum % 4 == 0)
                {
                    msgLogConsole.Clear();
                    msgLogConsole.Print(0, 0, theMessageInLog);
                }
                if (timeSum == 12) {
                    spawnBigSaucer();
                }

                //! statisztikák aktualizálása
                updateTheStats();

            //! GAME OVER

            };
            Components.Add(progressTimer);


            //! ___LISTÁK INICIALIZÁLÁSA___
            monsterList = new List<Monster>();
            bonusList = new List<Bonus>();

            gamegrid = new char[10, 9];
            playerLaserGrid = new int[10, 10];
            monsterLaserGrid = new int[10, 10];

            initTheGrids();

            //! ________________________DATA
            //! Player adatok inicaializálása ÉLET|ENERGIA  ❤️ ⚡

            playerLifeCount = 4;
            playerEnergyCount = 12;

            //! ________________________DATA
            //! PONTSZÁM 🎖️
            playerScore = 0;

        }


        //! ______MONSTER SPAWNOK___ 🛸
        public void spawnBlobptosaurus()
        {

            Random rnd = new Random();

            for (int i = 0; i < 5; i++)
            {
                int blX = rnd.Next(1, 9);
                int blY = rnd.Next(1, 8);
                if (playerdistanceValidator(blX, blY) == true)
                {
                    var m = new Blobtopus(blX, blY);
                    gamegrid[blX, blY] = 'J';
                    monsterList.Add(m);
                    theMessageInLog = "[c:r f:Orange] +Bloptopsaurus";

                    return;
                }
            }
        }

        public void spawnMosquito()
        {
            Random rnd = new Random();

            for (int i = 0; i < 5; i++)
            {
                int mosqX = rnd.Next(0, 10);
                int mosqY = rnd.Next(0, 3);

                if (playerdistanceValidator(mosqX, mosqY) == true)
                {
                    var m = new Mosquito(mosqX, mosqY);
                    gamegrid[mosqX, mosqY] = 'F';
                    monsterList.Add(m);

                    theMessageInLog = "[c:r f:Orange] +Mosquito";

                    return;
                }
            }
        }

        public void spawnSmallSaucer()
        {
            Random rnd = new Random();

            int smallSX;
            int smallSY;
            for (int i = 0; i < 5; i++)
            {
                int fennORlenn = rnd.Next(0, 2);
                smallSX = rnd.Next(0, 10);
                if (fennORlenn == 0)
                {
                    smallSY = rnd.Next(0, 3);
                }
                else
                {
                    smallSY = rnd.Next(7, 9);
                }
                if (playerdistanceValidator(smallSX, smallSY) == true)
                {
                    var m = new SmallSaucer(smallSX, smallSY);
                    gamegrid[smallSX, smallSY] = 'L';
                    monsterList.Add(m);

                    theMessageInLog = "[c:r f:Orange]+SmallSaucer";

                    return;
                }
            }
        }

        public void spawnBigSaucer()
        {
            Random rnd = new Random();
            int BigSX;
            int BigSY;
            for (int i = 0; i < 5; i++)
            {


                int fennORlenn = rnd.Next(0, 2);
                BigSX = rnd.Next(0, 10);
                if (fennORlenn == 0)
                {
                    BigSY = rnd.Next(0, 3);
                }
                else
                {
                    BigSY = rnd.Next(7, 9);
                }
                if (playerdistanceValidator(BigSX, BigSY) == true)
                {
                    var m = new BigSaucer(BigSX, BigSY);
                    gamegrid[BigSX, BigSY] = 'M';
                    monsterList.Add(m);
                    theMessageInLog = "[c:r f:Orange]+BigSaucer";
                    return;
                }
            }
        }


        //! _____________________________
        public bool playerdistanceValidator(int mSPx, int mSPy)
        {
            if ((localMSPlayerX - 1 <= mSPx) && (localMSPlayerX + 1 >= mSPx))
            {
                return false;
            }
            if ((localMSPlayerY - 1 <= mSPy) && (localMSPlayerY + 1 >= mSPy))
            {
                return false;
            }
            return true;
        }

        //! _________________________________
        //! Valoszinuseg szamlalo___🎲
        bool valszamDobas(int x)
        {
            Random rnd = new Random();
            int dobott = rnd.Next(1, x + 1);
            if (x == dobott) { return true; }
            else { return false; }
        }
        //!___________________________________

        public void spawnMTest()
        {

        }

        //! MONSTER-BONUS ütközés
        //!_____________________________________

        public void monsterBonusKill()
        {
            for (int mnstcount = monsterList.Count - 1; mnstcount >= 0; mnstcount--)
            {
                for (int bnscount = bonusList.Count - 1; bnscount >= 0; bnscount--)
                {
                    if ((monsterList[mnstcount].monsterX == bonusList[bnscount].bonusX) && ((monsterList[mnstcount].monsterY == bonusList[bnscount].bonusY)))
                    {
                        bonusList.RemoveAt(bnscount);
                        theMessageInLog = "[c:r f:Red]Destroyed Bonus";
                    }
                }
            }
        }


        //! +❤️
        public void spawnBonusLife() {
            theMessageInLog = "[c:r f:GREEN]+LIFE";
            Random rnd = new Random();
            for (int i = 0; i < 5; i++) {
                int xspBL = rnd.Next(0, 10);
                int yspBL = rnd.Next(0, 9);
                if (gamegrid[xspBL, yspBL] == '0')
                {
                    var bLF = new BonusLife(xspBL, yspBL);
                    bonusList.Add(bLF);
                    gamegrid[xspBL, yspBL] = 'X';
                    return;
                }
            }
        }

        //! +⚡
        public void spawnBonusEnergy() {
            for (int i = 0; i < 5; i++)
            {
                theMessageInLog = "[c:r f:GREEN]+ENERGY";
                Random rnd = new Random();
                int xspBL = rnd.Next(0, 9);
                int yspBL = rnd.Next(0, 8);
                if (gamegrid[xspBL, yspBL] == '0')
                {
                    var bLF = new BonusEnergy(xspBL, yspBL);
                    bonusList.Add(bLF);
                    gamegrid[xspBL, yspBL] = 'Z';
                    return;
                }
            }
        }

        public void initTheGrids()
        {
            //! monsterGrid
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    this.gamegrid[i, j] = '0';
                }
            }
            //! playerLaserGrid
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    this.playerLaserGrid[i, j] = 9;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    this.monsterLaserGrid[i, j] = 9;
                }
            }

        }
        public void updateTheGridExplosions()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    if (gamegrid[i, j] == 'G')
                    {
                        this.gridConsole.SetGlyph(i, j, 'W');
                        this.gridConsole.SetForeground(i, j, Color.White);
                        this.gridConsole.SetBackground(i, j, Color.Transparent);
                        gamegrid[i, j] = 'W';
                    }
                    else if (gamegrid[i, j] == 'W')
                    {
                        this.gridConsole.SetGlyph(i, j, '0');
                        this.gridConsole.SetForeground(i, j, Color.Transparent);
                        this.gridConsole.SetBackground(i, j, Color.Transparent);
                        gamegrid[i, j] = '0';
                    }
                    else if (gamegrid[i, j] == '5')
                    {
                        this.gridConsole.SetGlyph(i, j, 'W');
                        this.gridConsole.SetForeground(i, j, Color.White);
                        this.gridConsole.SetBackground(i, j, Color.Transparent);
                        gamegrid[i, j] = '7';
                    }
                    else if (gamegrid[i, j] == '7')
                    {
                        this.gridConsole.SetGlyph(i, j, '0');
                        this.gridConsole.SetForeground(i, j, Color.Transparent);
                        this.gridConsole.SetBackground(i, j, Color.Transparent);
                        gamegrid[i, j] = '0';
                    }

                }
            }
        }

        public void updateTheGridMonsters()
        {
            foreach (Monster m in monsterList)
            {
                gamegrid = m.moveOneStep(gamegrid);
            }
        }

        public bool isItHit(int x, int y)
        {
            string enemiesString = "GFVWJKL\\M]N";
            char targetChar = gamegrid[x, y];
            if (enemiesString.Contains(targetChar))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isItHitPlayer(int bex, int bey)
        {
            if ((bex == localMSPlayerX) && (bey == localMSPlayerY))
            {
                return true;
            }
            else { return false; }
        }

        public void removeMonster(int bex, int bey)
        {
            for (int mnstcount = monsterList.Count - 1; mnstcount >= 0; mnstcount--)
            {
                if ((monsterList[mnstcount].monsterX == bex) && ((monsterList[mnstcount].monsterY == bey)))
                {
                    playerScore += monsterList[mnstcount].plusScore;
                    //! TESZT
                    monsterList.RemoveAt(mnstcount);
                    gamegrid[bex, bey] = 'G';
                }
            }
        }
        public void removeBonus(int bex, int bey)
        {
            for (int bnscount = bonusList.Count - 1; bnscount >= 0; bnscount--)
            {
                if ((bonusList[bnscount].bonusX == bex) && ((bonusList[bnscount].bonusY == bey)))
                {
                    bonusList.RemoveAt(bnscount);
                    //
                    theMessageInLog = "[c:r f:Green]PowerUP";
                    gamegrid[bex, bey] = '5';
                }
            }
        }
        //! lézerek mozgatása, ellenőrzés, hogy találnak-e

        //!______ PLAYER LAZER UPDATE!!

        public void updateTheGridLasers()
        {

            int[,] returngrid = new int[10, 9];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    returngrid[i, j] = 9;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {


                    if (playerLaserGrid[i, j] == 0)
                    {

                        if ((j - 1) >= 0)
                        {

                            if (isItHit(i, j - 1))
                            {
                                theMessageInLog = "[c:r f:Red]HIT";
                                removeMonster(i, j - 1);
                                returngrid[i, j - 1] = 9;
                            }
                            else
                            {
                                returngrid[i, j - 1] = 0;
                            }
                        }

                    }

                    if (playerLaserGrid[i, j] == 1)
                    {
                        if ((i + 1) < 10)
                        {
                            if (isItHit(i + 1, j))
                            {
                                theMessageInLog = "[c:r f:Red]HIT";
                                removeMonster(i + 1, j);
                                returngrid[i + 1, j] = 9;
                            }
                            else
                            {
                                returngrid[i + 1, j] = 1;
                            }
                        }

                    }

                    if (playerLaserGrid[i, j] == 2)
                    {
                        if ((j + 1) < 9)
                        {
                            if (isItHit(i, j + 1))
                            {
                                theMessageInLog = "[c:r f:Red]HIT";
                                removeMonster(i, j + 1);
                                returngrid[i, j + 1] = 9;
                            }
                            else
                            {
                                returngrid[i, j + 1] = 2;
                            }
                        }
                    }

                    if (playerLaserGrid[i, j] == 3)
                    {
                        if ((i - 1) >= 0)
                        {
                            if (isItHit(i - 1, j))
                            {
                                theMessageInLog = "[c:r f:Red]HIT";
                                removeMonster(i - 1, j);
                                returngrid[i - 1, j] = 0;
                            }
                            else
                            {
                                returngrid[i - 1, j] = 3;
                            }
                        }
                    }
                }
            }
            //! átírja a rácsot
            playerLaserGrid = returngrid;
        }

        //!______ MONSTER LAZER UPDATE!!
        public void updateTheGridMonsterLasers()
        {
            int[,] returnMSgrid = new int[10, 9];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    returnMSgrid[i, j] = 9;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {


                    if (monsterLaserGrid[i, j] == 0)
                    {

                        if ((j - 1) >= 0)
                        {

                            if (isItHitPlayer(i, j - 1))
                            {
                                theMessageInLog = "[c:r f:Orange]DAMAGE";
                                playerLifeCount--;
                                GameLoop.PLvillogkezd = timeSum;
                                returnMSgrid[i, j - 1] = 9;
                            }
                            else
                            {
                                returnMSgrid[i, j - 1] = 0;
                            }
                        }

                    }

                    if (monsterLaserGrid[i, j] == 1)
                    {
                        if ((i + 1) < 10)
                        {
                            if (isItHitPlayer(i + 1, j))
                            {
                                theMessageInLog = "[c:r f:Orange]DAMAGE";
                                playerLifeCount--;
                                returnMSgrid[i + 1, j] = 9;
                            }
                            else
                            {
                                returnMSgrid[i + 1, j] = 1;
                            }
                        }

                    }

                    if (monsterLaserGrid[i, j] == 2)
                    {
                        if ((j + 1) < 9)
                        {
                            if (isItHitPlayer(i, j + 1))
                            {
                                theMessageInLog = "[c:r f:Orange]DAMAGE";
                                playerLifeCount--;
                                returnMSgrid[i, j + 1] = 9;
                            }
                            else
                            {
                                returnMSgrid[i, j + 1] = 2;
                            }
                        }
                    }

                    if (monsterLaserGrid[i, j] == 3)
                    {
                        if ((i - 1) >= 0)
                        {
                            if (isItHitPlayer(i - 1, j))
                            {
                                theMessageInLog = "[c:r f:Orange]DAMAGE";
                                playerLifeCount--;
                                returnMSgrid[i - 1, j] = 0;
                            }
                            else
                            {
                                returnMSgrid[i - 1, j] = 3;
                            }
                        }
                    }
                }
            }
            //! átírja a rácsot
            monsterLaserGrid = returnMSgrid;
        }
        //! MONSTER LASER GENERÁTOR
        public void monsterLaserGenerate(int bex, int bey, int direction)
        {
            int Direction;
            int monsterlaserX = 0;
            int monsterlaserY = 0;

            monsterlaserX = bex;
            monsterlaserY = bey;
            Direction = direction;
            if (Direction == 10)
            {
            }
            else
            {
                if (Direction == 0)
                {
                    if ((bey) >= 1) { monsterlaserY = bey - 1; }
                }
                if (Direction == 1)
                {
                    if ((bex) <= 8) { monsterlaserX = bex + 1; }
                }
                if (Direction == 2)
                {
                    if ((bey) <= 7) { monsterlaserY = bey + 1; }
                }
                if (Direction == 3)
                {
                    if ((bex) >= 1) { monsterlaserX = bex - 1; }
                }
                monsterLaserGrid[monsterlaserX, monsterlaserY] = Direction;
            }
        }


        //!


        public void renderTheGrid()
        {

            //! GAMEGRID____________________
            //! CONTAINS: MONSTERS!

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (gamegrid[i, j] == '?')
                    {
                        removeMonster(i, j);
                        gamegrid[i, j] = '0';
                    }
                    if (gamegrid[i, j] == '0')
                    {
                        this.gridConsole.SetGlyph(i, j, '0');
                        this.gridConsole.SetForeground(i, j, Color.Transparent);
                        this.gridConsole.SetBackground(i, j, Color.Transparent);
                    }
                    else
                    {
                        char actualGlyph = gamegrid[i, j];
                        this.gridConsole.SetGlyph(i, j, actualGlyph);
                        this.gridConsole.SetForeground(i, j, Color.White);
                        this.gridConsole.SetBackground(i, j, Color.Transparent);
                    }
                }
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    this.monsterLaserConsole.SetBackground(i, j, Color.Transparent);
                    if (this.monsterLaserGrid[i, j] == 9)
                    {
                        this.monsterLaserConsole.SetGlyph(i, j, 'O');
                        this.monsterLaserConsole.SetForeground(i, j, Color.White);
                        this.monsterLaserConsole.SetBackground(i, j, Color.Transparent);

                    }
                    if (this.monsterLaserGrid[i, j] != 9)
                    {
                        this.monsterLaserConsole.SetGlyph(i, j, '#');
                        this.monsterLaserConsole.SetForeground(i, j, Color.White);
                        this.monsterLaserConsole.SetBackground(i, j, Color.Transparent);
                    }
                }
            }



            //! PLAYERLASERCONSOLE______________
            //! CONTAINS: PLAYERLASERS, ||GRID CORNERS||
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    this.playerLaserConsole.SetBackground(i, j, Color.Transparent);
                    if (this.playerLaserGrid[i, j] == 9)
                    {
                        this.playerLaserConsole.SetGlyph(i, j, 'O');
                        this.playerLaserConsole.SetForeground(i, j, Color.White);
                        this.playerLaserConsole.SetBackground(i, j, Color.Transparent);
                        if ((i == 0) && (j == 0)) { this.playerLaserConsole.SetGlyph(0, 0, 'k'); }
                        if ((i == 9) && (j == 0)) { this.playerLaserConsole.SetGlyph(9, 0, 'm'); }
                        if ((i == 9) && (j == 8)) { this.playerLaserConsole.SetGlyph(9, 8, 'o'); }
                        if ((i == 0) && (j == 8)) { this.playerLaserConsole.SetGlyph(0, 8, 'i'); }

                    }
                    if (this.playerLaserGrid[i, j] != 9)
                    {
                        this.playerLaserConsole.SetGlyph(i, j, '6');
                        this.playerLaserConsole.SetForeground(i, j, Color.White);
                        this.playerLaserConsole.SetBackground(i, j, Color.Transparent);
                    }
                }
            }




        }



        public void updateTheStats()
        {

            //! ÉLETEK | SZÍVECSKÉK


            //! IKoN AZ ELEJÉN
            this.statsConsole.SetGlyph(1, 0, 'd');

            //! SZÍVEK
            if (playerLifeCount > 4) { playerLifeCount = 4; }


                if (playerLifeCount == 0)
            {
                this.statsConsole.SetGlyph(2, 0, '8', Color.White, Color.Black);
                this.statsConsole.SetGlyph(3, 0, '0', Color.Black, Color.Black);
                this.statsConsole.SetGlyph(5, 0, '0', Color.Black, Color.Black);
                this.statsConsole.SetGlyph(5, 0, '0', Color.Black, Color.Black);
                this.gameover = true;


            }
            if (playerLifeCount == 1)
                {
                    this.statsConsole.SetGlyph(2, 0, '4', Color.White, Color.Black);
                    this.statsConsole.SetGlyph(3, 0, '0', Color.Black, Color.Black);
                    this.statsConsole.SetGlyph(5, 0, '0', Color.Black, Color.Black);
                    this.statsConsole.SetGlyph(5, 0, '0', Color.Black, Color.Black);

                }
                if (playerLifeCount == 2)
                {
                    this.statsConsole.SetGlyph(2, 0, '4', Color.White, Color.Black);
                    this.statsConsole.SetGlyph(3, 0, '4', Color.White, Color.Black);
                    this.statsConsole.SetGlyph(4, 0, '0', Color.Black, Color.Black);
                    this.statsConsole.SetGlyph(5, 0, '0', Color.Black, Color.Black);

                }
                if (playerLifeCount == 3)
                {
                    this.statsConsole.SetGlyph(2, 0, '4', Color.White, Color.Black);
                    this.statsConsole.SetGlyph(3, 0, '4', Color.White, Color.Black);
                    this.statsConsole.SetGlyph(4, 0, '4', Color.White, Color.Black);
                    this.statsConsole.SetGlyph(5, 0, '0', Color.Black, Color.Black);

                }
                if (playerLifeCount == 4)
                {
                    this.statsConsole.SetGlyph(2, 0, '4', Color.White, Color.Black);
                    this.statsConsole.SetGlyph(3, 0, '4', Color.White, Color.Black);
                    this.statsConsole.SetGlyph(4, 0, '4', Color.White, Color.Black);
                    this.statsConsole.SetGlyph(5, 0, '4', Color.White, Color.Black);

                }



            //this.statsConsole.SetForeground(0, 0, Color.White);

            //! ENERGIA |
            //! IKON AZ ELEJÉN
            this.statsConsole.SetGlyph(1, 2, '2');

            if (playerEnergyCount > 12) { playerEnergyCount = 12; }
            if (playerEnergyCount == 12)
            {
                this.statsConsole.SetGlyph(2, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(3, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(4, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(5, 2, '>', Color.Green, Color.Black);
            }
            if (playerEnergyCount  ==11)
            {
                this.statsConsole.SetGlyph(2, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(3, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(4, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(5, 2, '>',Color.Orange,Color.Black);
            }
            if (playerEnergyCount == 10)
            {
                this.statsConsole.SetGlyph(2, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(3, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(4, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(5, 2, '>', Color.Red, Color.Black);
            }

            if (playerEnergyCount == 9)
            {
                this.statsConsole.SetGlyph(2, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(3, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(4, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(5, 2, '>', Color.Black, Color.Black);
            }
            if (playerEnergyCount == 8)
            {
                this.statsConsole.SetGlyph(2, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(3, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(4, 2, '>', Color.Orange, Color.Black);
                this.statsConsole.SetGlyph(5, 2, '>', Color.Black, Color.Black);
            }

            if (playerEnergyCount == 7)
            {
                this.statsConsole.SetGlyph(2, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(3, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(4, 2, '>', Color.Red, Color.Black);
                this.statsConsole.SetGlyph(5, 2, '>', Color.Black, Color.Black);
            }

            if (playerEnergyCount == 6)
            {
                this.statsConsole.SetGlyph(2, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(3, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(4, 2, '>', Color.Black, Color.Black);
                this.statsConsole.SetGlyph(5, 2, '>', Color.Black, Color.Black);
            }
            if (playerEnergyCount == 5)
            {
                this.statsConsole.SetGlyph(2, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(3, 2, '>', Color.Orange, Color.Black);
                this.statsConsole.SetGlyph(4, 2, '>', Color.Black, Color.Black);
                this.statsConsole.SetGlyph(5, 2, '>', Color.Black, Color.Black);
            }
            if (playerEnergyCount == 4)
            {
                this.statsConsole.SetGlyph(2, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(3, 2, '>', Color.Red, Color.Black);
                this.statsConsole.SetGlyph(4, 2, '>', Color.Black, Color.Black);
                this.statsConsole.SetGlyph(5, 2, '>', Color.Black, Color.Black);
            }
            if (playerEnergyCount == 3)
            {
                this.statsConsole.SetGlyph(2, 2, '>', Color.YellowGreen, Color.Black);
                this.statsConsole.SetGlyph(3, 2, '>', Color.Black, Color.Black);
                this.statsConsole.SetGlyph(4, 2, '>', Color.Black, Color.Black);
                this.statsConsole.SetGlyph(5, 2, '>', Color.Black, Color.Black);
            }
            if (playerEnergyCount == 2)
            {
                this.statsConsole.SetGlyph(2, 2, '>', Color.Orange, Color.Black);
                this.statsConsole.SetGlyph(3, 2, '>', Color.Black, Color.Black);
                this.statsConsole.SetGlyph(4, 2, '>', Color.Black, Color.Black);
                this.statsConsole.SetGlyph(5, 2, '>', Color.Black, Color.Black);
            }
            if (playerEnergyCount == 1)
            {
                this.statsConsole.SetGlyph(2, 2, '>', Color.Red, Color.Black);
                this.statsConsole.SetGlyph(3, 2, '>', Color.Black, Color.Black);
                this.statsConsole.SetGlyph(4, 2, '>', Color.Black, Color.Black);
                this.statsConsole.SetGlyph(5, 2, '>', Color.Black, Color.Black);
            }
            if (playerEnergyCount == 0)
            {
                this.statsConsole.SetGlyph(2, 2, '>', Color.Black, Color.Black);
                this.statsConsole.SetGlyph(3, 2, '>', Color.Black, Color.Black);
                this.statsConsole.SetGlyph(4, 2, '>', Color.Black, Color.Black);
                this.statsConsole.SetGlyph(5, 2, '>', Color.Black, Color.Black);
            }



            this.statsConsole.SetGlyph(2, 2, '>');
            this.statsConsole.SetGlyph(3, 2, '>');
            this.statsConsole.SetGlyph(4, 2, '>');
            this.statsConsole.SetGlyph(5, 2, '>');

        }

    }
}
