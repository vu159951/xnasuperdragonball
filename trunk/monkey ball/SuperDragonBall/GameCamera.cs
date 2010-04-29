using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperDragonBall
{
    /// <summary>
    /// Handles all Camera-related operations in the game
    /// </summary>
    public class GameCamera
    {
        private Matrix cameraMatrix;
        private Matrix projectionMatrix;
        private Vector3 facing;
        private Quaternion cameraQuat;
        private Vector3 m_kCameraPosition;


        public GameCamera() {
            //default camera orientation
            facing = new Vector3(0.0f, 10.0f, 100.0f);
            cameraMatrix = Matrix.CreateLookAt(facing, Vector3.Zero, Vector3.UnitY);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 2f, 4/3f, 2.0f, 10000f);
            cameraQuat = Quaternion.Identity;
           
        }

        /// <summary>
        /// Call this in a GameplayScreen Update function implementation
        /// Pass in the actor that the camera should follow behind
        /// </summary>
        /// <param name="actor"></param>
        public void followBehind(Actor actor) {
            
            Vector3 camPos = new Vector3(0f, 120.0f, 120.0f);
            camPos = (Vector3.Transform(camPos, Matrix.CreateFromQuaternion(actor.quat)));
            camPos += actor.position;
            m_kCameraPosition = camPos;
            Vector3 camUp = new Vector3(0, 1, 0);
            camUp = Vector3.Transform(camUp, Matrix.CreateFromQuaternion(actor.quat));

            cameraMatrix = Matrix.CreateLookAt(camPos, actor.position, camUp);

            facing = actor.position - camPos;
        }

        public void ManualCameraRotation(float rotation, Actor actor)
        {
            //Vector3 camPos = new Vector3(0f, 120.0f, 120.0f);
            //camPos = (Vector3.Transform(camPos, Matrix.CreateFromQuaternion(actor.quat)));
            
            //Vector3 camPos = new Vector3((float)Math.Cos(rotation) * 60f, 50.0f, (float)Math.Sin(rotation) * 60f);

            Vector3 camPos = new Vector3(0f, 120.0f, 120.0f);
            camPos = Vector3.Transform(camPos, Matrix.CreateRotationY(rotation));
            
            camPos += actor.position;
            m_kCameraPosition = camPos;
            Vector3 camUp = new Vector3(0, 1, 0);
            

            cameraMatrix = Matrix.CreateLookAt(camPos, actor.position, camUp);

            facing = actor.position - camPos;
        }

        


        /// <summary>
        /// Properties
        /// </summary>
        public Matrix CameraMatrix
        {
            get
            {
                return cameraMatrix;
            }         
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                return projectionMatrix;
            }
        }

        public Vector3 Facing {
            get {
                //MAY NOT BE CORRECT VALUE
                return facing;
            }
        }
        public Vector3 GetCameraPosition()
        {
            return m_kCameraPosition;
        }


    }
}
