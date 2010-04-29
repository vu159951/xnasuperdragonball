using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperDragonBall.Utils
{
    public class FrameRateCounter : DrawableGameComponent
    {
        private ContentManager m_kContent;
        private SpriteBatch m_kSpriteBatch;
        private SpriteFont m_kFont;
        private Queue<float> m_kLastFrames;
        private Vector2 m_vPosition;
        private const int countTime = 100;
        private float lastUpdate = 0;

		private float m_fCurrentFrameRate;
        private float m_fHighestFrameRate;
        private float m_fLowestFrameRate;
        
        public FrameRateCounter(Game game, Vector2 vPosition)
            : base(game)
        {
            m_kContent = new ContentManager(game.Services);
            m_kLastFrames = new Queue<float>();
            m_kContent.RootDirectory = "Content";

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
            float total=0;
            if(gameTime.ElapsedGameTime.Ticks!=0)
            {
                m_kLastFrames.Enqueue(1 / ((float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond / 1000.0f));
            
                

            
                //m_fCurrentFrameRate = total / m_kLastFrames.Count;
            }
            if (m_kLastFrames.Count >= countTime)
            {
                m_kLastFrames.Dequeue();
                //m_fCurrentFrameRate = total / m_kLastFrames.Count;
                //ResetFPSCount();
            }
            if(((float)gameTime.TotalGameTime.Ticks / System.TimeSpan.TicksPerMillisecond) -lastUpdate >=250)
            {
                m_fHighestFrameRate = m_kLastFrames.Peek();
                m_fLowestFrameRate = m_kLastFrames.Peek();
                foreach (float f in m_kLastFrames)
                {
                    total += f;
                    if (m_fHighestFrameRate < f)
                    {
                        m_fHighestFrameRate = f;
                    }
                    if (m_fLowestFrameRate > f)
                    {
                        m_fLowestFrameRate = f;
                    }

                }
                m_fCurrentFrameRate = total / m_kLastFrames.Count;
                lastUpdate=(float)gameTime.TotalGameTime.Ticks / System.TimeSpan.TicksPerMillisecond;
            }
            

			//We can't use the below because our framerate can be SO HIGH that the ms value rounds to zero
			//m_fCurrentFrameRate = 1 / ((float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

			base.Update(gameTime);
        }
        
        public override void Draw(GameTime gameTime)
        {
            m_kSpriteBatch.Begin();
            
			// Color this based on the framerate
            Color DrawColor = Color.Green;
			if (m_fCurrentFrameRate < 15.0f)
                DrawColor = Color.Red;
			else if (m_fCurrentFrameRate < 30.0f)
                DrawColor = Color.Yellow;
           
//			m_kSpriteBatch.DrawString(m_kFont, "Average FPS: " + m_fCurrentFrameRate.ToString("f3"), m_vPosition, DrawColor);
//          m_kSpriteBatch.DrawString(m_kFont, "Highest FPS: " + m_fHighestFrameRate.ToString("f3"), new Vector2(m_vPosition.X,m_vPosition.Y+20), DrawColor);
//          m_kSpriteBatch.DrawString(m_kFont, "Lowest FPS: " + m_fLowestFrameRate.ToString("f3"), new Vector2(m_vPosition.X, m_vPosition.Y + 40), DrawColor);
            m_kSpriteBatch.End();

        }

		public void ResetFPSCount()
		{
            m_kLastFrames.Clear();
		}
    }
}
