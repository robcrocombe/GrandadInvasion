using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;

namespace GranddadInvasionNS
{
    class Enemies
    {


        private const int GROWFACTOR = 2;
        private const float SPEED = 0.5f;
        private const float FRAMERATE = 200.0f;

        private List<Enemies> e = new List<Enemies>();

        private Vector2 pos;
        private Rectangle bounds;
        private int Xcount = 0, Ycount = 0;
        private int health = 100;

        private int frame = 0;
        private double frameCountdown;

        private int newTarget;
        private int newCount;
        public Texture2D[] images = new Texture2D[6];

        public void Init()
        {
            this.newTarget = GrandadInvasionGame.getRandom(10000, 50000);
            this.newCount = 0;
            Enemies temp = new Enemies();
            temp.pos = new Vector2(GrandadInvasionGame.getRandom(0, 325), -40);
            temp.bounds = new Rectangle((int)this.pos.X, (int)this.pos.Y, 20, 40);
            temp.health = 100;
            e.Add(temp);
            frameCountdown = FRAMERATE;
        }

        public void Load(ContentManager content)
        {
            images[0] = content.Load<Texture2D>("player/T1");
            images[1] = content.Load<Texture2D>("player/T2");
            images[2] = content.Load<Texture2D>("player/T3");
            images[3] = content.Load<Texture2D>("player/T4");
            images[4] = content.Load<Texture2D>("player/T5");
            images[5] = content.Load<Texture2D>("player/T6");
        }

        public void Update()
        {
            this.checkNew();
            foreach (Enemies en in e)
            {
                Movement(en);
            }
        }

        public void Draw(SpriteBatch batch, GameTime gameTime)
        {
            for (int i = e.Count - 1; i > -1; i--)
            {
                frameCountdown -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frame == 5)
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
        }



        public void checkNew()
        {
            if (e.Count < 10)
            {
                this.newCount += GrandadInvasionGame.getRandom(0, 1000);
                if (this.newCount >= this.newTarget)
                {
                    this.newCount = 0;
                    this.newTarget = GrandadInvasionGame.getRandom(1000, 5000);
                    addEnemy();
                }
            }
        }
        private void addEnemy()
        {
            Enemies temp = new Enemies();
            temp.Init();
            e.Add(temp);
        }
        private void Movement(Enemies e)
        {
            if (e.Xcount == 4)
            {
                e.bounds.Width += GROWFACTOR;
                e.Xcount = 0;
            }
            else
            {
                e.Xcount++;
            }
            if (e.Ycount == 2)
            {
                e.bounds.Height += GROWFACTOR;
                e.Ycount = 0;
            }
            else
            {
                e.Ycount++;
            }
            e.pos.Y += SPEED;
            e.bounds.Y = (int)pos.Y;
        }

    }
}
