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


namespace SuperDragonBall
{
    public class Asteroid : Actor
    {
        public bool collided=false;
        //public bool earlyCollide=false;

        public Asteroid(Game game, GameplayScreen host)
            : base(game, host)
        {
            position = new Vector3(-2000f, -2000f, 0f);
            //recalculateWolrdTransform();
            
            fTerminalVelocity = 5f;
            fMass = 3;
            modelName = "Asteroid";
            

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Vector3 rotationAxis = velocity;
            rotationAxis.Normalize();
           // bPhysicsDriven = false;

            quat *= Quaternion.CreateFromAxisAngle(rotationAxis, (float)Math.PI / 512 * Math.Max((gameTime.ElapsedGameTime.Ticks / System.TimeSpan.TicksPerMillisecond),0.5f));
           
        }

      



    }
}