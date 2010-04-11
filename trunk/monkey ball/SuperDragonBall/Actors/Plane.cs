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
    public class Plane : Actor

    {
        private Quaternion originalRot;
        private float rotX, rotZ;
        private GraphicsDeviceManager graphics;
        private GraphicsDevice device;

        public Plane(Game game, GameplayScreen host)
            : base(game, host)
        {
            // TODO: Construct any child components here
            modelName = "checker_plane";
            position = new Vector3(0f, 0f, -10f);
            scale = 50;
            rotX = 0;
            rotZ = 0;
            quat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)Math.PI / 2);
            originalRot = quat;
            List<Vector3> verticies = new List<Vector3>();
            //effect.TextureEnabled = true;

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
            this.quat = originalRot * Quaternion.CreateFromRotationMatrix(Matrix.CreateFromYawPitchRoll(0, rotX, rotZ));
            base.Update(gameTime);
        }

        public bool testCollision(Actor ball) {
            return false;
        }

        public Vector3 TEST_getPlaneNormal() {
            IndexBuffer ind = model.Meshes[0].IndexBuffer;
            VertexBuffer vert = model.Meshes[0].VertexBuffer;
            //Console.Write("INDEX BUFFER: " + ind);
            return Vector3.Zero;
        }


        public float RotX {
            get { return rotX; }
            set { rotX = value; }
        }

        public float RotZ
        {
            get { return rotZ; }
            set { rotZ = value; }
        }
      
    }
}