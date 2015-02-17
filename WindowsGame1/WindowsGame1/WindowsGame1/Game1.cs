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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        SpriteManager spriteManager;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle client;
        public Random r{get; private set;}

        ScrollingBackground background, background2;
        Texture2D backgroundTexture;

        int timesHit = 0, zombiesLet = 0, score = 0;
        SpriteFont font;

        enum GameState { StartState, InState, OverState, RuleState };
        GameState currentState = GameState.StartState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            client = new Rectangle(0, 0, 800, 600);

            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            spriteManager.Enabled = false;
            spriteManager.Visible = false;

            r = new Random();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundTexture = this.Content.Load<Texture2D>("back");
            background = new ScrollingBackground(backgroundTexture, new Vector2(0, 0));
            background2 = new ScrollingBackground(backgroundTexture, new Vector2(-800, 0));

            font = Content.Load<SpriteFont>("SpriteFont1");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            switch (currentState)
            {
                case GameState.StartState:
                    break;
                case GameState.InState:
                    background.Update(-3, 0);
                    background2.Update(-3, 0);
                    break;
                case GameState.OverState:
                    break;
                case GameState.RuleState:
                    break;
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            //background.Update(-3, 0);
            //background2.Update(-3, 0);

            base.Update(gameTime);
        }

        public void AddHit() { timesHit += 1; }
        public void ZombieLet() { zombiesLet += 1; }
        public void AddScore(double sc) { score += (int)sc; }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch(currentState)
            {
                case GameState.StartState:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Megaman vs. Zombies with Fireballs in Jungle", new Vector2(client.Width / 9,
                        client.Height / 3), Color.White);
                    spriteBatch.DrawString(font, "Press Space to start the game", new Vector2(client.Width / 5 + 50, client.Height / 3 + 50),
                        Color.White);
                    spriteBatch.DrawString(font, "Get hit 10 times or Let 10 Zombies Through", new Vector2(client.Width / 9 + 100, client.Height / 3 + 200),
                        Color.White, 0.0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, "AND YOU LOSE", new Vector2(client.Width / 5 + 190, client.Height / 3 + 250),
                        Color.White, 0.0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, "Press R for Rules/Controls", new Vector2(client.Width /4, client.Height-80),
                        Color.White);
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        currentState = GameState.InState;
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.R))
                    {
                        currentState = GameState.RuleState;
                    }
                    spriteBatch.End();
                    break;
                case GameState.RuleState:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Begin();

                    spriteBatch.DrawString(font,"-W/S to move Up/Down \n -Space to shoot \n -Multiplier Increases as time progresses \n SURVIVE",
                        new Vector2(0,0), Color.White);
                    spriteBatch.DrawString(font, "Press Space to Play",
                        new Vector2(0, client.Height/2), Color.White);
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        currentState = GameState.InState;
                        spriteManager.Visible = true;
                        spriteManager.Enabled = true;
                    }
                    spriteBatch.End();
                    break;
                case GameState.InState:
                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    // TODO: Add your drawing code here
                    spriteBatch.Begin();

                    background.DrawLoop(spriteBatch);
                    background2.DrawLoop(spriteBatch);

                    spriteBatch.DrawString(font, "Times Hit: " + timesHit, new Vector2(client.Width - 170, 10), Color.Black);
                    spriteBatch.DrawString(font, "Zombies Let Through: "+zombiesLet, new Vector2(client.Width-340, 30), Color.Black);
                    spriteBatch.DrawString(font, "Score: " + score, new Vector2(client.Width/2-50, client.Height-50), Color.Black);
                    spriteBatch.End();
                    if (zombiesLet == 10 || timesHit==10)
                    {
                        currentState = GameState.OverState;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;                          
                    }
                    break;
                case GameState.OverState:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "GAME OVER", new Vector2(client.Width / 2-75, client.Height / 2), Color.White);
                    spriteBatch.DrawString(font, "Press Esc to Exit", new Vector2(client.Width/5+115, client.Height/3+50),
                        Color.White);
                    spriteBatch.DrawString(font, "Your Score: "+score, new Vector2(client.Width / 5 + 118, client.Height / 3 + 175),
                        Color.White);
                    spriteBatch.End();
                    if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        spriteManager.Enabled=true;
                        spriteManager.Visible=true;
                        Exit();
                    }
                    break;
            }
            base.Draw(gameTime);
        }
    }
}
