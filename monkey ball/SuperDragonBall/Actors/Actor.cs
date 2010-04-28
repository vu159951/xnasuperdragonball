using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace SuperDragonBall
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Actor : Microsoft.Xna.Framework.DrawableGameComponent
    {

        protected Model model;
        public Matrix worldTransform;
        //variables to describe the position/scale/rotation of the actor
        protected float m_scale;
        protected Vector3 m_position;
        protected Quaternion m_quat;
        protected bool m_changed;

        //velocity
        protected Vector3 m_velocity;
        protected Vector3 m_rotationAxis;
        protected float m_rotationVelocity;
        //set to 1 for no speed up
        public const int SPEED_UP = 5;

        protected ContentManager contentManager;
        protected string modelName;
        protected Utils.Timer timer;
        protected Matrix[] bones;

        protected float fMass;
        protected float fTerminalVelocity;
        protected Vector3 vForce;
        protected Vector3 vAcceleration;
        protected bool bPhysicsDriven;

        protected BoundingSphere ModelBounds;
        protected BoundingSphere WorldBounds;

        protected GameplayScreen hostScreen;

        //Cel Shader enable property
        private bool _enableCelShading = false;
        public bool enableCelShading
        {
            get
            {
                return _enableCelShading;
            }
            set
            {
                _enableCelShading = value;
            }
        }

        //Cel Shader Effect objects
        private Effect celLightingEffect;
        private EffectParameter projectionParameter;
        private EffectParameter viewParameter;
        private EffectParameter worldParameter;
        private EffectParameter lightColorParameter;
        private EffectParameter lightDirectionParameter;
        //private EffectParameter ambientColorParameter;
        private EffectParameter EyePositionParameter;
        private EffectParameter LightPositionParameter;

        /// Data fields corresponding to the cel shader effect paramters
        private Matrix world, view, projection;
        private Vector3 diffuseLightDirection;
        private Vector4 diffuseLightColor;
        private Vector4 ambientLightColor;
        private Vector3 eyePosition;

        public Actor(Game game, GameplayScreen host)
            : base(game)
        {
            //the host contains useful things such as a camera matrix
            hostScreen = host;

            // TODO: Construct any child components here
            timer = new Utils.Timer();
            worldTransform = new Matrix();
            worldTransform = Matrix.Identity;

            // DO NOT ADJUST SCALE IN A CONSTRUCTOR
            m_scale = 1.0f;
            m_position = new Vector3(0.0f, 0.0f, 0.0f);
            m_quat = Quaternion.Identity;
            m_changed = true;
            modifyWorldTransform();

            m_velocity = new Vector3(0.0f, 0.0f, 0.0f);
            //default, rotate zero speed around the Up vector
            m_rotationAxis = Vector3.Up;
            m_rotationVelocity = 0.0f;

            fMass = 1;
            fTerminalVelocity = 350;
            vForce = Vector3.Zero;
            vAcceleration = Vector3.Zero;
            bPhysicsDriven = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (!PauseMenuScreen.IsPaused)
            {
                timer.Update(gameTime);

                float timeDelta = (float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond / 1000;


                if (bPhysicsDriven)
                {
                    m_velocity += vAcceleration * timeDelta / 2.0f;
                    m_position += m_velocity * timeDelta;
                    vAcceleration = vForce / fMass;
                    m_velocity += vAcceleration * timeDelta / 2.0f;

                    if (m_velocity.Length() > fTerminalVelocity)
                    {
                        m_velocity *= fTerminalVelocity / m_velocity.Length();
                    }
                }
                else
                {
                    //do it the previous way
                    //add velocity to position
                    this.position += m_velocity * timeDelta * SPEED_UP;
                }

                //rotate around axis            
                this.quat *= Quaternion.CreateFromAxisAngle(rotationAxis, rotationVelocity * timeDelta);

                // for all actors in the game
                //worldBoarder();

                //Cel Shading
                //Set the light direction to a fixed value.
                //This will place the light source behind, to the right, and above the user.
                diffuseLightDirection = new Vector3(-2.0f, 2, 0f);
                //ensure the light direction is normalized, or
                //the shader will give some weird results
                //diffuseLightDirection.Normalize();

                //set the color of the diffuse light
                diffuseLightColor = Color.White.ToVector4();

                //The built-in camera class provides the view matrix
                //view = camera.ViewMatrix;
                view = hostScreen.CameraMatrix;

                //eyePosition = camera.Position;
                eyePosition = hostScreen.GetCameraPosition();
                //LightPosition = new Vector3(50, 50, 50);
                //LightPosition.Normalize();


                base.Update(gameTime);
            }
        }

        protected override void LoadContent()
        {
            contentManager = new ContentManager(Game.Services, "Content/meshes");
            model = contentManager.Load<Model>(modelName);
            bones = new Matrix[model.Bones.Count];
            foreach (ModelMesh mesh in model.Meshes)
            {
                ModelBounds = BoundingSphere.CreateMerged(ModelBounds,
            mesh.BoundingSphere);
            }

            //Cel Shading
           // celLightingEffect = contentManager.Load<Effect>("Cel");
           // GetEffectParameters();

            //Calculate the projection properties first on any 
            //load callback.  That way if the window gets resized,
            //the perspective matrix is updated accordingly
            float aspectRatio = (float)GraphicsDevice.Viewport.Width /
                (float)GraphicsDevice.Viewport.Height;
            float fov = MathHelper.PiOver4 * aspectRatio * 3 / 4;

            //projection = Matrix.CreatePerspectiveFieldOfView(fov,
            //    aspectRatio, .1f, 1000f);
            projection = hostScreen.ProjectionMatrix;

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            Game.Components.Remove(this);
            contentManager.Unload();
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            //modify the world transform matrix if necessary
            if (m_changed)
            {
                modifyWorldTransform();
            }

            //do the drawing part
            GraphicsDevice.RenderState.DepthBufferEnable = true;
            model.CopyAbsoluteBoneTransformsTo(bones);

            if (_enableCelShading)
            {
                //Cel shading
                projectionParameter.SetValue(projection);
                viewParameter.SetValue(view);
                worldParameter.SetValue(world);
                lightColorParameter.SetValue(diffuseLightColor);
                lightDirectionParameter.SetValue(diffuseLightDirection);
                //LightPositionParameter.SetValue(LightPosition);
                EyePositionParameter.SetValue(eyePosition);

                DrawSampleMesh(model);
            }
            else
            {

                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = bones[mesh.ParentBone.Index] * worldTransform;
                        effect.View = hostScreen.CameraMatrix;
                        effect.Projection = hostScreen.ProjectionMatrix;
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;

                        effect.AmbientLightColor = hostScreen.AmbientLightColor;
                        effect.SpecularColor = hostScreen.SpecularColor;
                        effect.SpecularPower = hostScreen.SpecularPower;
                        effect.DirectionalLight0.Direction = hostScreen.DLightDirection;
                        effect.DirectionalLight0.DiffuseColor = hostScreen.DLightColor;

                        //effect.TextureEnabled = true;
                    }
                    mesh.Draw();
                }
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Should be called before draw functions are done
        /// </summary>
        protected virtual void modifyWorldTransform()
        {
            m_changed = false;
            worldTransform = Matrix.CreateScale(m_scale) * Matrix.CreateFromQuaternion(m_quat) * Matrix.CreateTranslation(m_position);
            WorldBounds.Center = m_position;
            WorldBounds.Radius = ModelBounds.Radius * m_scale;
        }

        //all actors in this test class stop at the edges of the world
        private void worldBoarder()
        {
            Vector3 currentP = position;
            Vector3 nextVel = Vector3.Zero;
            bool brokeWorldBounds = false;

            float boundX = WallManager.WallBoundsX;
            float boundZ = WallManager.WallBoundsZ;
            //float boundX = GameStateManagementGame.SCREEN_WIDTH / 2 + GameStateManagementGame.BUFFER_W;
            //float boundY = GameStateManagementGame.SCREEN_HEIGHT / 2 + GameStateManagementGame.BUFFER_H;

            if (currentP.X < -boundX)
            {
                nextVel.X += 30;
                currentP.X = -boundX;
                brokeWorldBounds = true;
                //currentP.X += boundX * 2;
            }
            else if (currentP.X > boundX)
            {
                nextVel.X -= 30;
                currentP.X = boundX;
                brokeWorldBounds = true;
                //currentP.X -= boundX * 2;
            }
            if (currentP.Z < -boundZ)
            {
                nextVel.Z += 30;
                currentP.Z = -boundZ;
                brokeWorldBounds = true;
                //currentP.Y += boundY * 2;
            }
            else if (currentP.Z > boundZ)
            {
                nextVel.Z -= 30;
                currentP.Z = boundZ;
                brokeWorldBounds = true;
                //currentP.Y -= boundY * 2;
            }
            if (brokeWorldBounds)
            {
                //position = currentP;
                velocity = nextVel;
            }
        }


        //
        // Getters
        //

        public Quaternion quat
        {
            get
            {
                return m_quat;
            }
            set
            {
                m_quat = value;
                m_changed = true;
            }
        }

        public Vector3 position
        {
            get
            {
                return m_position;
            }
            set
            {
                m_position = value;
                m_changed = true;
            }
        }

        public Vector3 velocity
        {
            get
            {
                return m_velocity;
            }
            set
            {
                m_velocity = value;
            }
        }

        public Vector3 netForce
        {
            get
            {
                return vForce;
            }
            set
            {
                vForce = value;
                m_changed = true;
            }
        }


        public Vector3 directionVec
        {
            get
            {
                //return Matrix.CreateFromQuaternion(quat) * Vector3.One;
                Vector3 dir = worldTransform.Forward;
                dir.Normalize();
                return dir;
            }
        }

        public Vector3 rotationAxis
        {
            get
            {
                return m_rotationAxis;
            }
            set
            {
                //normalize when set
                m_rotationAxis = new Vector3(value.X, value.Y, value.Z);
                m_rotationAxis.Normalize();
                m_changed = true;
            }
        }

        public float rotationVelocity
        {
            get
            {
                return m_rotationVelocity;
            }
            set
            {
                m_rotationVelocity = value;
                m_changed = true;
            }
        }

        public float scale
        {
            get
            {
                return m_scale;
            }
            set
            {
                m_scale = value;
                m_changed = true;
            }
        }

        public BoundingSphere WorldBoundSphere
        {
            get
            {
                return WorldBounds;
            }
        }

        public Vector3 GetWorldFacing()
        {
            return worldTransform.Forward;
        }

        /// <summary>
        /// This function obtains EffectParameter objects from the Effect objects.
        /// The EffectParameters are handles to the values in the shaders and are
        /// effectively how your C# code and your shader code communicate.
        /// </summary>
        private void GetEffectParameters()
        {
            worldParameter = celLightingEffect.Parameters["world"];
            viewParameter = celLightingEffect.Parameters["view"];
            projectionParameter = celLightingEffect.Parameters["projection"];
            lightColorParameter = celLightingEffect.Parameters["lightColor"];
            lightDirectionParameter = celLightingEffect.Parameters["LightDirection"];
            EyePositionParameter = celLightingEffect.Parameters["EyePosition"];
            LightPositionParameter = celLightingEffect.Parameters["LightPosition"];
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

            foreach (ModelMesh mesh in sampleMesh.Meshes)
            {
                
                worldParameter.SetValue(bones[mesh.ParentBone.Index] * worldTransform);
                lightColorParameter.SetValue(new Vector4(hostScreen.SpecularColor, 1.0f));
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    //our sample meshes only contain a single part, so we don't need to bother
                    //looping over the ModelMesh and ModelMeshPart collections. If the meshes
                    //were more complex, we would repeat all the following code for each part
                    //  ModelMesh mesh = sampleMesh.Meshes[0];
                    //  ModelMeshPart meshPart = mesh.MeshParts[0];

                    //set the vertex source to the mesh's vertex buffer
                    GraphicsDevice.Vertices[0].SetSource(
                        mesh.VertexBuffer, meshPart.StreamOffset, meshPart.VertexStride);

                    //set the vertex delclaration
                    GraphicsDevice.VertexDeclaration = meshPart.VertexDeclaration;

                    //set the current index buffer to the sample mesh's index buffer
                    GraphicsDevice.Indices = mesh.IndexBuffer;

                    //figure out which effect we're using currently
                    Effect effect = celLightingEffect;


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
                        GraphicsDevice.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList, meshPart.BaseVertex, 0,
                            meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);

                        //EffectPass.End must be called when the effect is no longer needed
                        effect.CurrentTechnique.Passes[i].End();
                    }

                    //Likewise, Effect.End will end the current technique
                    effect.End();
                }
            }
        }





        /*
        public Vector3 GetWorldPosition() {
            return worldTransform.Translation;
        }
        */
    }
}