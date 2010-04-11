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
    public abstract class GameplayScreen : GameScreen
    {
        #region Fields

        protected ContentManager content;
        protected SpriteFont gameFont;

        protected Random random = new Random();

        protected Utils.Timer m_kTimer = new Utils.Timer();
        protected GameCamera gameCamera;
        //protected Matrix cameraMatrix;
        //protected Matrix projectionMatrix;
        protected Vector3 ambientLightColor = new Vector3(.15f, .3f, .5f);
        protected float specularPower = .010f;
        protected Vector3 specularColor = new Vector3(.3f, .2f, .6f);
        protected Vector3 dLightDirection = new Vector3((float)Math.Sqrt(3), (float)Math.Sqrt(3), -(float)Math.Sqrt(3));
        protected Vector3 dLightColor = new Vector3(.4f, .25f, .1f);
        protected AudioEngine audio;
        protected WaveBank wavebank;
        protected SoundBank soundbank;

        public Matrix CameraMatrix {
            get {
                return gameCamera.CameraMatrix;
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                return gameCamera.ProjectionMatrix;
            }
        }

        public Vector3 AmbientLightColor
        {
            get
            {
                return ambientLightColor;
            }
            set
            {
                ambientLightColor = value;
            }
        }

        public float SpecularPower
        {
            get
            {
                return specularPower;
            }
            set
            {
                specularPower = value;
            }
        }

        public Vector3 SpecularColor
        {
            get
            {
                return specularColor;
            }
            set
            {
                specularColor = value;
            }
        }

        public Vector3 DLightDirection
        {
            get
            {
                return dLightDirection;
            }
            set
            {
                dLightDirection = value;
            }
        }
        public Vector3 DLightColor
        {
            get
            {
                return dLightColor;
            }
            set
            {
                dLightColor = value;
            }
        }

       

        #endregion

        #region Initialization

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
       // public virtual void HandleInput(InputState input, GameTime gameTime)
        //{ }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        //public virtual void Draw(GameTime gameTime) { }

        #endregion

       
    }

		
}
