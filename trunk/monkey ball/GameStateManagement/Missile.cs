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
     
    public class Missile : Actor
    {
        private Ship ship;
        private Utils.Timer missileTimer;
        public Missile(Game game, int idNum, Ship ship1)
            : base(game)
        {
            modelName = "Missile";
            GameplayScreen.soundbank.PlayCue("Ship_Missile");
            bPhysicsDriven = true;
             missileTimer = new Utils.Timer();
           // Quat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)Math.PI / 2);
            Scale = -2;
            missileTimer.AddTimer("Removal Timer " + idNum, 5, removeMissile, false);
            fTerminalVelocity = 8;
            fMass = 1f;
            ship = ship1;

            //I took out the missile removal do a bug, where all missiles are delted once the first timers go off and it deletes any subsequent missile immediatly

            //Console.WriteLine("Removal Timer " + idNum);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            missileTimer.Update(gameTime);
            bPhysicsDriven = false;
            
        }

        public void removeMissile()
        {
            Game.Components.Remove(this);
            ship.missiles.Remove(this);
            //UnloadContent();
        }
    }
}