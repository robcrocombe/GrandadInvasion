using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System.IO.IsolatedStorage;

namespace GranddadInvasionNS
{
    public class GameMenu
    {
        //Background
        private static SpriteObject background;

        //Logo
        private static SpriteObject[] logo;
        private static int frame = 0;
        private static float frameRate = 80.0f;
        private static double frameCountdown;
        private static bool end;

        //Gas
        private static SpriteObject[] backgroundGas;
        private static int currentBackgroundGas;

        //Main Menu
        private static MenuButton playGame;
        private static MenuButton highscores;
        private static MenuButton about;
        private static MenuButton settings;

        //Fonts
        private static SpriteFont tileFont;
        private static SpriteFont headerFont;
        private static SpriteFont largeTileNumber;

        //Highscores
        private static SpriteObject highscore1;
        private static SpriteObject highscore2;
        private static SpriteObject highscore3;
        private static SpriteObject highscore4;

        //On Off Sliders
        private static OnOffSlider soundEffectsSlider;
        private static OnOffSlider granddadDeathSoundsSlider;
        private static OnOffSlider vibrationSlider;
        private static OnOffSlider confettiSlider;
        
        //On off slider time delay
        private static int countdown = 0;

        //Intro
        private static SpriteObject introExplination;

        //Sounds
        private static SoundEffect breyshawIntro;

        public static void Initialize()
        {
            //Create Background Sprite
            background = new SpriteObject(0, 0, 480, 800);

            //Create Logo Sprite
            logo = new SpriteObject[7];
            end = false;
            for (int i = 0; i < logo.Length; i++)
            {
                logo[i] = new SpriteObject(0, 0, 480, 800);
            }

            //Gas
            backgroundGas = new SpriteObject[4];
            backgroundGas[0] = new SpriteObject(0, 420, 854, 322);
            backgroundGas[1] = new SpriteObject(481, 120, 866, 566);
            backgroundGas[2] = new SpriteObject(481, 200, 822, 484);
            backgroundGas[3] = new SpriteObject(481, 320, 930, 292);

            //Highscore Tiles
            highscore1 = new SpriteObject(25, 350, 200, 200);
            highscore2 = new SpriteObject(245, 350, 200, 200);
            highscore3 = new SpriteObject(25, 575, 200, 200);
            highscore4 = new SpriteObject(245, 575, 200, 200);
            
            //Set Up

            IsolatedStorageSystem.Initalize();

            if (!IsolatedStorageSystem.CheckFileExist("highscores.xml"))
            {
                HighscoreManager.setUpHighscores();
            }


            //Settings

            if (!IsolatedStorageSystem.CheckFileExist("settings.xml"))
            {
                SettingManager.setUpSettings();
            }

            //Sliders
            soundEffectsSlider = new OnOffSlider(new SpriteObject(300, 320, 165, 61), SettingManager.getSetting(Setting.SoundEffects));
            granddadDeathSoundsSlider = new OnOffSlider(new SpriteObject(300, 400, 165, 61), SettingManager.getSetting(Setting.GranddadDeathSounds));
            vibrationSlider = new OnOffSlider(new SpriteObject(300, 480, 165, 61), SettingManager.getSetting(Setting.Vibration));
            confettiSlider = new OnOffSlider(new SpriteObject(300, 560, 165, 61), SettingManager.getSetting(Setting.Confetti));

            //Intro
            introExplination = new SpriteObject(0, 801, 480, 800);
        }

        public static void LoadContent(ContentManager Content)
        {
            //Load Background
            background.spriteTexture = Content.Load<Texture2D>("GameMenu/Background");

            //Load Logo
            for (int i = 0; i < 7; i++)
            {
                logo[i].spriteTexture = Content.Load<Texture2D>("GameMenu/Logo/F" + (i + 1));
            }

            for (int i = 0; i < 4; i++)
            {
                backgroundGas[i].spriteTexture = Content.Load<Texture2D>("GameMenu/Gas/Gas" + (i + 1));
            }

            //Load Icons
            MenuButton.playIcon = Content.Load<Texture2D>("GameMenu/Icons/PlayGame");
            MenuButton.highscoreIcon = Content.Load<Texture2D>("GameMenu/Icons/Highscores");
            MenuButton.aboutIcon = Content.Load<Texture2D>("GameMenu/Icons/About");
            MenuButton.settingIcon = Content.Load<Texture2D>("GameMenu/Icons/Settings");

            //Create Tiles
            playGame = new MenuButton(ButtonType.play, new SpriteObject(30, 256, 195, 197));
            highscores = new MenuButton(ButtonType.highscores, new SpriteObject(250, 256, 194, 195));
            settings = new MenuButton(ButtonType.setting, new SpriteObject(30, 470, 191, 193));
            about = new MenuButton(ButtonType.about, new SpriteObject(250, 470, 199, 192));

            //Load fonts
            tileFont = Content.Load<SpriteFont>("GameMenu/Fonts/tileFont");
            headerFont = Content.Load<SpriteFont>("GameMenu/Fonts/headerFont");
            largeTileNumber = Content.Load<SpriteFont>("GameMenu/Fonts/largeTileNumber");

            //Load Highscore Tiles
            highscore1.spriteTexture = Content.Load<Texture2D>("GameMenu/Icons/Highscores/Highscore1");
            highscore2.spriteTexture = Content.Load<Texture2D>("GameMenu/Icons/Highscores/Highscore2");
            highscore3.spriteTexture = Content.Load<Texture2D>("GameMenu/Icons/Highscores/Highscore3");
            highscore4.spriteTexture = Content.Load<Texture2D>("GameMenu/Icons/Highscores/Highscore4");

            //Sliders
            OnOffSlider.offImage = Content.Load<Texture2D>("GameMenu/Slider/On");
            OnOffSlider.onImage = Content.Load<Texture2D>("GameMenu/Slider/Off");

            //Intro Explination
            introExplination.spriteTexture = Content.Load<Texture2D>("GameMenu/Intro/Explination");

            //Sound
            breyshawIntro = Content.Load<SoundEffect>("Sounds/BrayshawIntro");
        }

        public static void Update(ref GameState gameState, ContentManager Content)
        {
            if (countdown != 0)
            {
                --countdown;
            }
            if (gameState == GameState.menu)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    //Exit App
                }
                if (playGame.IsTapped())
                {
                    gameState = GameState.intro;
                }
                if (highscores.IsTapped())
                {
                    gameState = GameState.highscores;
                }
                if (settings.IsTapped())
                {
                    gameState = GameState.settings;
                }
                if (about.IsTapped())
                {
                    gameState = GameState.about;
                }

                //Animate Gas, innit
                animateGasRightToLeft(backgroundGas, ref currentBackgroundGas, 3);
            }

            if (gameState == GameState.about || gameState == GameState.game || gameState == GameState.highscores || gameState == GameState.settings)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    gameState = GameState.menu;
                }
                animateGasRightToLeft(backgroundGas, ref currentBackgroundGas, 3);
            }
            if (gameState == GameState.settings)
            {
                if(soundEffectsSlider.IsTapped() && countdown == 0)
                {
                    SettingManager.changeSetting(Setting.SoundEffects);
                    soundEffectsSlider.switchChoice();
                    countdown = 6;
                }
                if (granddadDeathSoundsSlider.IsTapped() && countdown == 0)
                {
                    SettingManager.changeSetting(Setting.GranddadDeathSounds);
                    granddadDeathSoundsSlider.switchChoice();
                    countdown = 6;
                }
                if (vibrationSlider.IsTapped() && countdown == 0)
                {
                    SettingManager.changeSetting(Setting.Vibration);
                    vibrationSlider.switchChoice();
                    countdown = 6;
                }
                if (confettiSlider.IsTapped() && countdown == 0)
                {
                    SettingManager.changeSetting(Setting.Confetti);
                    confettiSlider.switchChoice();
                    countdown = 6;
                }
            }
            if(gameState == GameState.intro)
            {
                TouchCollection tc = TouchPanel.GetState();
                if(tc.Count >= 2)
                {
                    ScoreManager.ResetScore();
                    GrandadInvasionGame.Init(Content);
                    gameState = GameState.game;
                }
                if(introExplination.spriteRectangle.Y < -690)
                {
                    breyshawIntro.Play();
                    ScoreManager.ResetScore();
                    GrandadInvasionGame.Init(Content);
                    gameState = GameState.game;
                }
                introExplination.spriteRectangle.Y = introExplination.spriteRectangle.Y - 3;
            }
        }

        public static void Draw(SpriteBatch sb, GameState gameState, GameTime gameTime)
        {
            //Background, Logo & Gas for all the things!
            frameCountdown -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (frame == 6)
            {
                end = true;
            }
            if (frame == 0 && end == true)
            {
                end = false;
            }
            if ((frameCountdown <= 0.0f) && (frame < logo.Length))
            {
                if (end == false)
                {
                    frame++;
                }
                else
                {
                    frame--;
                }
                frameCountdown = frameRate;
            }

            if (gameState == GameState.menu || gameState == GameState.highscores || gameState == GameState.about || gameState == GameState.settings)
            {
                sb.Draw(background.spriteTexture, background.spriteRectangle, Color.White);

                sb.Draw(logo[frame].spriteTexture, logo[frame].spriteRectangle, Color.White);

                backgroundGas[currentBackgroundGas].Draw(sb);
            }

            if (gameState == GameState.menu)
            {
                sb.Draw(playGame.background.spriteTexture, playGame.background.spriteRectangle, Color.White);
                sb.Draw(highscores.background.spriteTexture, highscores.background.spriteRectangle, Color.White);
                sb.Draw(settings.background.spriteTexture, settings.background.spriteRectangle, Color.White);
                sb.Draw(about.background.spriteTexture, about.background.spriteRectangle, Color.White);

                sb.DrawString(tileFont, "Play Game", new Vector2(45, 405), Color.White);
                sb.DrawString(tileFont, "Highscores", new Vector2(265, 405), Color.White);
                sb.DrawString(tileFont, "Settings", new Vector2(45, 620), Color.White);
                sb.DrawString(tileFont, "About", new Vector2(265, 620), Color.White);
            }

            if (gameState == GameState.about)
            {
                sb.DrawString(headerFont, "about", new Vector2(30, 250), Color.White);
                sb.DrawString(tileFont, "Lovingly produced by: \n \nDanny Brown \nNick Case\nRob Crocombe\nSean Heampstead", new Vector2(30, 320), Color.White);
            }
            if (gameState == GameState.highscores)
            {
                sb.DrawString(headerFont, "highscores", new Vector2(30, 250), Color.White);
                sb.DrawString(tileFont, "Congratulations to the high scorers!", new Vector2(30, 320), Color.White);

                highscore1.Draw(sb);
                highscore2.Draw(sb);
                highscore3.Draw(sb);
                highscore4.Draw(sb);

                sb.DrawString(largeTileNumber, "2", new Vector2(310, 350), Color.White);
                sb.DrawString(largeTileNumber, "3", new Vector2(90, 575), Color.White);
                sb.DrawString(largeTileNumber, "4", new Vector2(310, 575), Color.White);

                sb.DrawString(tileFont, HighscoreManager.getHighscore(1).Name, new Vector2(45, 485), Color.White);
                sb.DrawString(tileFont, HighscoreManager.getHighscore(2).Name, new Vector2(265, 485), Color.White);
                sb.DrawString(tileFont, HighscoreManager.getHighscore(3).Name, new Vector2(45, 710), Color.White);
                sb.DrawString(tileFont, HighscoreManager.getHighscore(4).Name, new Vector2(265, 710), Color.White);

                sb.DrawString(tileFont, HighscoreManager.getHighscore(1).Score + "  Kills", new Vector2(45, 507), Color.White);
                sb.DrawString(tileFont, HighscoreManager.getHighscore(3).Score + "  Kills", new Vector2(265, 507), Color.White);
                sb.DrawString(tileFont, HighscoreManager.getHighscore(3).Score + "  Kills", new Vector2(45, 733), Color.White);
                sb.DrawString(tileFont, HighscoreManager.getHighscore(4).Score + "  Kills", new Vector2(265, 733), Color.White);
            }
            if(gameState == GameState.settings)
            {
                sb.DrawString(headerFont, "settings", new Vector2(30, 250), Color.White);
                sb.DrawString(tileFont, "Sound Effects", new Vector2(30, 330), Color.White);
                sb.DrawString(tileFont, "Granddad Death sounds", new Vector2(30, 410), Color.White);
                sb.DrawString(tileFont, "Vibration", new Vector2(30, 490), Color.White);
                sb.DrawString(tileFont, "Confetti Blood", new Vector2(30, 570), Color.White);

                soundEffectsSlider.Draw(sb);
                granddadDeathSoundsSlider.Draw(sb);
                vibrationSlider.Draw(sb);
                confettiSlider.Draw(sb);
            }
            if (gameState == GameState.intro)
            {
                sb.Draw(background.spriteTexture, background.spriteRectangle, Color.White);
                sb.Draw(introExplination.spriteTexture, introExplination.spriteRectangle, Color.White);
            }
        }

        private static void animateGasRightToLeft(SpriteObject[] gas, ref int currentGas, int speed)
        {
            gas[currentGas].spriteRectangle.X = gas[currentGas].spriteRectangle.X - speed;

            if (-gas[currentGas].spriteRectangle.X > gas[currentGas].spriteRectangle.Width)
            {
                //Gas is completely off the screen
                //Put it back it its place just off the screen
                gas[currentGas].spriteRectangle.X = 485;
                //Grab the next gas
                currentGas++;

                if (currentGas > (gas.Length - 1))
                {
                    //If we've used all the gases, go back to the first one
                    currentGas = 0;
                }
            }
        }
    }
    enum ButtonType
    {
        play,
        setting,
        about,
        highscores
    }
    class MenuButton
    {
        public SpriteObject background;

        public bool IsTapped()
        {
            TouchCollection touches = TouchPanel.GetState();

            for (int i = 0; i < touches.Count; i++)
            {
                Rectangle fingerPosition = new Rectangle((int)touches[i].Position.X, (int)touches[i].Position.Y, 1, 1);
                if (fingerPosition.Intersects(this.background.spriteRectangle))
                {
                    return true;
                }
            }
            return false;
        }

        public MenuButton(ButtonType bt, SpriteObject backgroundReq)
        {
            background = backgroundReq;

            switch (bt)
            {
                case ButtonType.play:
                    background.spriteTexture = playIcon;
                    break;
                case ButtonType.about:
                    background.spriteTexture = aboutIcon;
                    break;
                case ButtonType.highscores:
                    background.spriteTexture = highscoreIcon;
                    break;
                case ButtonType.setting:
                    background.spriteTexture = settingIcon;
                    break;
            }
        }

        public static Texture2D playIcon;
        public static Texture2D settingIcon;
        public static Texture2D aboutIcon;
        public static Texture2D highscoreIcon;
    }
    public enum GameState
    {
        menu,
        game,
        highscores,
        about,
        settings,
        intro,
        endgame
    }
    public class SpriteObject
    {
        public Texture2D spriteTexture;
        public Rectangle spriteRectangle;

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteTexture, spriteRectangle, Color.White);
        }
        public void Draw(SpriteBatch sb, Color colour)
        {
            sb.Draw(spriteTexture, spriteRectangle, colour);
        }
        public SpriteObject(int x, int y, int width, int height)
        {
            spriteRectangle = new Rectangle(x, y, width, height);
        }
    }
}
