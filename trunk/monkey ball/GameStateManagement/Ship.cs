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


namespace GameStateManagement
{
    public class Ship : Actor
    {
        private bool canShoot = true;
        private int missileFired = 0;
        public List<Missile> missiles;
        public Ship(Game game)
            : base(game)
        {
            modelName = "Ship";
            Position = new Vector3(0f, 0f, 0f);
            
            Quat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)Math.PI / 2);
            
            GameplayScreen.soundbank.PlayCue("Ship_Spawn");
            
            fMass = 10;
            fTerminalVelocity = 10;
            bPhysicsDriven = true;
            missiles = new List<Missile>();

        }
        public override void Initialize()
        {
            base.Initialize();
       
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
           // GameplayScreen.CameraMatrix = Matrix.CreateLookAt(Position+new Vector3(0.0f, 10.0f, 50.0f), Position, Vector3.UnitY);
           
            Vector3 campos = new Vector3(0f, 50.0f, 60.0f);
            campos = (Vector3.Transform(campos, Matrix.CreateFromQuaternion(Quat)));
            campos += Position;
            Vector3 camup = new Vector3(0, 1, 0);
            camup = Vector3.Transform(camup, Matrix.CreateFromQuaternion(Quat));
            
            GameplayScreen.CameraMatrix = Matrix.CreateLookAt(campos, Position, camup);
           

     

        }

        public void turnLeft(GameTime gameTime)
        {
            Vector3 rotationAxis = new Vector3(0f,1f,0f);
            Quat *= Quaternion.CreateFromAxisAngle(rotationAxis, (float)(Math.PI / 512) * ((float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond));
            
        }

        public void turnRight(GameTime gameTime)
        {
            Vector3 rotationAxis = new Vector3(0f, 1f, 0f);
            Quat *= Quaternion.CreateFromAxisAngle(rotationAxis, (float)(Math.PI / -512) * ((float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond));
           
        }
        public void moveForward(GameTime gameTime)
        {
            vForce += Vector3.Normalize(GetWorldFacing()) * ((float)gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond);
          
        }


        public void stop()
        {
            vForce = new Vector3(0, 0, 0);
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

        public void shootMissile()
        {
            if (canShoot)
            {
                Missile temp = new Missile(Game, missileFired, this);
                missileFired++;
                temp.Position = Position;
                temp.vForce = GetWorldFacing() * 20000f;
                temp.Quat = Quat;
                temp.recalculateWolrdTransform();
                Game.Components.Add(temp);
                missiles.Add(temp);
                canShoot = false;
                timer.AddTimer("Shoot Timer", 1, allowShooting, false);
            }
        }

        public void allowShooting()
        {
            canShoot = true;
        }

    }
}
