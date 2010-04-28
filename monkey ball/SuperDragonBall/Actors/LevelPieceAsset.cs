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

using SuperDragonBall;
using SuperDragonBall.Utils;

namespace SuperDragonBall
{



    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class LevelPieceAsset : LevelPiece
    {

        public LevelPieceAsset(Game game, GameplayScreen host, String assetName)
            : base(game, host, assetName)
        {
          
        }

        /// <summary>
        /// Match the local rotation of the host
        /// </summary>
        public void matchLocalRotation(Quaternion hostLocalRotation) {
            m_localRotation = hostLocalRotation;
        }

        /// <summary>
        /// Match the offfset position of the host
        /// </summary>
        public void matchRotationOffset(Vector3 hostOffset)
        {
            m_rotationOffset = hostOffset;
        }



    }
}