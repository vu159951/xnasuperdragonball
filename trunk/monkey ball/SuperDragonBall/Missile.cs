using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class Missile : Actor
    {

        private const float MISSILE_DELETE_TIME = 5.0f;
        private Utils.Timer missileTimer;

        public Missile(Game game)
            : base(game)
        {
            modelName = "Missile";

            missileTimer = new Utils.Timer();
            quat = Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationX((float)Math.PI / 2));

            bPhysicsDriven = true;
            fTerminalVelocity *= 1.5f;
        }

        public override void Initialize()
        {
            missileTimer.AddTimer("DeleteMissile", MISSILE_DELETE_TIME, deleteSelf, false);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            missileTimer.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void UnloadContent()
        {

            base.UnloadContent();
        }

        //called by the timer after MISSILE_DELETE_TIME seconds
        private void deleteSelf()
        {
            Game.Components.Remove(this);
            this.UnloadContent();
        }

    }
}
