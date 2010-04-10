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
    public class SpawnManager : Microsoft.Xna.Framework.GameComponent
    {
        public List<Asteroid> asteroids;
        private Game game;
        private Utils.Timer timer;
        private Random random; 
        public SpawnManager(Game game)
            : base(game)
        {
            
            asteroids = new List<Asteroid>();
            timer = new Utils.Timer();

            timer.AddTimer("creationTimer", .1f, createAsteroids, true);
            this.game = game;
            random = new Random();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
            timer.Update(gameTime);
        }

        public void createAsteroids()
        {
            /*
            Asteroid temp;
            if (asteroids.Count < 0)
            {
                 temp= new Asteroid(game);
                temp.position = new Vector3((float)(random.NextDouble() * 1024)-512, (float)(random.NextDouble() * 768)-384, 0.0f);
                temp.netForce = new Vector3(((float)random.NextDouble()*2-1), ((float)random.NextDouble()*2-1), 0)*300f;
                game.Components.Add(temp);
                //if(temp
                asteroids.Add(temp);
                
                //GameplayScreen.asteroidLocations.Add(temp, temp.Position);

            }
             */
        }
    }
}