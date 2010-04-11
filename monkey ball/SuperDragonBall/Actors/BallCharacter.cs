using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperDragonBall
{
    class BallCharacter: Actor
    {
        public BallCharacter(Game game, GameplayScreen host) : base(game, host)
        {
            modelName = "testBallThing";
            m_scale *= 5;

            quat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)Math.PI / 2);

            fMass = 10;
            bPhysicsDriven = true;
            fTerminalVelocity = 450;
           
        }

    }
}
