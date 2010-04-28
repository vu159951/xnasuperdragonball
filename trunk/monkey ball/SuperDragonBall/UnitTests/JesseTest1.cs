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
    class JesseTest1 : GameplayScreen
    {
        #region Fields

        BallCharacter player;
        WallManager m_kWallManager;
        LevelPiece m_kPlane;

        protected Vector3 gravityVec;

        private Model[] meshes;

        #endregion

        /// <summary>
        /// Effect objects
        /// </summary>
        #region Effect Fields
        private Effect celLightingEffect;
        private EffectParameter projectionParameter;
        private EffectParameter viewParameter;
        private EffectParameter worldParameter;
        private EffectParameter lightColorParameter;
        private EffectParameter lightDirectionParameter;
        //private EffectParameter ambientColorParameter;
        private EffectParameter EyePositionParameter;
        private EffectParameter LightPositionParameter;
        #endregion

        /// <summary>
        /// Data fields corresponding to the effect paramters
        /// </summary>
        #region Uniform Data Fields
        private Matrix world, view, projection;
        private Vector3 diffuseLightDirection;
        private Vector4 diffuseLightColor;
        //private Vector4 _ambientLightColor;
        private Vector3 eyePosition;
        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public JesseTest1()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            gameCamera = new GameCamera();
            gravityVec = new Vector3(0, -100, 0);

        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            meshes = new Model[1];
            meshes[0] = ScreenManager.Game.Content.Load<Model>("meshes/cone");

            player = new BallCharacter(ScreenManager.Game, this);
            //player.position = new Vector3(10f, 25f, 0f);
            player.position = new Vector3(0f, 0f, 0f);
            ScreenManager.Game.Components.Add(player);


            m_kWallManager = new WallManager(ScreenManager.Game, this);
            ScreenManager.Game.Components.Add(m_kWallManager);

            m_kPlane = new LevelPiece(ScreenManager.Game, this, "checker_plane");
            m_kPlane.scale = 15;
            m_kPlane.position += new Vector3(150f, -10f, 0);
            ScreenManager.Game.Components.Add(m_kPlane);

            celLightingEffect = ScreenManager.Game.Content.Load<Effect>("Cel");
            GetEffectParameters();

            //Calculate the projection properties first on any 
            //load callback.  That way if the window gets resized,
            //the perspective matrix is updated accordingly
            float aspectRatio = (float)ScreenManager.Game.GraphicsDevice.Viewport.Width /
                (float)ScreenManager.Game.GraphicsDevice.Viewport.Height;
            float fov = MathHelper.PiOver4 * aspectRatio * 3 / 4;
            projection = Matrix.CreatePerspectiveFieldOfView(fov,
                aspectRatio, .1f, 1000f);

            //create a default world matrix
            world = Matrix.Identity;
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

            //player.velocity += gravityVec * timeDelta;
            //gameCamera.followBehind(player);

            //test for collision
            //Vector3 pushAway = m_kPlane.testCollision(player);
            //if (pushAway != Vector3.Zero)
           // {
          //      player.CollidedWithStage = true;
          //      respondToCollision(pushAway, timeDelta);
          //  }
         //   else
         //   {
        //        player.CollidedWithStage = false;
        //    }

            //reset player position when fallen off
        //    if (player.position.Y < -100) {
        //        player.position = new Vector3(0f, 25f, 0f);
       //         player.velocity = Vector3.Zero;
        //        player.netForce = Vector3.Zero;
        //    }

            //Set the light direction to a fixed value.
            //This will place the light source behind, to the right, and above the user.
            diffuseLightDirection = new Vector3(1, 1, 1);

            //ensure the light direction is normalized, or
            //the shader will give some weird results
            //diffuseLightDirection.Normalize();

            //set the color of the diffuse light
            diffuseLightColor = Color.White.ToVector4();


            //set the ambient lighting color
            //_ambientLightColor = Color.DarkSlateGray.ToVector4();

            //The built-in camera class provides the view matrix
            view = gameCamera.ProjectionMatrix;
            //view = camera.ViewMatrix;

            //eyePosition = camera.Position;
            eyePosition = gameCamera.GetCameraPosition();

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            Draw(gameTime);

        }

        /// <summary>
        /// Collision resolution. Kind of a hack.
        /// Called from the Update function
        /// Tweak numbers until the feel is "right"
        /// </summary>
        /// <param name="pushAway">The normalized vector that the ball should be pushed away</param>
        /// <param name="timeDelta"></param>
        private void respondToCollision(Vector3 pushAway, float timeDelta) {

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
            /*
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

      
            m_kPlane.setRotationOffset(player.position);
            */
            
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

            //always set the shared effects parameters
            SetSharedEffectParameters();

            //ambientColorParameter.SetValue(_ambientLightColor);
            lightColorParameter.SetValue(diffuseLightColor);
            lightDirectionParameter.SetValue(diffuseLightDirection);
            //LightPositionParameter.SetValue(LightPosition);
            EyePositionParameter.SetValue(eyePosition);

            DrawSampleMesh(meshes[0]);


            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);

            //base.Draw(gameTime);

        }

        /// <summary>
        /// Example 1.6
        /// 
        /// Draws a sample mesh using a single effect with a single technique.
        /// This pattern is very common in simple effect usage.
        /// </summary>
        /// <param name="sampleMesh"></param>
        public void DrawSampleMesh(Model sampleMesh)
        {
            if (sampleMesh == null)
                return;

            //our sample meshes only contain a single part, so we don't need to bother
            //looping over the ModelMesh and ModelMeshPart collections. If the meshes
            //were more complex, we would repeat all the following code for each part
            ModelMesh mesh = sampleMesh.Meshes[0];
            ModelMeshPart meshPart = mesh.MeshParts[0];

            //set the vertex source to the mesh's vertex buffer
            ScreenManager.Game.GraphicsDevice.Vertices[0].SetSource(
                mesh.VertexBuffer, meshPart.StreamOffset, meshPart.VertexStride);

            //set the vertex delclaration
            ScreenManager.Game.GraphicsDevice.VertexDeclaration = meshPart.VertexDeclaration;

            //set the current index buffer to the sample mesh's index buffer
            ScreenManager.Game.GraphicsDevice.Indices = mesh.IndexBuffer;

            //figure out which effect we're using currently
            Effect effect;
            //if (enableAdvancedEffect) effect = vertexLightingEffect;
          //  if (enableAdvancedEffect) 
                effect = celLightingEffect;


            //at this point' we're ready to begin drawing
            //To start using any effect, you must call Effect.Begin
            //to start using the current technique (set in LoadGraphicsContent)
            effect.Begin(SaveStateMode.None);

            //now we loop through the passes in the teqnique, drawing each
            //one in order
            for (int i = 0; i < effect.CurrentTechnique.Passes.Count; i++)
            {
                //EffectPass.Begin will update the device to
                //begin using the state information defined in the current pass
                effect.CurrentTechnique.Passes[i].Begin();

                //sampleMesh contains all of the information required to draw
                //the current mesh
                ScreenManager.Game.GraphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList, meshPart.BaseVertex, 0,
                    meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);

                //EffectPass.End must be called when the effect is no longer needed
                effect.CurrentTechnique.Passes[i].End();
            }

            //Likewise, Effect.End will end the current technique
            effect.End();
        }

        /// <summary>
        /// Example 1.3
        /// This function obtains EffectParameter objects from the Effect objects.
        /// The EffectParameters are handles to the values in the shaders and are
        /// effectively how your C# code and your shader code communicate.
        /// </summary>
        private void GetEffectParameters()
        {


            //These parameters are shared in the same EffectPool.
            //Shared parameters use the "shared" keyword in the effect file
            //to indicate that they are shared between multiple effects.  In
            //the source code then, only one parameter is required for multiple
            //effects that share a parameter of the same type and name.
            worldParameter = celLightingEffect.Parameters["world"];
            viewParameter = celLightingEffect.Parameters["view"];
            projectionParameter = celLightingEffect.Parameters["projection"];


            //These effect parameters are only used by vertexLightingEffect
            //to indicate the lights' colors and direction
            lightColorParameter = celLightingEffect.Parameters["lightColor"];
            lightDirectionParameter = celLightingEffect.Parameters["LightDirection"];
            EyePositionParameter = celLightingEffect.Parameters["EyePosition"];
            LightPositionParameter = celLightingEffect.Parameters["LightPosition"];
            //ambientColorParameter = celLightingEffect.Parameters["ambientColor"];

        }

        /// <summary>
        /// Example 1.4
        /// 
        /// The effect parameters set in this function
        /// are shared between all of the rendered elements in the scene.
        /// </summary>
        private void SetSharedEffectParameters()
        {
            projectionParameter.SetValue(projection);
            viewParameter.SetValue(view);
            worldParameter.SetValue(world);
        }
        #endregion


    }


}
