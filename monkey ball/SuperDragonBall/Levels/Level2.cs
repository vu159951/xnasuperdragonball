﻿using System;
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
    public class Level2 : LevelData
    {
        public Level2(Game game, GameplayScreen host)
            : base(game, host)
        {

            startingLocation = new Vector3(10f, 35f, 0f);
            //make a few planes
            CollisionLevelPiece currentPlane;
            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 15;
            currentPlane.position += new Vector3(0f, 0f, 0);
            planes.Add(currentPlane);

            //moving level piece
            MovingLevelPiece movingPlane;
            movingPlane = new MovingLevelPiece(game, host, "checker_plane_3", new Vector3(0, -300f, 0), 80f);
            movingPlane.scale = 15;
            movingPlane.position += new Vector3(0f, 200f, -360f);
            planes.Add(movingPlane);

            movingPlane = new MovingLevelPiece(game, host, "checker_plane_3", new Vector3(0, -500f, 0), 80f);
            movingPlane.scale = 15;
            movingPlane.position += new Vector3(0f, 400f, -720f);
            planes.Add(movingPlane);

            movingPlane = new MovingLevelPiece(game, host, "checker_plane_3", new Vector3(0, -400f, 0), 80f);
            movingPlane.scale = 15;
            movingPlane.position += new Vector3(0f, 200f, -1080f);
            planes.Add(movingPlane);

            currentPlane = new CollisionLevelPiece(game, host, "checker_plane_3");
            currentPlane.scale = 15;
            currentPlane.position += new Vector3(0f, 0f, -1440f);
            planes.Add(currentPlane);


            //goal
            goal = new GoalObject(game, host);
            goal.position += new Vector3(0, 20, -1440f);
            goal.scale = 15;


            //death bound for this level
            m_fDeathBound = -150f;


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