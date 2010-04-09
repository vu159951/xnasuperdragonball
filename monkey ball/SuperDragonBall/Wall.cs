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

    public class Wall : Actor
    {
        
        public Wall(Game game)
            : base(game)
        {
            position = new Vector3(0f, 0f, 0f);
            modelName = "cube";
           
            scale = 5;    
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
        }

     
    }
}