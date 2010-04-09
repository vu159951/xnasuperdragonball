using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperDragonBall
{
    class BallCharacter: Actor
    {
        public BallCharacter(Game game) : base(game)
        {
            modelName = "testBallThing";
            //m_scale *= 5;

            quat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)Math.PI / 2);

            //GameplayScreen.soundbank.PlayCue("Ship_Spawn");

            fMass = 10;
            bPhysicsDriven = true;
            fTerminalVelocity = 450;
           
        }

    }
}
