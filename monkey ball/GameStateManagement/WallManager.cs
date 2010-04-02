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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    /// 
    public class WallManager : Microsoft.Xna.Framework.GameComponent
    {
      private Game game;
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
            for (int i = (int)-Actor.Bounds / 20; i < (int)Actor.Bounds / 20; i++)
            {
                Wall topWall = new Wall(game);
                topWall.Position = new Vector3(i * 20.0f, -Actor.Bounds, 0f);
                game.Components.Add(topWall);
            }
            for (int i = (int)-Actor.Bounds / 20; i < (int)Actor.Bounds / 20; i++)
            {
                Wall topWall = new Wall(game);
                topWall.Position = new Vector3(i * 20.0f, Actor.Bounds, 0f);
                game.Components.Add(topWall);
            }
            for (int i = (int)-Actor.Bounds / 20; i < (int)Actor.Bounds / 20; i++)
            {
                Wall topWall = new Wall(game);
                topWall.Position = new Vector3(-Actor.Bounds, i * 20.0f, 0f);
                game.Components.Add(topWall);
            }
            for (int i = (int)-Actor.Bounds / 20; i < (int)Actor.Bounds / 20; i++)
            {
                Wall topWall = new Wall(game);
                topWall.Position = new Vector3(Actor.Bounds, i * 20.0f, 0f);
                game.Components.Add(topWall);
            }
        }

       
    }
}