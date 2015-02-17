using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace WindowsGame1
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Megaman player;
        int nextSpawnTime, spawnMin = 500, spawnMax = 1000, minSpeed = 6, maxSpeed = 10;
        List<Sprite> spriteList = new List<Sprite>();
        List<Bullet> bullets = new List<Bullet>();
        Texture2D bulletTexture, enemyTexture;

        SoundEffect shootEffect, zHitEffect;
        List<SoundEffect> hitEffects = new List<SoundEffect>();
        Song song;

        double multiplier = 1;

        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            ResetSpawnTime();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            bulletTexture = Game.Content.Load<Texture2D>("fireball");
            player = new Megaman(Game.Content.Load<Texture2D>("runshoot"), new Vector2(0,Game.GraphicsDevice.PresentationParameters.BackBufferHeight/2-75), new Point(105, 110), 2, new Point(0, 0),
                new Point(4, 1), new Vector2(0, 3), 100, bulletTexture,this);
            shootEffect = Game.Content.Load<SoundEffect>("gun_rifle_02");
            zHitEffect = Game.Content.Load<SoundEffect>("mutantdie");
            hitEffects.Add(Game.Content.Load<SoundEffect>("pain_jack_01"));
            hitEffects.Add(Game.Content.Load<SoundEffect>("pain_jack_02"));
            hitEffects.Add(Game.Content.Load<SoundEffect>("pain_jack_03"));
            song = Game.Content.Load<Song>("nbinstrumental");

            MediaPlayer.Volume = 0.4f;
            MediaPlayer.Play(song);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
            if (nextSpawnTime < 0)
            {
                SpawnEnemy();
                ResetSpawnTime();
            }
            if (gameTime.TotalGameTime.TotalMilliseconds >= 10000)
            {
                minSpeed = 9;
                maxSpeed = 12;
                multiplier = 2;
            }
            if (gameTime.TotalGameTime.TotalMilliseconds >= 15000)
            {
                minSpeed = 12;
                maxSpeed = 16;
                spawnMin = 450;
                spawnMax = 900;
                multiplier = 4;
            }
            if (gameTime.TotalGameTime.TotalMilliseconds >= 20000)
            {
                minSpeed = 14;
                maxSpeed = 19;
                spawnMin = 400;
                spawnMax = 850;
                multiplier = 8;
            }

            player.Update(gameTime, new Rectangle(0, 0, 800, 600));

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                    bullets.Add(new Bullet(bulletTexture, new Vector2(player.Position.X, player.Position.Y), new Vector2(2, 0),
                        new Point(40, 40), new Point(0, 0),new Point(5, 0), 30, 10));
                    //shootEffect.Play(0.1f, 0.5f, 0.0f);
            }
            try
            {
                foreach (Bullet b in bullets)
                {
                    b.Update(gameTime, new Rectangle(0,0,800,600));
                    if (b.getPosition.X > 800)
                    {
                        bullets.Remove(b);
                    }
                }
            }
            catch (InvalidOperationException e) { }

            for (int i = 0; i < spriteList.Count; i++)
            {
                try
                {
                    Sprite s = spriteList[i];
                    s.Update(gameTime, new Rectangle(0, 0, 800, 600));

                    if (s.isOutOfBounds())
                    {
                        ((Game1)Game).ZombieLet();
                        spriteList.RemoveAt(i);
                        --i;
                    }

                    if (player.CollisionRectangle.Intersects(s.CollisionRectangle))
                    {
                        ((Game1)Game).AddHit();
                        ((Game1)Game).AddScore(-50*multiplier);
                        SoundEffect hF = hitEffects[((Game1)Game).r.Next(0, 3)];
                        hF.Play(0.4f, 0.0f, 0.0f);
                        spriteList.RemoveAt(i);
                        --i;
                    }
                    for (int j = 0; j < bullets.Count; j++)
                    {
                        Bullet b = bullets[j];
                        if (b.CollisionRectangle.Intersects(s.CollisionRectangle))
                        {
                            ((Game1)Game).AddScore(100*multiplier);
                            try
                            {
                                spriteList.RemoveAt(i);
                            }
                            catch (ArgumentOutOfRangeException e) { }
                            --i;
                            bullets.RemoveAt(j);
                            --j;
                            zHitEffect.Play(0.4f, 0.0f, 0.0f);
                        }
                    }
                }
                catch (ArgumentOutOfRangeException e) { }
            }
            for (int i = 1; i < bullets.Count; i++)
            {
                bullets.RemoveAt(i - 1);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            player.Draw(gameTime, spriteBatch);
            foreach (Sprite s in spriteList)
            {
                s.Draw(gameTime, spriteBatch);
            }

            foreach (Bullet b in bullets)
            {
                b.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetSpawnTime()
        {
            nextSpawnTime = ((Game1)Game).r.Next(spawnMin, spawnMax);
        }

        private void SpawnEnemy()
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = Vector2.Zero;

            Point frameSize = new Point(128, 91);
            enemyTexture=(Game.Content.Load<Texture2D>("enemysprite"));

            position = new Vector2(Game.GraphicsDevice.PresentationParameters.BackBufferWidth, ((Game1)Game).r.Next(0,
                Game.GraphicsDevice.PresentationParameters.BackBufferHeight-91));
            speed = new Vector2(-((Game1)Game).r.Next(minSpeed, maxSpeed),0);

            spriteList.Add(new Enemy(enemyTexture, position, frameSize, 1, new Point(0, 0),
                new Point(4, 1), speed, 70));
        }

    }
}
