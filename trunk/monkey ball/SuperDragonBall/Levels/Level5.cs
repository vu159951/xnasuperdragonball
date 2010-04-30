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
    public class Level5 : LevelData
    {
        public Level5(Game game, GameplayScreen host)
            : base(game, host)
        {

            startingLocation = new Vector3(1f, 905f, 1f);
            //make a few planes

            CollisionLevelPiece currentPlane;
            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.ScaleX = 30f;
            currentPlane.ScaleY = 15f;
            currentPlane.ScaleZ = 60f;

            currentPlane.position += new Vector3(0f, 440f, -400f);
            currentPlane.setLocalRotation(MathHelper.ToRadians(-30), 0);
            planes.Add(currentPlane);

            /*
            MovingLevelPiece movingPlane;
            movingPlane = new MovingLevelPiece(game, host, "checker_plane_3", new Vector3(300, 0f, 0), 85f);
            movingPlane.scale = 15;
            movingPlane.position += new Vector3(-150f, 0f, -360f);
            planes.Add(movingPlane);

            movingPlane = new MovingLevelPiece(game, host, "checker_plane_3", new Vector3(300, 0f, 0), -85f);
            movingPlane.scale = 15;
            movingPlane.position += new Vector3(150f, 0f, -720f);
            planes.Add(movingPlane);

            movingPlane = new MovingLevelPiece(game, host, "checker_plane_3", new Vector3(300, 0f, 0), 85f);
            movingPlane.scale = 15;
            movingPlane.position += new Vector3(-150f, 0f, -1080f);
            planes.Add(movingPlane);

            movingPlane = new MovingLevelPiece(game, host, "checker_plane_3", new Vector3(300, 0f, 0), -85f);
            movingPlane.scale = 15;
            movingPlane.position += new Vector3(150f, 0f, -1440f);
            planes.Add(movingPlane);
            */


            //goal
            goal = new GoalObject(game, host);
            goal.position += new Vector3(0, 300, -1540);
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