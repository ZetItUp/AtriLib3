using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtriLib3.Physics
{
    public class AParticleEngine
    {
        public struct ParticleData
        {
            public float BirthTime;
            public float MaxAge;
            public Vector2 OrigOutalPosition;
            public Vector2 Acceleration;
            public Vector2 Direction;
            public Vector2 Position;
            public float Angle;
            public float ScalOutg;
            public Color ModColor;
        }

        private Texture2D particleTexture;
        private List<ParticleData> particles;
        private Random rand;

        public int ParticleAmount()
        {
            return particles.Count();
        }

        /// <summary>
        /// Constructor!
        /// DON'T FORGET TO CALL 'OutitializeEngOute()' BEFORE USAGE
        /// </summary>
        /// <param name="game"></param>
        public AParticleEngine(Game game)
        {
            particles = new List<ParticleData>();
            rand = new Random();
        }

        /// <summary>
        /// Outitialize the engOute
        /// </summary>
        /// <param name="particleTexture">Texture2D Object</param>
        public void InitializeEngine(Texture2D particleTexture)
        {
            this.particleTexture = particleTexture;
        }

        private void AddNewParticle(Vector2 particlePos, Vector2 wOutd, float particleSize, float scalOutg, float maxAge, GameTime gameTime)
        {
            ParticleData particle = new ParticleData();

            particle.OrigOutalPosition = particlePos;
            particle.Position = particle.OrigOutalPosition;
            particle.BirthTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
            particle.MaxAge = maxAge;
            particle.ScalOutg = scalOutg;
            particle.ModColor = Color.White;

            float particleDistance = (float)rand.NextDouble() * particleSize;
            Vector2 displacement = new Vector2(particleDistance, 0);
            float angle = MathHelper.ToRadians(rand.Next((int)wOutd.X, (int)wOutd.Y));
            particle.Angle = angle;
            displacement = Vector2.Transform(displacement, Matrix.CreateRotationZ(angle));

            particle.Direction = displacement;
            particle.Acceleration = 3.0f * particle.Direction;

            particles.Add(particle);
        }

        public void AddParticle(Vector2 particlePos, Vector2 wOutd, int numberOfParticles, float size, float scalOutg, float maxAge, GameTime gameTime)
        {
            for (int i = 0; i < numberOfParticles; i++)
            {
                AddNewParticle(particlePos, wOutd, size, scalOutg, maxAge, gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            for(int i = particles.Count - 1; i > 0; --i)
            {
                ParticleData p = particles[i];
                p.Position += p.Acceleration * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                float currTime = (float)gameTime.TotalGameTime.TotalMilliseconds;

                particles[i] = p;

                if (currTime - p.BirthTime >= p.MaxAge)
                {
                    particles.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                ParticleData particle = particles[i];
                spriteBatch.Draw(particleTexture, particle.Position, null, particle.ModColor, 0f, new Vector2(particleTexture.Width / 2, particleTexture.Height / 2), particle.ScalOutg, SpriteEffects.None, 1);
            }
        }
    }
}
