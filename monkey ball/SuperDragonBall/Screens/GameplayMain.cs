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

using SuperDragonBall.Actors;
using SuperDragonBall.Utils;
using SuperDragonBall.Levels;

#endregion

namespace SuperDragonBall
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayMain : GameplayScreen
    {
        #region Fields
        public Utils.CountdownTimer m_kCountdownTimer;
        public Utils.ScoreKeeper m_kScoreKeeper;
        public BallCharacter player;
        //WallManager m_kWallManager;
        //Wall topWall;
        //LevelPiece m_kPlane;
        protected Vector3 m_kLookingDir;

        public LevelData activeLevel;
        //List<LevelPiece> planes;
        private int currentLevel = 0;
        //modify this to increse the number of levels
        private int TOTAL_LEVELS = 5;

     
        private SkySphere sky;

        private float manualCameraRotation;

        private const Boolean IsTesting = false; //Use this to turn off automatic level restart when the timer runs out
        
        int score;

       


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayMain()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            
            gameCamera = new GameCamera();
            //LevelList = new List<LevelData>();
            score = 0;
            dLightColor = new Vector3(.7f, .7f, 0f);
            dLightDirection = Vector3.Normalize(new Vector3(0f, -1f, -(float)Math.Sqrt(2)));
            specularColor = new Vector3(.17f, .10f, .33f);
            AmbientLightColor = Vector3.Normalize(new Vector3(.25f, .25f, 1f));
            manualCameraRotation = 0f;

        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            sky = new SkySphere(ScreenManager.Game, this);
            player = new BallCharacter(ScreenManager.Game, this);
            //soundEffect.Play(1.0f, 0.0f, 0.0f, true);

            //sets up the level
            SwitchToNextLevel();

        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            ScreenManager.Game.Components.Remove(player);
            activeLevel.clearLevel(ScreenManager.Game);
            ScreenManager.Game.Components.Remove(activeLevel);
            //m_kWallManager.removeWallComponents();
            //ScreenManager.Game.Components.Remove(m_kWallManager);
            ScreenManager.Game.Components.Remove(m_kCountdownTimer);
            ScreenManager.Game.Components.Remove(m_kScoreKeeper);
            ScreenManager.Game.Components.Remove(sky);

            base.UnloadContent();
        }

        public void SwitchToNextLevel()
        {
            currentLevel++;
            if (currentLevel > TOTAL_LEVELS) {
                currentLevel = 1;
            }
            //currentLevel = currentLevel % LevelList.Count;
            //activeLevel = (LevelData)LevelList.ToArray()[currentLevel];
            //activeLevel.startLevel(ScreenManager.Game);

            if (activeLevel != null)
            {
                
                activeLevel.clearLevel(ScreenManager.Game);
                activeLevel = null;
            }
            switch (currentLevel) { 
                case 1:
                    activeLevel = new Level1(ScreenManager.Game, this);
                    break;
                case 2:
                    activeLevel = new Level2(ScreenManager.Game, this);
                    break;
                case 3:
                    activeLevel = new Level3(ScreenManager.Game, this);
                    break;
                case 4:
                    activeLevel = new Level4(ScreenManager.Game, this);
                    break;
                case 5:
                    activeLevel = new Level5(ScreenManager.Game, this);
                    break;
                default:
                    break;                    
            }
            activeLevel.startLevel(ScreenManager.Game);

            
            player.position = activeLevel.startingLocation;
            player.velocity = Vector3.Zero;
            player.netForce = Vector3.Zero;
            manualCameraRotation = 0.0f;
            ScreenManager.Game.Components.Add(player);
            m_kCountdownTimer = new Utils.CountdownTimer(ScreenManager.Game, new Vector2(875.0f, 20.0f));
            m_kScoreKeeper = new Utils.ScoreKeeper(ScreenManager.Game, new Vector2(20f, 20f));
            m_kScoreKeeper.setScore(score);
            ScreenManager.Game.Components.Add(m_kCountdownTimer);
            ScreenManager.Game.Components.Add(m_kScoreKeeper);
            ScreenManager.Game.Components.Add(sky);

            

            

        }

        public void RestartLevel()
        {
            UnloadContent();
            //activeLevel = (LevelData)LevelList.ToArray()[currentLevel];
            activeLevel.startLevel(ScreenManager.Game);

            player.position = activeLevel.startingLocation;
            player.velocity = Vector3.Zero;
            player.netForce = Vector3.Zero;

            ScreenManager.Game.Components.Add(player);
            m_kCountdownTimer = new Utils.CountdownTimer(ScreenManager.Game, new Vector2(875.0f, 20.0f));
            m_kScoreKeeper = new Utils.ScoreKeeper(ScreenManager.Game, new Vector2(20f, 20f));
            m_kScoreKeeper.setScore(score);
            ScreenManager.Game.Components.Add(m_kCountdownTimer);
            ScreenManager.Game.Components.Add(m_kScoreKeeper);
            ScreenManager.Game.Components.Add(sky);
            manualCameraRotation = 0.0f;
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
            if (IsActive)
            {
                //gameCamera.followBehind(player);

                activeLevel.MovePlayer(player, gameTime);
                
                //fell off the stage
                //reset player position when fallen off
                if (player.position.Y < activeLevel.DeathBound)
                {
                    RestartLevel();
                }


                //hit the goal
                if (activeLevel.IsCollidingWithGoal(player))
                {
                    UnloadContent();
                    soundbank.PlayCue("zing");
                    SwitchToNextLevel();
                }
                if (activeLevel.IsCollidingWithCollectable(player, ScreenManager.Game))
                {
                    score++;
                    m_kScoreKeeper.setScore(score);
                }
                if (m_kCountdownTimer.getRemainingTime() == 0 && !IsTesting)
                {
                    RestartLevel();
                }
                //realign camera position
                m_kLookingDir = player.position - gameCamera.GetCameraPosition();
                m_kLookingDir.Y = 0f;
                m_kLookingDir.Normalize();
            }


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
            //The axis are flipped from what would be expected
            //If you want to go forward/backwards, you need to switch Z and X
            //If you want to go Left/Right you keep Z and X consistent
            if (input.IsKeyHeld(Keys.Up))
            {
                RotX += ((float)Math.PI / 9) * m_kLookingDir.Z;
                RotZ += -((float)Math.PI / 9) * m_kLookingDir.X;
            }
            if (input.IsKeyHeld(Keys.Down))
            {
                RotX += -((float)Math.PI / 9) * m_kLookingDir.Z;
                RotZ += ((float)Math.PI / 9) * m_kLookingDir.X;
            }
            if (input.IsKeyHeld(Keys.Left))
            {
                RotX += -((float)Math.PI / 9) * m_kLookingDir.X;
                RotZ += -((float)Math.PI / 9) * m_kLookingDir.Z;
            }
            if (input.IsKeyHeld(Keys.Right))
            {
                RotX += ((float)Math.PI / 9) * m_kLookingDir.X;
                RotZ += ((float)Math.PI / 9) * m_kLookingDir.Z;
            }


            //Manually changing the camera rotation based on user input
            if (input.IsADown)
            {
                manualCameraRotation += (float)Math.PI / 4 * (float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond / 1000;
            }
            if (input.IsDDown)
            {
                manualCameraRotation -= (float)Math.PI / 4 * (float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond / 1000;
            }

            //camera rotation for 360 controller
            GamePadState gamePadState = input.CurrentGamePadStates[0];
            manualCameraRotation += (float)Math.PI / 4 * gamePadState.ThumbSticks.Right.X * timeDelta * 5;
            gameCamera.ManualCameraRotation(manualCameraRotation, player);

            //for forward/backward movement
            //GamePadState gamePadState = input.CurrentGamePadStates[0];
           // RotX += ((float)Math.PI / 9) * m_kLookingDir.Z;
           // RotZ += -((float)Math.PI / 9) * m_kLookingDir.X;

            RotX += ((float)Math.PI / 9) * m_kLookingDir.Z * gamePadState.ThumbSticks.Left.Y;
            RotZ += -((float)Math.PI / 9) * m_kLookingDir.X * gamePadState.ThumbSticks.Left.Y;

            //for Left/Right movement
            RotX += ((float)Math.PI / 9) * m_kLookingDir.X * gamePadState.ThumbSticks.Left.X;
            RotZ += ((float)Math.PI / 9) * m_kLookingDir.Z * gamePadState.ThumbSticks.Left.X;

            activeLevel.setRotation(RotX, RotZ, player.position);
           
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
