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

using SuperDragonBall;
using SuperDragonBall.Utils;

namespace SuperDragonBall
{



    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class LevelPiece : Actor
    {
        private Quaternion originalRot;

        protected float m_fScaleX, m_fScaleY, m_fScaleZ;

        public float ScaleX {
            get { return m_fScaleX; }
            set 
            { 
                m_fScaleX = value;
                m_changed = true;
            }
        }
        public float ScaleY
        {
            get { return m_fScaleY; }
            set { 
                m_fScaleY = value;
                m_changed = true;
            }
        }
        public float ScaleZ
        {
            get { return m_fScaleZ; }
            set
            {
                m_fScaleZ = value;
                m_changed = true;
            }
        }

        private float m_rotX, m_rotZ;
        // a function of player position relative to the origin of the plane
        protected Vector3 m_rotationOffset;
        //the rotation of the level piece independent of player movement
        protected Quaternion m_localRotation;

        public LevelPiece(Game game, GameplayScreen host, String assetName)
            : base(game, host)
        {
            // TODO: Construct any child components here
            modelName = assetName;
            position = new Vector3(0f, 0f, 0f);
            // DO NOT ADJUST SCALE IN A CONSTRUCTOR
            //scale = 50;
            m_rotX = 0;
            m_rotZ = 0;
            //quat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)Math.PI / 2);
            quat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), 0);
            originalRot = quat;
            m_rotationOffset = Vector3.Zero;
            //effect.TextureEnabled = true;
            m_localRotation = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), 0);

            m_fScaleX = 1.0f;
            m_fScaleY = 1.0f;
            m_fScaleZ = 1.0f;

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

        protected override void LoadContent()
        {
            base.LoadContent();

           
        }

        protected override void UnloadContent()
        {       
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            this.quat = originalRot * Quaternion.CreateFromRotationMatrix(Matrix.CreateFromYawPitchRoll(0, 
                m_rotX, m_rotZ));
            base.Update(gameTime);
        }

        /// <summary>
        /// Plane rotation is dependent on player postion
        /// </summary>
        protected override void modifyWorldTransform()
        {
            m_changed = false;
            Matrix rom = Matrix.CreateTranslation(m_rotationOffset);
            Matrix rom2 = Matrix.CreateTranslation(-m_rotationOffset);
            Matrix tiltOffset = rom * Matrix.CreateFromQuaternion(m_quat) * rom2;
            worldTransform = Matrix.CreateScale(m_scale*m_fScaleX, m_scale*m_fScaleY, m_scale*m_fScaleZ) 
                * Matrix.CreateFromQuaternion(m_localRotation) * tiltOffset * Matrix.CreateTranslation(m_position);
            WorldBounds.Center = m_position;
            WorldBounds.Radius = ModelBounds.Radius * m_scale;

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
  
        }

        public float RotX
        {
            get { return m_rotX; }
            set { m_rotX = value; }
        }

        public float RotZ
        {
            get { return m_rotZ; }
            set { m_rotZ = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotX">In radians</param>
        /// <param name="rotZ">In radians</param>
        public void setLocalRotation(float pRotX, float pRotZ) {
            m_localRotation = originalRot * Quaternion.CreateFromYawPitchRoll(0, pRotX, pRotZ);
        }

        /// <summary>
        /// Used to keep the plane rotating around the player's postion
        /// Should be set every time the player's postion changes
        /// </summary>
        /// <param name="playerPosition"></param>
        public void setRotationOffset(Vector3 playerPosition)
        {
            m_rotationOffset = this.position - playerPosition;
        }



    }
}