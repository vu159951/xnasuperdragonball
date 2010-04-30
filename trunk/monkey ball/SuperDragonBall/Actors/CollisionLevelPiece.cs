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
    public class CollisionLevelPiece : LevelPiece
    {

        //data extracted from the mesh
        private List<Vector3> verticies;
        private List<MeshDataExtractor.TriangleVertexIndices> TVIndices;
        //public const Vector3 OriginalFacing

        //the non-colliding asset of the the level piece
        private LevelPieceAsset nonCollideAsset;

        public CollisionLevelPiece(Game game, GameplayScreen host, String assetName)
            : base(game, host, assetName)
        {
            //all collision pieces are checker planes
            modelName = "checker_plane_3";
            TVIndices = new List<MeshDataExtractor.TriangleVertexIndices>();
            verticies = new List<Vector3>();
       
            nonCollideAsset = new LevelPieceAsset(game, host, "cloud_2");
            //nonCollideAsset = new LevelPieceAsset(game, host, "DragonBall");
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

            //modifyWorldTransform();
            // extract mesh data in object space
            Matrix identity = Matrix.Identity;
            MeshDataExtractor.ExtractModelMeshData(model.Meshes[0], ref identity, verticies, TVIndices, modelName, true);
            printExtractedData();
        }

        protected override void UnloadContent()
        {
            Game.Components.Remove(nonCollideAsset);
            base.UnloadContent();
        }

        /// <summary>
        /// Damnit why can't UnloadContent just work?
        /// </summary>
        public void removeAsset() {
            Game.Components.Remove(nonCollideAsset);
        }

        //needed for the levels to work again for a second playthrough
        public void addAsset(Game game)
        {
            game.Components.Add(nonCollideAsset);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            base.Update(gameTime);
            //match the nonColideAssetProperties
            nonCollideAsset.quat = this.quat;
            nonCollideAsset.position = this.position;
            nonCollideAsset.scale = this.scale;
            nonCollideAsset.ScaleX = this.m_fScaleX;
            nonCollideAsset.ScaleY = this.m_fScaleY;
            nonCollideAsset.ScaleZ = this.m_fScaleZ;
            nonCollideAsset.matchRotationOffset(m_rotationOffset);
            nonCollideAsset.matchLocalRotation(m_localRotation);
           
        }

        public override void Draw(GameTime gameTime)
        {
            //!!!!!!!!!!!!!!!!!!
            //DON'T DRAW MY MESH
            //!!!!!!!!!!!!!!!!!!
            //base.Draw(gameTime);

            //modify the world transform matrix if necessary
            if (m_changed)
            {
                modifyWorldTransform();
            }



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
                    //Console.WriteLine("HIT " + i);
                    pushAwayDir += getTriangleNormal(currentTriangle);
                    //return true;
                }
            }

            if (pushAwayDir != Vector3.Zero)
            {
                pushAwayDir.Normalize();
            }
            else
            {
                //Console.WriteLine("MISS");
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

        //returns an arbitrary point on the plane
        public Vector3 getPlanePoint()
        {
            return Vector3.Transform(verticies[0], worldTransform);
        }


    }
}