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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    /// 
    public class WallManager : Microsoft.Xna.Framework.GameComponent
    {
      private Game game;

      public static float WallBoundsX = 600f;
      public static float WallBoundsY = 600f;

        public WallManager(Game game)
            : base(game)
        {
            
          this.game = game;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
            buildWall();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
           
        }

        private void buildWall()
        {
            for (int i = (int)-WallBoundsY / 20; i < (int)WallBoundsY / 20; i++)
            {
                Wall topWall = new Wall(game);
                topWall.position = new Vector3(i * 20.0f, -WallBoundsY, 0f);
                game.Components.Add(topWall);
            }
            for (int i = (int)-WallBoundsY / 20; i < (int)WallBoundsY / 20; i++)
            {
                Wall bottomWall = new Wall(game);
                bottomWall.position = new Vector3(i * 20.0f, WallBoundsY, 0f);
                game.Components.Add(bottomWall);
            }
            for (int i = (int)-WallBoundsX / 20; i < (int)WallBoundsX / 20; i++)
            {
                Wall leftWall = new Wall(game);
                leftWall.position = new Vector3(-WallBoundsX, i * 20.0f, 0f);
                game.Components.Add(leftWall);
            }
            for (int i = (int)-WallBoundsX / 20; i < (int)WallBoundsX / 20; i++)
            {
                Wall rightWall = new Wall(game);
                rightWall.position = new Vector3(WallBoundsX, i * 20.0f, 0f);
                game.Components.Add(rightWall);
            }
        }

       
    }
}