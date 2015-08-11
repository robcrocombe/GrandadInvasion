using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GranddadInvasionNS
{
    public class Particle
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity;
        public float angle;
        public float angularVelocity;
        public Color color;
        public float size;
        public int ttl;
        public float alpha;

        public Particle(Texture2D tex, Vector2 pos, Vector2 velo, float ang, float angVel, Color col, float siz, int tt)
        {
            texture = tex;
            position = pos;
            velocity = velo;
            angle = ang;
            angularVelocity = angVel;
            color = col;
            size = siz;
            ttl = tt;
         //   alpha = 0;
        }
        public void Update()
        {
            ttl--;
          //  alpha *= -1;
            position += velocity;
            angle += angularVelocity;
            MathHelper.Clamp(color.R -= 5, 0, 255);
        }
        public void Draw(SpriteBatch batch)
        {
            Rectangle sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            batch.Draw(texture, position, sourceRect, color, angle, origin, size, SpriteEffects.None, 0f);
        }
    }

    public class ParticleEngine
    {
        private Random rand;
        public Vector2 location;
        private List<Particle> particles;
        private Texture2D[] texture;

        public ParticleEngine(Texture2D[] tex, Vector2 loc)
        {
            location = loc;
            this.texture = tex;
            this.particles = new List<Particle>();
            rand = new Random();
        }

        private Particle NewParticle()
        {
            Texture2D tex = texture[rand.Next(2)];
            Vector2 position = location;
            Vector2 velocity = new Vector2(1f * (float)(rand.NextDouble() * 2 - 1),
                1f * (float)(rand.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVel = 0.1f * (float)(rand.NextDouble() * 2 - 1);
            Color col = new Color((float)rand.NextDouble(), 0, 0);
            float size = (float)rand.NextDouble();
            int ttl = 100 + rand.Next(100);

            return new Particle(tex, position, velocity, angle, angularVel, col, size, ttl);
        }

        public void Update()
        {
            int total = 10;
            if (location.X != -10)
            {
                for (int i = 0; i < total; i++)
                {
                    this.particles.Add(NewParticle());
                }
            }
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].ttl <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
          //  int count = particles.Count;
          //  if (particles.Count > 0)
          //  {
            foreach (Particle p in particles)
            {
                batch.Draw(p.texture, p.position, p.color);
            }
         //   }
        }
    }
}
