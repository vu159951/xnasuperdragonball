using System;
using System.Collections.Generic;
using System.Collections;
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
      public static float WallBoundsZ = 600f;
      private GameplayScreen hostScreen;
      private ArrayList wallCubeList;
          

      public WallManager(Game game, GameplayScreen host)
            : base(game)
        {
            hostScreen = host;   
            this.game = game;
            wallCubeList = new ArrayList();
         
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
        /// Removes the wall cube components
        /// </summary>
        public void removeWallComponents() {
            foreach (Wall w in wallCubeList)
            {
                game.Components.Remove(w);
            }
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
            for (int i = (int)-WallBoundsZ / 20; i < (int)WallBoundsZ / 20; i++)
            {
                Wall topWall = new Wall(game, hostScreen);
                topWall.position = new Vector3(i * 20.0f, 0f, -WallBoundsZ);
                game.Components.Add(topWall);
                wallCubeList.Add(topWall);
            }
            for (int i = (int)-WallBoundsZ / 20; i < (int)WallBoundsZ / 20; i++)
            {
                Wall bottomWall = new Wall(game, hostScreen);
                bottomWall.position = new Vector3(i * 20.0f, 0f, WallBoundsZ);
                game.Components.Add(bottomWall);
                wallCubeList.Add(bottomWall);
            }
            for (int i = (int)-WallBoundsX / 20; i < (int)WallBoundsX / 20; i++)
            {
                Wall leftWall = new Wall(game, hostScreen);
                leftWall.position = new Vector3(-WallBoundsX, 0f, i * 20.0f);
                game.Components.Add(leftWall);
                wallCubeList.Add(leftWall);
            }
            for (int i = (int)-WallBoundsX / 20; i < (int)WallBoundsX / 20; i++)
            {
                Wall rightWall = new Wall(game, hostScreen);
                rightWall.position = new Vector3(WallBoundsX, 0f, i * 20.0f);
                game.Components.Add(rightWall);
                wallCubeList.Add(rightWall);
            }
        }

       
    }
}