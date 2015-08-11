using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;


namespace GranddadInvasionNS
{
    class Enemy
    {
        private const int GROWFACTOR = 1;
        private const float SPEED = 0.25f;
        private const float FRAMERATE = 100.0f;

        private static Texture2D[] images = new Texture2D[10];
        private static Texture2D touch;

        private ParticleEngine bloodEngine;

        private Vector2 pos;

        private Rectangle bounds;
        private Rectangle headBox;

        private int Xcount = 0, Ycount = 0;
        private int health;
        private int frame = 0;
        private double frameCountdown;

        private int target, seconds;

        public Rectangle getBound()
        {
            return bounds;
        }
        public Rectangle getHead()
        {
            return headBox;
        }
        public void setBound(Rectangle b)
        {
            bounds = b;
        }
        public int getHealth()
        {
            return health;
        }
        public void setHealth(int h)
        {
            health = h;
        }
        public Texture2D getTex()
        {
            return images[frame];
        }

        public void Init()
        {
            this.pos = new Vector2(GrandadInvasionGame.getRandom(0, 325), -40);
            this.bounds = new Rectangle((int)this.pos.X, (int)this.pos.Y, 20, 40);
            this.headBox = new Rectangle((int)(this.pos.X + 7.25), (int)(this.pos.Y + 1), 5, 5);
            this.health = 100;
            frameCountdown = FRAMERATE;
            this.target = GrandadInvasionGame.getRandom(10000, 50000);
            this.seconds = 0;
        }

        public void Load(ContentManager content)
        {
            touch = content.Load<Texture2D>("touch");
            images[0] = content.Load<Texture2D>("player/G1");
            images[1] = content.Load<Texture2D>("player/G2");
            images[2] = content.Load<Texture2D>("player/G3");
            images[3] = content.Load<Texture2D>("player/G4");
            images[4] = content.Load<Texture2D>("player/G5");
            images[5] = content.Load<Texture2D>("player/G6");
            images[6] = content.Load<Texture2D>("player/G7");
            images[7] = content.Load<Texture2D>("player/G8");
            images[8] = content.Load<Texture2D>("player/G9");
            images[9] = content.Load<Texture2D>("player/G10");
        }

        public void Update(GameDifficulty d, List<Enemy> enemy, GameTime gameTime, ContentManager Content, ParticleEngine engine)
        {
            engine.location = new Vector2(-10, -10);
            checkNewEnemy(gameTime, enemy, Content);
            updateEnemies(enemy, d, engine);
            engine.Update();
        }

        public void Draw(SpriteBatch batch, GameTime gameTime)
        {
            frameCountdown -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (frame == 9)
            {
                frame = 0;
            }
            if ((frameCountdown <= 0.0f) && (frame < images.Length))
            {
                frame++;
                frameCountdown = FRAMERATE;
            }
            batch.Draw(images[frame], bounds, Color.White);
        }


        private void checkNewEnemy(GameTime gameTime, List<Enemy> e, ContentManager content)
        {
            if (e.Count < 10)
            {
                seconds += GrandadInvasionGame.getRandom(100, 1300);
                if (seconds >= target)
                {
                    target = GrandadInvasionGame.getRandom(10000, 50000);
                    seconds = 0;
                    addEnemy(e, content);
                }
            }
        }
        private void updateEnemies(List<Enemy> enemy, GameDifficulty d, ParticleEngine engine)
        {
            List<Rectangle> eBoundList = new List<Rectangle>();
            List<Rectangle> eHeadList = new List<Rectangle>();
            List<Texture2D> colorArray = new List<Texture2D>();
            foreach (Enemy e in enemy)
            {
                eBoundList.Add(e.getBound());
                eHeadList.Add(e.getHead());
                colorArray.Add(e.getTex());
                updateMovement();
            }
            string type;
            int intersect = touchScreen.Screen.checkTouch(eHeadList, eBoundList, out type);
            if (intersect != -10)
            {
                if (type == "head")
                {
                    engine.location = new Vector2((int)enemy[intersect].getHead().X + 10, (int)enemy[intersect].getHead().Y);
                    enemy.RemoveAt(intersect);
                    eHeadList.RemoveAt(intersect);
                    ScoreManager.Add(100);
                }
                else
                {
                    engine.location = new Vector2(-10, -10);
                    int health = enemy[intersect].getHealth();
                    health = takeHealth(d);
                    if (health <= 0)
                    {
                        enemy.RemoveAt(intersect);
                        eBoundList.RemoveAt(intersect);
                        ScoreManager.Add(50);
                    }
                    else
                    {
                        enemy[intersect].setHealth(health);
                    }
                }
            }
        }

        private void updateMovement()
        {
            if (Xcount == 4)
            {
                this.bounds.Width += GROWFACTOR;
                this.headBox.Height += GROWFACTOR;
                this.headBox.Width += GROWFACTOR;
                Xcount = 0;
            }
            else
            {
                Xcount++;
            }
            if (Ycount == 2)
            {
                this.bounds.Height += GROWFACTOR;
                Ycount = 0;
            }
            else
            {
                Ycount++;
            }
            this.pos.Y += SPEED;
            this.bounds.Y = (int)this.pos.Y;
            this.headBox.Y = (int)this.pos.Y;
        }
        private int takeHealth(GameDifficulty d)
        {
            switch (d)
            {
                case GameDifficulty.Easy:
                    {
                        health -= 100;
                        return health;
                    }
                case GameDifficulty.Normal:
                    {
                        health -= 50;
                        return health;
                    }
                case GameDifficulty.Hard:
                    {
                        health -= 25;
                        return health;
                    }
            }
            return -10;

        }
        private void addEnemy(List<Enemy> enemy, ContentManager Content)
        {
            Enemy temp = new Enemy();
            temp.Init();
            temp.Load(Content);
            enemy.Add(temp);
        }
    }
}
