using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace particles
{
    public class Particle
    {
        public static Particle[] particles = new Particle[4096];
        
        public Vector2 position;
        public Vector2 velocity;
        public Color color;
        public double mass;

        public Particle(Vector2 p, Vector2 v, double mass = 1)
        {
            this.position += p;
            this.velocity += v;
            Random r = new Random();
            this.color = new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            this.mass = 1;
        }

        private void upd8(GameTime t)
        {
            Vector2 Fnet = Vector2.Zero;
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i] != null && particles[i] != this)
                {
                    double mg = Main.G * (particles[i].mass * this.mass) / Math.Pow(Vector2.Distance(particles[i].position, this.position), 2);
                    Vector2 Fg = particles[i].position - this.position;
                    Fg.Normalize();
                    Fg *= (float)mg;
                    Fnet += Fg;
                }
            }
            this.velocity += Fnet / (float)this.mass;
            this.position += this.velocity * 100 * (float)t.ElapsedGameTime.TotalMilliseconds;

            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i] != null && particles[i] != this)
                {
                    double mg = Main.G * (particles[i].mass * this.mass) / Math.Pow(Vector2.Distance(particles[i].position, this.position), 2);
                    Vector2 Fg = particles[i].position - this.position;
                    Fg.Normalize();
                    Fg *= (float)mg;
                    Fnet += Fg;
                }
            }
        }

        public static void Update(GameTime t)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i] != null)
                {
                    particles[i].upd8(t);
                }
            }
        }
    }
}
