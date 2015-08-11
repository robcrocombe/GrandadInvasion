using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GranddadInvasionNS
{
    class Wire
    {
        private Texture2D tex;
        private Rectangle bounds;
        private Vector2 pos;
        private float alpha = 255;

        public void Init(Vector2 p)
        {
            this.pos = p;
            bounds = new Rectangle((int)this.pos.X, (int)this.pos.Y, 480, 50);
        }
        public void Load(ContentManager content)
        {
            this.tex = content.Load<Texture2D>("Wire1");
        }
        public void Update(List<Enemy> e, List<Wire> w)
        {
            if (e[0].getBound().Intersects(this.bounds))
            {
                if (GrandadInvasionGame.wireCount == 0)
                {
                    w.RemoveAt(w.Count - 1);
                    e.RemoveAt(0);
                    GrandadInvasionGame.wireCount++;
                }
                else
                {
                    e.RemoveAt(0);
                }
            }
        }
        public void updateCount()
        {
            if (GrandadInvasionGame.wireCount != 0)
            {
                GrandadInvasionGame.wireCount++;
            }
            if (GrandadInvasionGame.wireCount >= 60)
            {
                GrandadInvasionGame.wireCount = 0;
            }
        }
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(tex, bounds, new Color(255, 255, 255, alpha));
        }
    }
}
