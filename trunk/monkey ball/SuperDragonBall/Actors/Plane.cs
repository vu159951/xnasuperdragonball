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
    public class Plane : Actor

    {
        public Plane(Game game, GameplayScreen host)
            : base(game, host)
        {
            // TODO: Construct any child components here
            modelName = "checker_plane";
            position = new Vector3(0f, 0f, -10f);
            scale *= 50;
            
            quat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)Math.PI / 2);

            //effect.TextureEnabled = true;

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
        /*
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            base.Draw(gameTime);
            GraphicsDevice.RenderState.DepthBufferEnable = true;
            GraphicsDevice.RenderState.AlphaBlendEnable = true;
            GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            GraphicsDevice.RenderState.BlendFunction = BlendFunction.Add;
            //dev.RenderState.TextureFactor=Color.FromArgb(204,255,255,255).ToArgb()


            model.CopyAbsoluteBoneTransformsTo(boneTransforms);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransforms[mesh.ParentBone.Index] * worldTransform;
                    effect.View = GameplayScreen.CameraMatrix;
                    effect.Projection = GameplayScreen.ProjectionMatrix;
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.AmbientLightColor = GameplayScreen.AmbientLightColor;
                    effect.SpecularPower = GameplayScreen.SpecularPower;
                    effect.SpecularColor = GameplayScreen.SpecularColor;
                    effect.DirectionalLight0.Direction = GameplayScreen.DLightDirection;
                    effect.DirectionalLight0.DiffuseColor = GameplayScreen.DLightColor;

                    effect.TextureEnabled = true;
                    Texture2D tex = contentManager.Load<Texture2D>("2031");
                    effect.Texture = tex;

                }
                mesh.Draw();

            }
            GraphicsDevice.RenderState.AlphaBlendEnable = false;
        }*/
    }
}