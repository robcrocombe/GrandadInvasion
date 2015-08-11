using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GranddadInvasionNS
{
    class EndGame
    {
        //Background
        private static SpriteObject gameOverLogoAndBackground;

        //Highscore Tiles
        private static SpriteObject highscore1;
        private static SpriteObject highscore2;
        private static SpriteObject highscore3;
        private static SpriteObject highscore4;

        private static SpriteObject noHighscore;

        //Highscore Settings
        private static string highscoreCongrats;
        private static string rank;
        public static bool highscoreSet;
        private static int highscorePosition;
        private static int playerScore;

        //Fonts
        private static SpriteFont tileFont;
        private static SpriteFont rankFont;
        private static SpriteFont highscoreCongratsFont;
        private static SpriteFont largeTileNumber;

        public static void LoadContent(ContentManager Content)
        {
            //Game over logo
            gameOverLogoAndBackground.spriteTexture = Content.Load<Texture2D>("EndGame/GameOverScreen");

            //Highscore Tiles
            highscore1.spriteTexture = Content.Load<Texture2D>("GameMenu/Icons/Highscores/Highscore1");
            highscore2.spriteTexture = Content.Load<Texture2D>("GameMenu/Icons/Highscores/Highscore2");
            highscore3.spriteTexture = Content.Load<Texture2D>("GameMenu/Icons/Highscores/Highscore3");
            highscore4.spriteTexture = Content.Load<Texture2D>("GameMenu/Icons/Highscores/Highscore4");

            noHighscore.spriteTexture = highscore2.spriteTexture;

            //Fonts
            tileFont = Content.Load<SpriteFont>("GameMenu/Fonts/tileFont");
            rankFont = Content.Load<SpriteFont>("EndGame/Fonts/rankFont");
            highscoreCongratsFont = Content.Load<SpriteFont>("EndGame/Fonts/highscoreCongratsFont");
            largeTileNumber = Content.Load<SpriteFont>("GameMenu/Fonts/largeTileNumber");
        }

        public static void Initialize()
        {
            //Logo and background
            gameOverLogoAndBackground = new SpriteObject(0, 0, 480, 800);

            //Highscore Tiles
            highscore1 = new SpriteObject(25, 350, 200, 200);
            highscore2 = new SpriteObject(245, 350, 200, 200);
            highscore3 = new SpriteObject(25, 575, 200, 200);
            highscore4 = new SpriteObject(245, 575, 200, 200);

            noHighscore = new SpriteObject(140, 390, 200, 200);

            //Highscore set
            highscoreSet = false;
        }

        public static void Update(int score)
        {
            if(highscoreSet == false)
            {
                playerScore = score;
                highscorePosition = HighscoreManager.addHighscore(score);
                switch (highscorePosition)
                {
                    case 0:
                        highscoreCongrats = "You didn't manage to get in the\nhighscores this time";
                        break;
                    case 1:
                        highscoreCongrats = "Best player ever.";
                        break;
                    case 2:
                        highscoreCongrats = "Not quite the best yet though";
                        break;
                    case 3:
                        highscoreCongrats = "Keep trying, you'll make it to the top";
                        break;
                    case 4:
                        highscoreCongrats = "Just made it to the highscores";
                        break;
                }
                if (score <= 50)
                {
                    rank = "Rubbish";
                }
                else
                {
                    if (score > 50 && score <= 100)
                    {
                        rank = "Granddad Pain Inflictor";
                    }
                    else
                    {
                        if (score > 100 && score <= 150)
                        {
                            rank = "Granddad Killer";
                        }
                        else
                        {
                            if (score > 150 && score <= 200)
                            {
                                rank = "Granddad Slayer";
                            }
                            else
                            {
                                if (score > 200)
                                {
                                    rank = "Granddad Annihilator";
                                }
                            }
                        }
                    }
                }
                highscoreSet = true;
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            //Logo and background
            sb.Draw(gameOverLogoAndBackground.spriteTexture, gameOverLogoAndBackground.spriteRectangle, Color.White);

            Vector2 rankPosition = getCentreWidthForText(rankFont, rank, 250);
            sb.DrawString(rankFont, rank, rankPosition, Color.White);

            Vector2 highscoreCongratsPosition = getCentreWidthForText(highscoreCongratsFont, highscoreCongrats, 310);
            sb.DrawString(highscoreCongratsFont, highscoreCongrats, highscoreCongratsPosition, Color.White);

            //Highscore boxes
            if (highscorePosition == 0)
            {
                noHighscore.Draw(sb);
                sb.DrawString(largeTileNumber, "X", new Vector2(200, 380), Color.White);
                sb.DrawString(tileFont, "You", new Vector2(165, 520), Color.White);
                sb.DrawString(tileFont, playerScore + " Kills", new Vector2(165, 545), Color.White);
            }
            else
            {
                highscore1.Draw(sb);
                highscore2.Draw(sb);
                highscore3.Draw(sb);
                highscore4.Draw(sb);

                sb.DrawString(tileFont, HighscoreManager.getHighscore(1).Name, new Vector2(45, 485), Color.White);
                sb.DrawString(tileFont, HighscoreManager.getHighscore(2).Name, new Vector2(265, 485), Color.White);
                sb.DrawString(tileFont, HighscoreManager.getHighscore(3).Name, new Vector2(45, 710), Color.White);
                sb.DrawString(tileFont, HighscoreManager.getHighscore(4).Name, new Vector2(265, 710), Color.White);

                sb.DrawString(tileFont, HighscoreManager.getHighscore(1).Score + "  Kills", new Vector2(45, 507), Color.White);
                sb.DrawString(tileFont, HighscoreManager.getHighscore(3).Score + "  Kills", new Vector2(265, 507), Color.White);
                sb.DrawString(tileFont, HighscoreManager.getHighscore(3).Score + "  Kills", new Vector2(45, 733), Color.White);
                sb.DrawString(tileFont, HighscoreManager.getHighscore(4).Score + "  Kills", new Vector2(265, 733), Color.White);
            }
        }
        private static string getNameOrYou(int highscoreNumber)
        {
            if (highscoreNumber == highscorePosition)
            {
                return "You";
            }
            else
            {
                return HighscoreManager.getHighscore(highscoreNumber).Name;
            }
        }
        private static Vector2 getCentreWidthForText(SpriteFont font, string text, int yCoordinate)
        {
            if (text == null)
            {
                text = "";
            }
            Vector2 textSize = font.MeasureString(text);
            Vector2 position = new Vector2((480 / 2) - (textSize.X / 2), yCoordinate);
            return position;
        }
    }
}
