using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperDragonBall.Utils
{
    public class CountdownTimer : DrawableGameComponent
    {
        private ContentManager m_kContent;
        private SpriteBatch m_kSpriteBatch;
        private SpriteFont m_kFont;
        private Vector2 m_vPosition;

        //Change the time to count down from here
        private const int countTime = 30;

        private float currentTime = 0;

        public CountdownTimer(Game game, Vector2 vPosition)
            : base(game)
        {
            m_kContent = new ContentManager(game.Services);
            m_kContent.RootDirectory = "Content";
            currentTime = countTime;

            m_vPosition = vPosition;
            DrawOrder = 1000;
        }

        protected override void LoadContent()
        {
            IGraphicsDeviceService graphicsService = (IGraphicsDeviceService)this.Game.Services.GetService(typeof(IGraphicsDeviceService));

            m_kSpriteBatch = new SpriteBatch(graphicsService.GraphicsDevice);
            m_kFont = m_kContent.Load<SpriteFont>("fpsfont");
        }

        protected override void UnloadContent()
        {
            m_kContent.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            
            if (!PauseMenuScreen.IsPaused)
            {
                if (gameTime.ElapsedGameTime.Ticks != 0 && currentTime > 0)
                {
                    
                    currentTime -= gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond / 1000.0f;
                    //(float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond / 1000.0f;
                }

                if (currentTime == 0)
                {

                }
                base.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            m_kSpriteBatch.Begin();

            // Color this based on the framerate
            Color DrawColor = Color.Yellow;

            m_kSpriteBatch.DrawString(m_kFont, "Time: " + (int)currentTime, m_vPosition, DrawColor);
            m_kSpriteBatch.End();

        }

        public void ResetCountdown()
        {
            currentTime = countTime;
        }
    }
}
