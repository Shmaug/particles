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
        public float radius;
        private double m;
        public double mass
        {
            get
            {
                return this.m;
            }
            set
            {
                this.m = value;
                this.radius = (float)(this.m / 2);
            }
        }

        public Particle(Vector2 p, Vector2 v, double mass = 1)
        {
            this.position += p;
            this.velocity += v;
            Random r = new Random();
            this.color = new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            this.mass = mass;
        }

        private double calcGrav(double m2, Vector2 pos)
        {
            return Main.G * (m2 * this.mass) / Math.Pow(Vector2.Distance(pos, this.position), 2);
        }

        private void upd8(GameTime t)
        {
            Vector2 Fnet = Vector2.Zero;
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i] != null && particles[i] != this)
                {
                    double mg = this.calcGrav(particles[i].mass, particles[i].position);
                    Vector2 Fg = particles[i].position - this.position;
                    Fg.Normalize();
                    Fg *= (float)mg;
                    Fnet += Fg;
                }
            }
            // collisions
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i] != null && particles[i] != this)
                {
                    float dist = Vector2.Distance(particles[i].position, this.position);
                    float r = (particles[i].radius + this.radius) / 2;
                    if (dist < r)
                    {
                        double mg = this.calcGrav(particles[i].mass, particles[i].position);
                        Vector2 norm = this.position - particles[i].position;
                        norm.Normalize();
                        if (dist == r)
                            norm *= (float)mg;
                        else if (dist < r)
                            norm *= (float)(this.velocity.Length() * this.mass);
                        Fnet += norm;
                    }
                }
            }
            this.velocity += Fnet / (float)this.mass;
            this.position += (this.velocity * 100) * (float)t.ElapsedGameTime.TotalSeconds; // 100 px per meter. velocity in m/s
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
