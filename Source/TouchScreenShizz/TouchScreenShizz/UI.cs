using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Weapons;

//namespace UInamespace
//{
    public class UI
    {

        Texture2D UIgraphic;
        Rectangle UIREctangle;
        SpriteFont spriteFont;
        int score;
        int ammoTillReload;


        public void UIInitialise()
        {
            UIREctangle = new Rectangle(0, 702, 480, 100);

        }

        public void Load(ContentManager content)
        {
            UIgraphic = content.Load<Texture2D>("UI");
            spriteFont = content.Load<SpriteFont>("EndGame/Fonts/rankFont");
        }

        public void Update(GameTime gameTime)
        {
            score = ScoreManager.GetScore();
        }

        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            spritebatch.Draw(UIgraphic, UIREctangle, Color.White);
            spritebatch.DrawString(spriteFont, score.ToString(), new Vector2(400, 750), Color.White);
        }
    }
//}
