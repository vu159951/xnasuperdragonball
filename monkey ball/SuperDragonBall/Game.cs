#region File Description
//-----------------------------------------------------------------------------
// Game.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace SuperDragonBall
{
    /// <summary>
    /// Sample showing how to manage different game states, with transitions
    /// between menu screens, a loading screen, the game itself, and a pause
    /// menu. This main game class is extremely simple: all the interesting
    /// stuff happens in the ScreenManager component.
    /// </summary>
    public class GameStateManagementGame : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        public Utils.FrameRateCounter m_kFrameRate;
        

        //screen width/height
        public static int SCREEN_WIDTH = 1024;
        public static int SCREEN_HEIGHT = 768;
        //buffer for world wrap
        public static int BUFFER_W = 50;
        public static int BUFFER_H = 50;

        #endregion

		#region Properties
		public float fTargetMsPerFrame
		{
			get { return (float)TargetElapsedTime.Milliseconds; }
			set 
			{ 
				if (value > 1.0f)
					TargetElapsedTime = System.TimeSpan.FromMilliseconds(value); 
				else
					TargetElapsedTime = System.TimeSpan.FromMilliseconds(1.0f); 
			}
		}
		#endregion

		#region Initialization


		/// <summary>
        /// The main game constructor.
        /// </summary>
        public GameStateManagementGame()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;

            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen());
            screenManager.AddScreen(new MainMenuScreen());

            m_kFrameRate = new Utils.FrameRateCounter(this, new Vector2(700.0f, 600.0f));
            Components.Add(m_kFrameRate);

            

            // For testing purposes, let's disable fixed time step and vsync.
            // non-fixed timestep is damned irritating
            IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = false;
        }


        #endregion

        #region Draw


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }


        #endregion
    }


    #region Entry Point

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static class Program
    {
        static void Main()
        {
            using (GameStateManagementGame game = new GameStateManagementGame())
            {
                game.Run();
            }
        }
    }

    #endregion
}
