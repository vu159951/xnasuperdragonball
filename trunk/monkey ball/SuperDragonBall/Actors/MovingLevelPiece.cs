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
    public class MovingLevelPiece : CollisionLevelPiece
    {
        private Vector3 originalPosition;
        //a vector that describes where the piece should move to and from
        private Vector3 m_movement;
        private Vector3 m_moveVelocity;

        public MovingLevelPiece(Game game, GameplayScreen host, String assetName, Vector3 pMovement, float pMoveSpeed)
            : base(game, host, assetName)
        {
            
            m_movement = pMovement;
            m_moveVelocity = m_movement;
            if (m_moveVelocity.LengthSquared() != 0)
            {
                m_moveVelocity.Normalize();
            }
            //50 is a good number, fyi
            m_moveVelocity *= pMoveSpeed;


        }
      

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond / 1000;

            //determine if I need to switch velocity direction
            if (Math.Abs(position.LengthSquared() - originalPosition.LengthSquared()) > m_movement.LengthSquared()) {
                m_moveVelocity *= -1;
            } 
                
            // update the level piece position in the direction of its movement vector
            position += m_moveVelocity * timeDelta;

            base.Update(gameTime);
        }

        /// <summary>
        /// Original Position cannot be defined correctly at construction time
        /// </summary>
        public Vector3 OriginalPosition {
            set { originalPosition = value; }
        }

    }
}