using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace particles
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        public const double G = 6.67e-2;

        public static Texture2D circleTexture;

        public static MouseState ms, lastms;

        public static Particle p;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            circleTexture = new Texture2D(GraphicsDevice, 512, 512);
            Color[] color = new Color[512 * 512];
            for (int x = 0; x < 512; x++)
            {
                for (int y = 0; y < 512; y++)
                {
                    float d = Vector2.Distance(new Vector2(256, 256), new Vector2(x, y));
                    if (d < 254)
                        color[x + y * 512] = Color.White;
                    else if (d < 256)
                        color[x + y * 512] = Color.Lerp(Color.White, Color.Transparent, (d-254) / 2);
                    else
                        color[x + y * 512] = Color.Transparent;
                }
            }
            circleTexture.SetData<Color>(color);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            ms = Mouse.GetState();

            if (ms.LeftButton == ButtonState.Pressed)
            {
                if (p == null)
                    p = new Particle(new Vector2(ms.X, ms.Y), Vector2.Zero, 100);
                if (ms.ScrollWheelValue > lastms.ScrollWheelValue)
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                        p.mass += 1;
                    else
                        p.mass += 5;
                else if (ms.ScrollWheelValue < lastms.ScrollWheelValue)
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                        p.mass -= 1;
                    else
                        p.mass -= 5;
                if (p.mass < 1)
                    p.mass = 1;
            }
            else
            {
                if (p != null)
                {
                    for (int i = 0; i < Particle.particles.Length; i++)
                    {
                        if (Particle.particles[i] == null)
                        {
                            Particle.particles[i] = p;
                            break;
                        }
                    }
                    p = null;
                }
            }

            Particle.Update(gameTime);

            lastms = ms;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if (p != null)
                spriteBatch.Draw(circleTexture,
                    new Rectangle((int)(p.position.X - p.radius / 2),
                                  (int)(p.position.Y - p.radius / 2),
                                  (int)(p.radius),
                                  (int)(p.radius)),
                    null,Color.White);
            
            for (int i = 0; i < Particle.particles.Length; i++)
            {
                if (Particle.particles[i] != null)
                {
                    spriteBatch.Draw(circleTexture,
                        new Rectangle((int)(Particle.particles[i].position.X - (Particle.particles[i].radius) / 2),
                                      (int)(Particle.particles[i].position.Y - (Particle.particles[i].radius) / 2),
                                      (int)(Particle.particles[i].radius),
                                      (int)(Particle.particles[i].radius)),
                        null, Particle.particles[i].color);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
