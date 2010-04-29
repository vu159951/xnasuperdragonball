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
using SuperDragonBall.Utils;
#endregion

namespace SuperDragonBall
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class ScottTest1 : GameplayScreen
    {
        #region Fields


        BallCharacter player;
        WallManager m_kWallManager;
        //Wall topWall;
        CollisionLevelPiece m_kPlane;
        float manualCameraRotation;

        protected Vector3 gravityVec;



        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public ScottTest1()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            gameCamera = new GameCamera();
            gravityVec = new Vector3(0, -100, 0);
            manualCameraRotation = 0f;
           

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

            m_kPlane = new CollisionLevelPiece(ScreenManager.Game, this, "checker_plane_3");
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
            float timeDelta = (float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond / 1000;

            player.velocity += gravityVec * timeDelta;
           // gameCamera.followBehind(player);

            //test for collision
            Vector3 pushAway = m_kPlane.testCollision(player);
            if (pushAway != Vector3.Zero)
            {
                player.CollidedWithStage = true;
                respondToCollision(pushAway, timeDelta);
            }
            else
            {
                player.CollidedWithStage = false;
            }

            //reset player position when fallen off
            if (player.position.Y < -100)
            {
                player.position = new Vector3(0f, 25f, 0f);
                player.velocity = Vector3.Zero;
                player.netForce = Vector3.Zero;
            }



            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }

        private void respondToCollision(Vector3 pushAway, float timeDelta)
        {

            //Vector3 v3 = m_kPlane.getPlaneNormal();
            Vector3 vDiff = player.velocity;
            vDiff.X += pushAway.X * 10;
            vDiff.Z += pushAway.Z * 10;
            vDiff.Y += pushAway.Y;
            vDiff.Y -= (gravityVec.Y * timeDelta);
            player.velocity = vDiff;

            //temporary collision resolution
            //player.velocity += v3;

            //player.position += new Vector3(0, 5, 0);
            //Vector3 stop = player.velocity;
            //stop.Y = 0;
            //player.velocity = stop;
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
            Vector3 m_kLookingDir = player.position - gameCamera.GetCameraPosition();
            m_kLookingDir.Y = 0f;
            m_kLookingDir.Normalize();
            //m_kLookingDir = new Vector3(-1f, 0, 0);

            //The axis are flipped from what would be expected
            //If you want to go forward/backwards, you need to switch Z and X
            //If you want to go Left/Right you keep Z and X consistent
            if (input.IsKeyHeld(Keys.Up))
            {

                m_kPlane.RotX +=  ((float)Math.PI / 9) * m_kLookingDir.Z;
                m_kPlane.RotZ += -((float)Math.PI / 9) * m_kLookingDir.X;
                
            }
            if (input.IsKeyHeld(Keys.Down))
            {
                m_kPlane.RotX += -((float)Math.PI / 9) * m_kLookingDir.Z;
                m_kPlane.RotZ +=  ((float)Math.PI / 9) * m_kLookingDir.X;
            }
            if (input.IsKeyHeld(Keys.Left))
            {
                m_kPlane.RotX += -((float)Math.PI / 9) * m_kLookingDir.X;
                m_kPlane.RotZ += -((float)Math.PI / 9) * m_kLookingDir.Z;
            }
            if (input.IsKeyHeld(Keys.Right))
            {
                m_kPlane.RotX +=  ((float)Math.PI / 9) * m_kLookingDir.X;
                m_kPlane.RotZ +=  ((float)Math.PI / 9) * m_kLookingDir.Z;
            }

            /*
            //for gamepad input (untested!!)
            //for forward/backward movement
            GamePadState gamePadState = input.CurrentGamePadStates[0];
            m_kPlane.RotX += ((float)Math.PI / 9) * m_kLookingDir.Z * gamePadState.ThumbSticks.Left.Y;
            m_kPlane.RotZ += -((float)Math.PI / 9) * m_kLookingDir.X * gamePadState.ThumbSticks.Left.Y;

            //for Left/Right movement
            m_kPlane.RotX += -((float)Math.PI / 9) * m_kLookingDir.X * gamePadState.ThumbSticks.Left.X;
            m_kPlane.RotZ += -((float)Math.PI / 9) * m_kLookingDir.Z * gamePadState.ThumbSticks.Left.X;
            */

            m_kPlane.setRotationOffset(player.position);

            //Manually changing the camera rotation based on user input

            if (input.IsADown)
            {
                manualCameraRotation += (float)Math.PI / 4 * (float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond / 1000;
            }
            if (input.IsDDown)
            {
                manualCameraRotation -= (float)Math.PI / 4 * (float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond / 1000;
            }

            gameCamera.ManualCameraRotation(manualCameraRotation, player);

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
