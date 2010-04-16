using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperDragonBall
{
    class BallCharacter: Actor
    {
        private bool collidedWithStage;
        private Vector3 damping;

        public BallCharacter(Game game, GameplayScreen host) : base(game, host)
        {
            modelName = "testBallThing";
            m_scale = 10;

            //quat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)Math.PI / 2);
            quat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), 0);

            fMass = 10;
            bPhysicsDriven = true;
            fTerminalVelocity = 450;
            collidedWithStage = false;
            damping = new Vector3(0.99f, 0.99f, 0.99f);
           
        }

        public override void Update(GameTime gameTime)
        {
            //DAMPING
            m_velocity *= damping;
            if (Math.Abs(m_velocity.X) < 0.01) {
                m_velocity.X = 0f;
            }
            if (Math.Abs(m_velocity.Y) < 0.01)
            {
                m_velocity.Y = 0f;
            }
            if (Math.Abs(m_velocity.Z) < 0.01)
            {
                m_velocity.Z = 0f;
            }
            base.Update(gameTime);
        }

        public bool CollidedWithStage {
            get { return collidedWithStage; }
            set { collidedWithStage = value; }
        }

    }
}
