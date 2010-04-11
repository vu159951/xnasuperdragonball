﻿#region File Description
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
    class GregTestFlatPlane : GameplayScreen
    {
        #region Fields


        BallCharacter player;
        WallManager m_kWallManager;
        //Wall topWall;
        Plane m_kPlane;

        protected Vector3 gravityVec;



        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GregTestFlatPlane()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            gameCamera = new GameCamera();
            gravityVec = new Vector3(0, -1, 0);

        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            player = new BallCharacter(ScreenManager.Game, this);
            player.position = new Vector3(0f, 25f, 0f);
            ScreenManager.Game.Components.Add(player);


            m_kWallManager = new WallManager(ScreenManager.Game, this);
            ScreenManager.Game.Components.Add(m_kWallManager);

            m_kPlane = new Plane(ScreenManager.Game, this);
            m_kPlane.scale = 15;
            m_kPlane.position += new Vector3(0, -10f, 0);
            ScreenManager.Game.Components.Add(m_kPlane);
            
          
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            ScreenManager.Game.Components.Remove(player);
            ScreenManager.Game.Components.Remove(m_kPlane);
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
            player.velocity += gravityVec * 0.1f;
            gameCamera.followBehind(player);

            

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

           
            m_kPlane.RotX = 0;
            m_kPlane.RotZ = 0;
            if(input.IsKeyHeld(Keys.Up)) {
                m_kPlane.RotX += -(float)Math.PI / 9;
            }
            if (input.IsKeyHeld(Keys.Down))
            {
                m_kPlane.RotX += (float)Math.PI / 9;
            }
            if (input.IsKeyHeld(Keys.Left))
            {
                m_kPlane.RotZ += (float)Math.PI / 9;
            }
            if (input.IsKeyHeld(Keys.Right))
            {
                m_kPlane.RotZ += -(float)Math.PI / 9;
            }

            if (m_kPlane.testCollision(player))
            {
                player.CollidedWithStage = true;
                Console.WriteLine("HIT");
                //gravityVec = Vector3.Zero;
                Vector3 v3 = m_kPlane.getPlaneNormal();
                //if (v3.X != 0 || v3.Z != 0) {
                //   Console.WriteLine(v3);
                //}

                Vector3 vDiff = player.velocity;
                vDiff.X += v3.X * 10;
                vDiff.Z += v3.Z * 10;
                player.velocity = vDiff;

                //temporary collision resolution
                player.velocity += v3;
                //player.position += new Vector3(0, 5, 0);
                Vector3 stop = player.velocity;
                stop.Y = 0;
                player.velocity = stop;
            }
            else
            {
                player.CollidedWithStage = false;
            }
      
            m_kPlane.setRotationOffset(player.position);

            
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
