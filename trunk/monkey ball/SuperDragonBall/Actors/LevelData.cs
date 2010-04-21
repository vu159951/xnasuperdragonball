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


namespace SuperDragonBall.Actors
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class LevelData : Actor
    {
        protected List<LevelPiece> planes;
        protected Vector3 gravityVec;
        public Vector3 startingLocation;
        protected GoalObject goal;

        public LevelData(Game game, GameplayScreen host)
            : base(game, host)
        {

            gravityVec = new Vector3(0, -100, 0);
            planes = new List<LevelPiece>();
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

        public void MovePlayer(BallCharacter player, GameTime gameTime)
        {

            float timeDelta = (float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond / 1000;

            player.velocity += gravityVec * timeDelta;

            //test for collision
            Vector3 pushAway = Vector3.Zero;
            if (planes.Count != 0)
            {
                foreach (LevelPiece p in planes)
                {
                    pushAway += p.testCollision(player);
                }
            }

            if (pushAway != Vector3.Zero)
            {
                pushAway.Normalize();
                player.CollidedWithStage = true;
                respondToCollision(pushAway, timeDelta, player);
            }
            else
            {
                player.CollidedWithStage = false;
            }

            //reset player position when fallen off
            if (player.position.Y < -100)
            {
                player.position = new Vector3(0f, 25f, 0f);
                player.velocity = Vector3.Zero;
                player.netForce = Vector3.Zero;
            }
        }

        public Boolean IsCollidingWithGoal(BallCharacter player)
        {
            if (goal != null)
            {
                if (goal.WorldBoundSphere.Intersects(player.WorldBoundSphere))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Collision resolution. Kind of a hack.
        /// Called from the Update function
        /// Tweak numbers until the feel is "right"
        /// </summary>
        /// <param name="pushAway">The normalized vector that the ball should be pushed away</param>
        /// <param name="timeDelta"></param>
        private void respondToCollision(Vector3 pushAway, float timeDelta, BallCharacter player)
        {
            //Vector3 v3 = m_kPlane.getPlaneNormal();
            Vector3 vDiff = player.velocity;
            vDiff.X += pushAway.X * 10;
            vDiff.Z += pushAway.Z * 10;
            if (vDiff.Y < -50)
            {
                vDiff.Y /= 1.5f;
            }
            vDiff.Y += pushAway.Y;
            vDiff.Y -= (gravityVec.Y * timeDelta) * 2;
            player.velocity = vDiff;
        }

        public void setRotation(float RotX, float RotZ, Vector3 playerPosition)
        {
            foreach (LevelPiece p in planes)
            {
                p.RotX = RotX;
                p.RotZ = RotZ;
                p.setRotationOffset(playerPosition);
            }

            if (goal != null)
            {
                goal.RotX = RotX;
                goal.RotZ = RotZ;
                goal.setRotationOffset(playerPosition);
            }
        }

        protected override void UnloadContent()
        {
            Console.WriteLine("test unload");
            foreach (LevelPiece lp in planes)
            {
                Game.Components.Remove(lp);
            }
            Game.Components.Remove(goal);
            base.UnloadContent();
        }


    }
}