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

    public class SkySphere : Actor
    {

        public SkySphere(Game game, GameplayScreen host)
            : base(game, host)
        {
            position = new Vector3(0f, 0f, 0f);
            modelName = "sky_sphere_y";
            quat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)Math.PI / 2);

            scale = 500;
            textureName = "ysky";
            mixCelWithTexture = true;
            skyCulling = true;
            //enableCelShading = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


    }
}