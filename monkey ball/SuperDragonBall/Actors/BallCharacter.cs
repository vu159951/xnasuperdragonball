using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperDragonBall
{
    public class BallCharacter: Actor
    {
        private bool collidedWithStage;
        private Vector3 damping;

        public BallCharacter(Game game, GameplayScreen host) : base(game, host)
        {
            //modelName = "testBallThing";
            modelName = "DragonBall";
            //modelName = "cloud_1";
            scale = 15;

            //quat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)Math.PI / 2);
            quat = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), 0);

            fMass = 10;
            bPhysicsDriven = true;
            fTerminalVelocity = 450;
            collidedWithStage = false;
            damping = new Vector3(0.99f, 0.99f, 0.99f);

            //rotationAxis = Vector3.Up;
            rotationVelocity = 1;

            mixCelWithTexture = true;
            celLightColor = Color.Gold.ToVector4();
            textureName = "dBallT";
            enableCelShading = true;
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

            //roll the ball
            Vector3 ballRotAxis = GetWorldFacing();
            ballRotAxis.Y = 0;
            //ballRotAxis = Vector3.Cross(ballRotAxis, Vector3.Up);
            //rotationAxis = ballRotAxis;
            rotationVelocity = m_velocity.Length() / 10;

            rotationAxis = new Vector3(0, 1, 0);
            //rotationVelocity = 10;

            base.Update(gameTime);
        }

        public bool CollidedWithStage {
            get { return collidedWithStage; }
            set { collidedWithStage = value; }
        }

    }
}
