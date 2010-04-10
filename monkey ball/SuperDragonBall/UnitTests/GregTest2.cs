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
    class GregTest2 : GameplayScreen
    {
        #region Fields


        Ship m_kShip;
        WallManager m_kWallManager;
        //Wall topWall;
        Plane m_kPlane;



        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GregTest2()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            cameraMatrix = Matrix.CreateLookAt(new Vector3(0.0f, 10.0f, 100.0f), Vector3.Zero, Vector3.UnitY);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 2, 1f, 2.0f, 10000f);

        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            m_kShip = new Ship(ScreenManager.Game, this);
            ScreenManager.Game.Components.Add(m_kShip);

            m_kWallManager = new WallManager(ScreenManager.Game, this);
            ScreenManager.Game.Components.Add(m_kWallManager);

            m_kPlane = new Plane(ScreenManager.Game, this);
            ScreenManager.Game.Components.Add(m_kPlane);

        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
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
            m_kShip.rotationVelocity = 0;
            if (input.ShipTurnLeft)
            {
                m_kShip.rotationVelocity += 3;
            }
            if (input.ShipTurnRight)
            {
                m_kShip.rotationVelocity += -3;
            }
            Vector3 thrust = Vector3.Zero;
            if (input.ShipMove)
            {
                thrust += 3500f * m_kShip.directionVec;
            }
            if (input.ReverseThrust)
            {
                thrust += -3500f * m_kShip.directionVec;
            }
            m_kShip.netForce = thrust;
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
