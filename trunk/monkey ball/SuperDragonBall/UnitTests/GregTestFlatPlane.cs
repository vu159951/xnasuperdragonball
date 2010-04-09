#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
#endregion

namespace SuperDragonBall
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GregTestFlatPlane : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        Vector2 playerPosition = new Vector2(100, 100);
        Vector2 enemyPosition = new Vector2(100, 100);

        Random random = new Random();

        Utils.Timer m_kTimer = new Utils.Timer();
        public static Matrix CameraMatrix;
        public static Matrix ProjectionMatrix;
        public static Vector3 AmbientLightColor = new Vector3(.15f, .3f, .5f);
        public static float SpecularPower = .010f;
        public static Vector3 SpecularColor = new Vector3(.3f, .2f, .6f);
        public static Vector3 DLightDirection = new Vector3((float)Math.Sqrt(3), (float)Math.Sqrt(3), -(float)Math.Sqrt(3));
        public static Vector3 DLightColor = new Vector3(.4f, .25f, .1f);
        public static AudioEngine audio;
        public static WaveBank wavebank;
        public static SoundBank soundbank;
        //public static Dictionary<Asteroid, bool> asteroidCollided = new Dictionary<Asteroid, bool>();
        Ship player;
        WallManager m_kWallManager;
        //Wall topWall;
        Plane m_kPlane;



        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GregTestFlatPlane()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            //CameraMatrix = Matrix.CreateLookAt(new Vector3(0.0f, 10.0f, 100.0f), Vector3.Zero, Vector3.UnitY);

            //ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 2, 1f, 2.0f, 10000f);
            
            CameraMatrix = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 2000.0f), Vector3.Zero, Vector3.UnitY);
            ProjectionMatrix = Matrix.CreateOrthographic(GameStateManagementGame.SCREEN_WIDTH,
                GameStateManagementGame.SCREEN_HEIGHT, 0.1f, 10000.0f);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("gamefont");

            audio = new AudioEngine("Content/Sounds.xgs");
            wavebank = new WaveBank(audio, "Content/XNAsteroids Waves.xwb");
            soundbank = new SoundBank(audio, "Content/XNAsteroids Cues.xsb");

           

            player = new Ship(ScreenManager.Game);
            ScreenManager.Game.Components.Add(player);

            m_kWallManager = new WallManager(ScreenManager.Game);
            ScreenManager.Game.Components.Add(m_kWallManager);

            m_kPlane = new Plane(ScreenManager.Game);
            ScreenManager.Game.Components.Add(m_kPlane);

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            //Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.

            ScreenManager.Game.ResetElapsedTime();

        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive)
            {
                m_kTimer.Update(gameTime);

            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input, GameTime gameTime)
        {
            float timeDelta = (float)(gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond / 1000.0f);

            if (input == null)
                throw new ArgumentNullException("input");

            if (input.PauseGame)
            {
                // If they pressed pause, bring up the pause menu screen.
                ScreenManager.AddScreen(new PauseMenuScreen());
            }
            if (input.ShipFire)
            {
                //fireMissile();
            }
            player.rotationVelocity = 0;
            if (input.ShipTurnLeft)
            {
                player.rotationVelocity += 3;
            }
            if (input.ShipTurnRight)
            {
                player.rotationVelocity += -3;
            }
            Vector3 thrust = Vector3.Zero;
            if (input.ShipMove)
            {
                thrust += 3500f * player.directionVec;
            }
            if (input.ReverseThrust)
            {
                thrust += -3500f * player.directionVec;
            }
            player.netForce = thrust;
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
           
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.Black, 0, 0);


            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);

        }

        #endregion

    }


}
