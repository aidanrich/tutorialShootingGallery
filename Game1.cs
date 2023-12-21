using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tutorialShootingGallery
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D targetSprite;
        Texture2D backgroundSprite;
        Texture2D crosshairsSprite;
        SpriteFont gameFont;

        Vector2 targetPosition = new Vector2(300, 300);
        const int targetRadius = 45;

        MouseState mState;
        bool mReleased = true;
        int score = 0;
        int winningScore = 30;

        double timer = 10;
        double bonus = 10;
        bool level2 = false;
        bool level2TimeBonus = false;
        bool level3 = false;
        bool level3TimeBonus = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            targetSprite = Content.Load<Texture2D>("target");
            backgroundSprite = Content.Load<Texture2D>("sky");
            crosshairsSprite = Content.Load<Texture2D>("crosshairs");
            gameFont = Content.Load<SpriteFont>("galleryFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (timer > 0 && score != winningScore) 
            { 
                timer -= gameTime.ElapsedGameTime.TotalSeconds; 
            }

            if (timer < 0)
            {
                timer = 0;
            }

            if (score > 10 && level2 == false && level2TimeBonus == false)
            {
                level2 = true;
                level2TimeBonus = true;
                
            }
            if (level2TimeBonus == true && level2 == true)
            {
                timer += bonus;
                level2TimeBonus = false;
            }

            if (score > 20 && level3 == false && level3TimeBonus == false)
            {
                level3 = true;
                level3TimeBonus = true;

            }
            if (level3TimeBonus == true && level3 == true)
            {
                timer += bonus;
                level3TimeBonus = false;
            }

            mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
            {
                float mouseTargetDist = Vector2.Distance(targetPosition, mState.Position.ToVector2());
                if (mouseTargetDist < targetRadius && timer > 0)
                {
                    score++;

                    Random rand = new Random();

                    targetPosition.X = rand.Next(0, _graphics.PreferredBackBufferWidth);
                    targetPosition.Y = rand.Next(0, _graphics.PreferredBackBufferHeight);
                }
                mReleased = false;
            }

            if (mState.LeftButton == ButtonState.Released)
            {
                mReleased = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);
            if (level2 == true)
            {
                _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.Green);
            }
            if (level3 == true)
            {
                _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.Red);
            }
            if (timer > 0 && score < winningScore)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(targetPosition.X - targetRadius, targetPosition.Y - targetRadius), Color.White);
            }
            _spriteBatch.Draw(crosshairsSprite, new Vector2(mState.X - 25, mState.Y - 25), Color.White);
            _spriteBatch.DrawString(gameFont, "Score: " + score.ToString(), new Vector2(3, 3), Color.White);
            _spriteBatch.DrawString(gameFont, "Time: " + Math.Ceiling(timer).ToString(), new Vector2(3, 40), Color.White);
            if (score == winningScore)
            {
                _spriteBatch.DrawString(gameFont, "Victory!", new Vector2(_graphics.PreferredBackBufferWidth / 2 - 80, _graphics.PreferredBackBufferHeight / 2 - 50), Color.White);
            }
            if (timer == 0 && score < winningScore)
            {
                _spriteBatch.DrawString(gameFont, "Game Over", new Vector2(_graphics.PreferredBackBufferWidth / 2 - 80, _graphics.PreferredBackBufferHeight / 2 - 50), Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
