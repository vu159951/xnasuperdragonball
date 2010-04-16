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
        protected static string modelName;
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

            base.Update(gameTime);
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

      

        

        /*
        public Vector3 GetWorldPosition() {
            return worldTransform.Translation;
        }
        */
    }
}