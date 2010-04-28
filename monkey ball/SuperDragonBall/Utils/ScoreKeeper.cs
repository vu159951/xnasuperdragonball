using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SuperDragonBall.Utils
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ScoreKeeper : DrawableGameComponent
    {
        private ContentManager m_kContent;
        private SpriteBatch m_kSpriteBatch;
        private SpriteFont m_kFont;
        private Vector2 m_vPosition;
        private int score;


        public ScoreKeeper(Game game, Vector2 vPosition)
            : base(game)
        {
             m_kContent = new ContentManager(game.Services);
            m_kContent.RootDirectory = "Content";
            score=0;
            m_vPosition = vPosition;
            DrawOrder = 1000;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
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


        public override void Draw(GameTime gameTime)
        {
            m_kSpriteBatch.Begin();

            // Color this based on the framerate
            Color DrawColor = Color.Yellow;

            m_kSpriteBatch.DrawString(m_kFont, "Score: " + score, m_vPosition, DrawColor);
            m_kSpriteBatch.End();
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public void setScore(int newScore)
        {
            score=newScore;
        }
    }
}