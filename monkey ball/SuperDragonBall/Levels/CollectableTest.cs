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
    public class CollectableTest : LevelData
    {
        public CollectableTest(Game game, GameplayScreen host)
            : base(game, host)
        {

            startingLocation = new Vector3(10f, 25f, 0f);
            //make a few planes
            CollisionLevelPiece currentPlane;
            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 15;
            currentPlane.position += new Vector3(0f, -10f, 0);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 10;
            currentPlane.position += new Vector3(200f, -10f, -300f);
            currentPlane.setLocalRotation(0, (float)Math.PI / 18);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 10;
            currentPlane.position += new Vector3(-250f, -10f, -200f);
            currentPlane.setLocalRotation(0.123f, -(float)Math.PI / 18);
            planes.Add(currentPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 5;
            currentPlane.position += new Vector3(-220f, 10f, 80f);
            planes.Add(currentPlane);

            

            //moving level piece
            MovingLevelPiece movingPlane;
            //50 is a good movement speed
            movingPlane = new MovingLevelPiece(game, host, "checker_plane_3", new Vector3(0, 100f, 0f), 50);
            movingPlane.scale = 5;
            movingPlane.position += new Vector3(0f, 0f, -250f);
            //CRITICAL!!!
            movingPlane.OriginalPosition = movingPlane.position;
            planes.Add(movingPlane);

            goal = new GoalObject(game, host);
            goal.position += new Vector3(0, 10, -50);
            goal.scale = 5;
            //game.Components.Add(goal);

            //foreach (LevelPiece p in planes)
            //{
            //    game.Components.Add(p);
            //}

            Collectable collect1 = new Collectable(game, host);
            collect1.position = new Vector3(40, 10, 40);
            collectables.Add(collect1);

            //foreach (Collectable collect in collectables)
            //{
            //    game.Components.Add(collect);
            //}
            

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