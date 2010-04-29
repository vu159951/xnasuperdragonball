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
using SuperDragonBall.Actors;

namespace SuperDragonBall.Levels
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Level4 : LevelData
    {
        public Level4(Game game, GameplayScreen host)
            : base(game, host)
        {

            startingLocation = new Vector3(1f, 35f, 100f);
            //make a few planes

            CollisionLevelPiece currentPlane;
            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 15;
            currentPlane.position += new Vector3(0f, 0f, 0);
            planes.Add(currentPlane);

            //Left arm


            //large arm

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 9;
            currentPlane.position += new Vector3(-180f, 0f, -288);
            planes.Add(currentPlane);

            //smaller arms
            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 3;
            currentPlane.position += new Vector3(-252f, 0f, -432);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 3;
            currentPlane.position += new Vector3(-108f, 0f, -432);
            planes.Add(currentPlane);

            //tiny arm

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 1;
            currentPlane.position += new Vector3(-252f, 0f, -480);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 1;
            currentPlane.position += new Vector3(-108f, 0f, -480);
            planes.Add(currentPlane);


            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 1;
            currentPlane.position += new Vector3(-252f, 0f, -504);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 1;
            currentPlane.position += new Vector3(-108f, 0f, -504);
            planes.Add(currentPlane);


            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 1;
            currentPlane.position += new Vector3(-252f, 0f, -528);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 1;
            currentPlane.position += new Vector3(-108f, 0f, -528);
            planes.Add(currentPlane);


            //small arm

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 3;
            currentPlane.position += new Vector3(-252f, 0f, -576);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 3;
            currentPlane.position += new Vector3(-108f, 0f, -576);
            planes.Add(currentPlane);

            //large arm

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 9;
            currentPlane.position += new Vector3(-180f, 0f, -720);
            planes.Add(currentPlane);




            //Right arm

            //large arm

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 9;
            currentPlane.position += new Vector3(180f, 0f, -288);
            planes.Add(currentPlane);

            //smaller arms


            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 3;
            currentPlane.position += new Vector3(252f, 0f, -432);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 3;
            currentPlane.position += new Vector3(108f, 0f, -432);
            planes.Add(currentPlane);

            //tiny arms

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 1;
            currentPlane.position += new Vector3(252f, 0f, -480);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 1;
            currentPlane.position += new Vector3(108f, 0f, -480);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 1;
            currentPlane.position += new Vector3(252f, 0f, -504);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 1;
            currentPlane.position += new Vector3(108f, 0f, -504);
            planes.Add(currentPlane);


            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 1;
            currentPlane.position += new Vector3(252f, 0f, -528);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 1;
            currentPlane.position += new Vector3(108f, 0f, -528);
            planes.Add(currentPlane);


            //small arms

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 3;
            currentPlane.position += new Vector3(252f, 0f, -576);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 3;
            currentPlane.position += new Vector3(108f, 0f, -576);
            planes.Add(currentPlane);

            //large arm

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 9;
            currentPlane.position += new Vector3(180f, 0f, -720);
            planes.Add(currentPlane);
           


            //connecting bar

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 2;
            currentPlane.position += new Vector3(48f, 0f, -804);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 2;
            currentPlane.position += new Vector3(-48f, 0f, -804);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 2;
            currentPlane.position += new Vector3(0f, 0f, -804);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 2;
            currentPlane.position += new Vector3(0f, 0f, -852);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 2;
            currentPlane.position += new Vector3(0f, 0f, -900);
            planes.Add(currentPlane);





            //goal
            goal = new GoalObject(game, host);
            goal.position += new Vector3(0, 10, -900);
            goal.scale = 10;

            //collectables

            //death bound for this level
            m_fDeathBound = -250f;




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
        }
    }
}