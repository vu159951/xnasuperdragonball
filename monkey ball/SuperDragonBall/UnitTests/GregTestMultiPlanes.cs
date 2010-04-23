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
    class GregTestMultiPlanes : GameplayScreen
    {
        #region Fields


        BallCharacter player;
        WallManager m_kWallManager;
        //Wall topWall;
       

        List<LevelPiece> planes;
        protected Vector3 gravityVec;
        protected Vector3 m_kLookingDir;


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GregTestMultiPlanes()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            gameCamera = new GameCamera();
            gravityVec = new Vector3(0, -100, 0);
            planes = new List<LevelPiece>();
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            player = new BallCharacter(ScreenManager.Game, this);
            player.position = new Vector3(10f, 25f, 0f);
            ScreenManager.Game.Components.Add(player);


            m_kWallManager = new WallManager(ScreenManager.Game, this);
            ScreenManager.Game.Components.Add(m_kWallManager);

            String assetName = "checker_plane_3";

            //make a few planes
            LevelPiece currentPlane;
            currentPlane = new LevelPiece(ScreenManager.Game, this, assetName);
            currentPlane.scale = 15;
            currentPlane.position += new Vector3(0f, -10f, 0);
            planes.Add(currentPlane);

            currentPlane = new LevelPiece(ScreenManager.Game, this, assetName);
            currentPlane.scale = 10;
            currentPlane.position += new Vector3(200f, -10f, -300f);
            currentPlane.setLocalRotation(0, (float) Math.PI / 18);
            planes.Add(currentPlane);

            currentPlane = new LevelPiece(ScreenManager.Game, this, assetName);
            currentPlane.scale = 10;
            currentPlane.position += new Vector3(-250f, -10f, -200f);
            currentPlane.setLocalRotation(0.123f, -(float)Math.PI / 18);
            planes.Add(currentPlane);

            currentPlane = new LevelPiece(ScreenManager.Game, this, assetName);
            currentPlane.scale = 5;
            currentPlane.position += new Vector3(-220f, 10f, 80f);
            planes.Add(currentPlane);

            //moving level piece
            MovingLevelPiece movingPlane;
            //50 is a good movement speed
            movingPlane = new MovingLevelPiece(ScreenManager.Game, this, assetName, new Vector3(0, 100f, 0f), 50);
            movingPlane.scale = 5;
            movingPlane.position += new Vector3(0f, 0f, -200f);
            //CRITICAL!!!
            movingPlane.OriginalPosition = movingPlane.position;
            planes.Add(movingPlane);

            foreach (LevelPiece p in planes)
            {
                ScreenManager.Game.Components.Add(p);
            }

        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            ScreenManager.Game.Components.Remove(player);
            foreach (LevelPiece p in planes)
            {
                ScreenManager.Game.Components.Remove(p);
            }
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
            gameCamera.followBehind(player);

            //test for collision
            Vector3 pushAway = Vector3.Zero;

            foreach (LevelPiece p in planes)
            {
                pushAway += p.testCollision(player);
            }
           
            if (pushAway != Vector3.Zero)
            {
                pushAway.Normalize();
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

            //realign camera position
            m_kLookingDir = player.position - gameCamera.GetCameraPosition();
            m_kLookingDir.Y = 0f;
            m_kLookingDir.Normalize();

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }

        /// <summary>
        /// Collision resolution. Kind of a hack.
        /// Called from the Update function
        /// Tweak numbers until the feel is "right"
        /// </summary>
        /// <param name="pushAway">The normalized vector that the ball should be pushed away</param>
        /// <param name="timeDelta"></param>
        private void respondToCollision(Vector3 pushAway, float timeDelta)
        {
            //Vector3 v3 = m_kPlane.getPlaneNormal();
            Vector3 vDiff = player.velocity;
            vDiff.X += pushAway.X * 10;
            vDiff.Z += pushAway.Z * 10;
            if (vDiff.Y < -50) {
                vDiff.Y /= 1.5f;
            }
            vDiff.Y += pushAway.Y;
            vDiff.Y -= (gravityVec.Y * timeDelta) * 2;
            player.velocity = vDiff;
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

            foreach (LevelPiece p in planes)
            {
                p.RotX = 0;
                p.RotZ = 0;
                if (input.IsKeyHeld(Keys.Up))
                {
                    p.RotX += -(float)Math.PI / 9;
                }
                if (input.IsKeyHeld(Keys.Down))
                {
                    p.RotX += (float)Math.PI / 9;
                }
                if (input.IsKeyHeld(Keys.Left))
                {
                    p.RotZ += (float)Math.PI / 9;
                }
                if (input.IsKeyHeld(Keys.Right))
                {
                    p.RotZ += -(float)Math.PI / 9;
                }
              
                //for forward/backward movement
                GamePadState gamePadState = input.CurrentGamePadStates[0];
                p.RotX += ((float)Math.PI / 9) * m_kLookingDir.Z * gamePadState.ThumbSticks.Left.Y;
                p.RotZ += ((float)Math.PI / 9) * m_kLookingDir.X * gamePadState.ThumbSticks.Left.Y;

                //for Left/Right movement
                p.RotX += ((float)Math.PI / 9) * m_kLookingDir.X * gamePadState.ThumbSticks.Left.X;
                p.RotZ += ((float)Math.PI / 9) * m_kLookingDir.Z * gamePadState.ThumbSticks.Left.X;

                p.setRotationOffset(player.position);
            }

         
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
