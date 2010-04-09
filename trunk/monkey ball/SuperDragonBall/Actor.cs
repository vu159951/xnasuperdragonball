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


namespace GameStateManagement
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Actor : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Model model;
        public Matrix worldTransform;
        protected ContentManager contentManager;
        public static String modelName;
        protected Utils.Timer timer;
        protected Matrix[] boneTransforms;
        public static float Bounds =600f;
        private float MaxX = Bounds;
        private float MaxY = Bounds;

        public float fMass=10;
        public float fTerminalVelocity=10;
        public Vector3 vForce=new Vector3(0f,0f,0f);
        protected Vector3 vAcceleration = new Vector3(0f, 0f, 0f);
        public bool bPhysicsDriven=false;
        public bool isWall = false;

        public BoundingSphere ModelBounds;
        public BoundingSphere WorldBounds;

        public Vector3 velocity = new Vector3(0);

        private bool m_bChanged = false;
        

        private float m_kScale=1.0f;
        public float Scale
        {
            get
            {
                return m_kScale;
            }
            set
            {
                m_kScale = value;
                m_bChanged = true;
            }
        }


        private Vector3 m_kPosition = new Vector3(0f, 0f, 0f);
        public Vector3 Position
        {
            get
            {
                return m_kPosition;
            }
            set
            {
                m_kPosition = value;
                
                m_bChanged = true;
            }
        }


        private Quaternion m_kQuat = Quaternion.Identity;
        public Quaternion Quat
        {
            get
            {
                return m_kQuat;
            }
            set
            {
                m_kQuat = value;
                m_bChanged = true;
            }
        }


        public Actor(Game game)
            : base(game)
        {
            timer = new Utils.Timer();
            worldTransform = Matrix.Identity;
            

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
       
            base.Update(gameTime);
            timer.Update(gameTime);
            if (!isWall)
            {
                if (bPhysicsDriven)
                {
                    velocity += vAcceleration * ((float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond) / 2.0f;
                    if (velocity.LengthSquared() > (fTerminalVelocity * fTerminalVelocity))
                    {
                        velocity = Vector3.Normalize(velocity) * fTerminalVelocity;
                    }
                    Position += velocity * ((float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond) / 30f;
                    vAcceleration = vForce / fMass;
                    velocity += vAcceleration * ((float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond) / 2.0f;
                    if (velocity.LengthSquared() > (fTerminalVelocity * fTerminalVelocity))
                    {
                        velocity = Vector3.Normalize(velocity) * fTerminalVelocity;
                        //vForce.Normalize();
                    }
                }
                else
                {
                    //Position += velocity * ((float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond) / 30.0f;
                }
                if (m_kPosition.X > MaxX)
                {
                    m_kPosition.X = MaxX -5;
                    vForce = new Vector3(0, 0, 0);
                }
                else if (m_kPosition.X < -MaxX)
                {
                    m_kPosition.X = -MaxX+5;
                    vForce = new Vector3(0, 0, 0);
                }
                if (m_kPosition.Y > MaxY)
                {
                    m_kPosition.Y = MaxY -5;
                    vForce = new Vector3(0, 0, 0);
                }
                else if (m_kPosition.Y < -MaxY)
                {
                    m_kPosition.Y = -MaxY +5;
                    vForce = new Vector3(0, 0, 0);
                }
            }
            

            if (m_bChanged)
            {
                recalculateWolrdTransform();
                m_bChanged = false;
            }
        }

        protected override void LoadContent()
        {

            base.LoadContent();
            contentManager = new ContentManager(Game.Services, "Content");
            model = contentManager.Load<Model>("meshes/" + modelName);
            boneTransforms = new Matrix[model.Bones.Count];
            foreach (ModelMesh mesh in model.Meshes)
            {
                ModelBounds = BoundingSphere.CreateMerged(ModelBounds,
                mesh.BoundingSphere);
            }
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            contentManager.Unload();
        }

        public override void Draw(GameTime gameTime)
        {
            
            base.Draw(gameTime);
            GraphicsDevice.RenderState.DepthBufferEnable = true;
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World =boneTransforms[mesh.ParentBone.Index] * worldTransform;
                    effect.View = GameplayScreen.CameraMatrix;
                    effect.Projection = GameplayScreen.ProjectionMatrix;
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.AmbientLightColor = GameplayScreen.AmbientLightColor;
                    effect.SpecularPower = GameplayScreen.SpecularPower;
                    effect.SpecularColor = GameplayScreen.SpecularColor;
                    effect.DirectionalLight0.Direction=GameplayScreen.DLightDirection;
                    effect.DirectionalLight0.DiffuseColor = GameplayScreen.DLightColor;
                    
                  
                }
                mesh.Draw();
            }
        }

        public Vector3 GetWorldFacing()
        {
            return worldTransform.Forward;
        }

        public Vector3 GetWorldPosition()
        {
            return worldTransform.Translation;
        }

        public void recalculateWolrdTransform()
        {
            //Matrix temp = Matrix.Identity;
            worldTransform =Matrix.CreateScale(m_kScale);
            worldTransform  *= Matrix.CreateFromQuaternion(m_kQuat);
            worldTransform  *= Matrix.CreateTranslation(m_kPosition);
            WorldBounds.Center = m_kPosition;
            WorldBounds.Radius = ModelBounds.Radius * Scale;
        
        }
    }
}