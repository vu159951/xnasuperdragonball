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


namespace SuperDragonBall.Actors
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GoalObject : LevelPiece
    {
        public GoalObject(Game game, GameplayScreen host)
            : base(game, host, "goalShen")
        {
            //modelName = "cube";
            // TODO: Construct any child components here
            m_localRotation = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), -(float)Math.PI/2);
            textureName = "1305";
            mixCelWithTexture = true;
            celLightColor = Color.Green.ToVector4();
            enableCelShading = true;
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
        }

        protected override void modifyWorldTransform()
        {
            base.modifyWorldTransform();
            Vector3 Pos = new Vector3(scale*1f, scale*1.0f, scale*13.0f);
            Pos = (Vector3.Transform(Pos, Matrix.CreateFromQuaternion(quat)));
            Pos+=position;
            WorldBounds.Center = Pos;
            WorldBounds.Radius = 2*scale;

        }
    }
}