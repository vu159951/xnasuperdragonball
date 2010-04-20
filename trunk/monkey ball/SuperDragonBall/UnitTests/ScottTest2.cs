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

using SuperDragonBall.Actors;
using SuperDragonBall.Utils;

#endregion

namespace SuperDragonBall
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class ScottTest2 : GameplayScreen
    {
        #region Fields


        public BallCharacter player;
        WallManager m_kWallManager;
        //Wall topWall;
        //LevelPiece m_kPlane;

        public LevelDataTest level1;
        List<LevelPiece> planes;



        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public ScottTest2()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            gameCamera = new GameCamera();
            
            
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            level1 = new LevelDataTest(ScreenManager.Game, this);
            player = new BallCharacter(ScreenManager.Game, this);
            player.position = level1.startingLocation;
            ScreenManager.Game.Components.Add(player);


            m_kWallManager = new WallManager(ScreenManager.Game, this);
            ScreenManager.Game.Components.Add(m_kWallManager);

            

        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            ScreenManager.Game.Components.Remove(player);
           
            m_kWallManager.removeWallComponents();
            ScreenManager.Game.Components.Remove(m_kWallManager);

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
            gameCamera.followBehind(player);

            level1.MovePlayer(player, gameTime);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }

        /// <summary>
        /// Collision resolution. Kind of a hack.
        /// Called from the Update function
        /// Tweak numbers until the feel is "right"
        /// </summary>
        /// <param name="pushAway">The normalized vector that the ball should be pushed away</param>
        /// <param name="timeDelta"></param>
       

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

        
            float RotX = 0;
            float RotZ = 0;
            if (input.IsKeyHeld(Keys.Up))
            {
                RotX += -(float)Math.PI / 9;
            }
            if (input.IsKeyHeld(Keys.Down))
            {
                RotX += (float)Math.PI / 9;
            }
            if (input.IsKeyHeld(Keys.Left))
            {
                RotZ += (float)Math.PI / 9;
            }
            if (input.IsKeyHeld(Keys.Right))
            {
                RotZ += -(float)Math.PI / 9;
            }

            level1.setRotation(RotX, RotZ, player.position);
            /*
            //OLD SHIP FUNCTIONS
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
            */
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
