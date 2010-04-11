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


namespace SuperDragonBall
{
    public class Ship : Actor
    {

        public bool isAlive;

        public Ship(Game game, GameplayScreen host)
            : base(game, host)
        {
           // modelName = "testBallThing";
            modelName = "Ship";
            //m_scale *= 5;
            
            //quat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)Math.PI / 2);
            quat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), 0);
            //GameplayScreen.soundbank.PlayCue("Ship_Spawn");
            
            fMass = 10;
            bPhysicsDriven = true;
            fTerminalVelocity = 450;
            isAlive = true;
           

        }
        public override void Initialize()
        {
            base.Initialize();
       
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
           
        }

        public void turnLeft(GameTime gameTime)
        {
            rotationVelocity += 0.3f;
            //Vector3 rotationAxis = new Vector3(0f,1f,0f);
            //quat *= Quaternion.CreateFromAxisAngle(rotationAxis, (float)(Math.PI / 512) * ((float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond));
            
        }

        public void turnRight(GameTime gameTime)
        {
            rotationVelocity -= 0.3f;
            //Vector3 rotationAxis = new Vector3(0f, 1f, 0f);
            //quat *= Quaternion.CreateFromAxisAngle(rotationAxis, (float)(Math.PI / -512) * ((float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond));
           
        }
        public void moveForward(GameTime gameTime)
        {
            netForce += 450f * directionVec;
            //vForce += Vector3.Normalize(GetWorldFacing()) * ((float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond);
          
        }

        public void moveReverse(GameTime gameTime)
        {
            netForce -= 450f * directionVec;
            //vForce += Vector3.Normalize(GetWorldFacing()) * ((float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond);

        }


        public void stop()
        {
            netForce = new Vector3(0, 0, 0);
            velocity = new Vector3(0, 0, 0);

        }


        public void brake(GameTime gameTime)
        {
            if (velocity.Length() < .1)
            {
                velocity = new Vector3(0f, 0f, 0f);
                vForce = new Vector3(0f,0f,0f);
            }
            else
            {
                vForce -= Vector3.Normalize(velocity) * ((float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond);
            }//velocity = GetWorldFacing() * 10;
        }

    }
}
