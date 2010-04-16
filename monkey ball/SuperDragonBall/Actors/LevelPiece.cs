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
       
        private float m_rotX, m_rotZ;
        // a function of player position relative to the origin of the plane
        private Vector3 m_rotationOffset;
        //the rotation of the level piece independent of player movement
        private Quaternion m_localRotation;

        //data extracted from the mesh
        private List<Vector3> verticies;
        private List<MeshDataExtractor.TriangleVertexIndices> TVIndices;
        //public const Vector3 OriginalFacing

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
            TVIndices = new List<MeshDataExtractor.TriangleVertexIndices>();
            verticies = new List<Vector3>();
            m_rotationOffset = Vector3.Zero;
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

        protected override void LoadContent()
        {
            base.LoadContent();

            modifyWorldTransform();
            // extract mesh data in object space
            Matrix identity = Matrix.Identity;
            MeshDataExtractor.ExtractModelMeshData(model.Meshes[0], ref identity, verticies, TVIndices, modelName, true);
            printExtractedData();
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
            worldTransform = Matrix.CreateScale(m_scale) * Matrix.CreateFromQuaternion(m_localRotation) * tiltOffset * Matrix.CreateTranslation(m_position);
            WorldBounds.Center = m_position;
            WorldBounds.Radius = ModelBounds.Radius * m_scale;

        }

        public Vector3 testCollision(Actor ball)
        {
            //the normalized direction that the actor should be pushed away
            Vector3 pushAwayDir = Vector3.Zero;
            
            
            for (int i = 0; i < TVIndices.Count; i++)
            {
                MeshDataExtractor.TriangleVertexIndices currentTVI = TVIndices[i];
                
                /*
                Vector3[] currentTriangle = { Vector3.Zero, Vector3.Zero, Vector3.Zero };
                currentTriangle[0] = new Vector3(verticies[currentTVI.I0].X, verticies[currentTVI.I0].Y, verticies[currentTVI.I0].Z);
                currentTriangle[1] = new Vector3(verticies[currentTVI.I1].X, verticies[currentTVI.I1].Y, verticies[currentTVI.I1].Z);
                currentTriangle[2] = new Vector3(verticies[currentTVI.I2].X, verticies[currentTVI.I2].Y, verticies[currentTVI.I2].Z);
                currentTriangle[0] = Vector3.Transform(currentTriangle[0], worldTransform);
                currentTriangle[1] = Vector3.Transform(currentTriangle[1], worldTransform);
                currentTriangle[2] = Vector3.Transform(currentTriangle[2], worldTransform);
                
                */
                /*
                Vector3 v0 = verticies[currentTVI.I0];
                Vector3 v1 = verticies[currentTVI.I1];
                Vector3 v2 = verticies[currentTVI.I2];
                v0 *= m_scale;
                v1 *= m_scale;
                v2 *= m_scale;
                Vector3[] currentTriangle = {  v0, v1, v2 };
                */
                
                Vector3[] currentTriangle = {  Vector3.Transform(verticies[currentTVI.I0], worldTransform), 
                                               Vector3.Transform(verticies[currentTVI.I1], worldTransform), 
                                               Vector3.Transform(verticies[currentTVI.I2], worldTransform) };
                
                if (IntersectHelper.sphereTriangleIntersect(ball.WorldBoundSphere, currentTriangle))
                {
                    Console.WriteLine("HIT " + i);
                    pushAwayDir += getTriangleNormal(currentTriangle);
                    //return true;
                }
            }

            if (pushAwayDir != Vector3.Zero)
            {
                pushAwayDir.Normalize();
            }
            else {
                Console.WriteLine("MISS");
            }
            return pushAwayDir;
            //return false;

        }

        private Vector3 getTriangleNormal(Vector3[] tri)
        {
            Vector3 va = tri[0] - tri[1];
            Vector3 vb = tri[1] - tri[2];
            Vector3 n = Vector3.Cross(vb, va);
            //n.Normalize();
            return n;
        }


        public void printExtractedData()
        {
            Console.WriteLine("Verticies:");
            foreach (Vector3 v in verticies)
            {
                Console.WriteLine(v);
            }
            Console.WriteLine("\nTriangle Vertex Indicies:");
            foreach (MeshDataExtractor.TriangleVertexIndices t in TVIndices)
            {
                Console.WriteLine("{0}, {1}, {2}", t.I0, t.I1, t.I2);
            }
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


        //returns an arbitrary point on the plane
        public Vector3 getPlanePoint()
        {
            return Vector3.Transform(verticies[0], worldTransform);
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