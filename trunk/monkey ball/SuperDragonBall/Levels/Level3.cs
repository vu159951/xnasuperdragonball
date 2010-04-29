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
    public class Level3 : LevelData
    {
        public Level3(Game game, GameplayScreen host)
            : base(game, host)
        {

            startingLocation = new Vector3(10f, 35f, 0f);
            //make a few planes
            CollisionLevelPiece currentPlane;
            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 15;
            currentPlane.position += new Vector3(0f, 0f, 0);
            planes.Add(currentPlane);

            /*
            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 15;
            currentPlane.position += new Vector3(0f, 0f, -360f);
            planes.Add(currentPlane);
            */

            //moving level piece
            MovingLevelPiece movingPlane;
            movingPlane = new MovingLevelPiece(game, host, "checker_plane_3", new Vector3(0, -500f, 0), 80f);
            movingPlane.scale = 15;
            movingPlane.position += new Vector3(0f, 400f, -360f);
            planes.Add(movingPlane);

            //wall
            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 15;
            currentPlane.position += new Vector3(0f, 160f, -550f);
            currentPlane.setLocalRotation((float)Math.PI / 2, 0);
            planes.Add(currentPlane);

            //cloud above wall
            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 15;
            currentPlane.position += new Vector3(0f, 300f, -730f);
            planes.Add(currentPlane);


            //goal
            goal = new GoalObject(game, host);
            goal.position += new Vector3(0, 300, -730);
            goal.scale = 10;


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