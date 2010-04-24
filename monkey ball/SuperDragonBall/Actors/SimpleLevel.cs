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


namespace SuperDragonBall.Actors
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SimpleLevel : LevelData
    {
        public SimpleLevel(Game game, GameplayScreen host)
            : base(game, host)
        {

            startingLocation = new Vector3(10f, 25f, 0f);
            //make a few planes
            LevelPiece currentPlane;
            currentPlane = new LevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 15;
            currentPlane.position += new Vector3(0f, -10f, 0);
            planes.Add(currentPlane);





            goal = new GoalObject(game, host);
            goal.position += new Vector3(40, 10, -20);
            goal.scale = 5;
            

           


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