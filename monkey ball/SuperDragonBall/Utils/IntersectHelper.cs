using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperDragonBall.Utils
{
    /// <summary>
    /// Contains static functions to help with Complex intersections
    /// Written by Greg Lieberman
    /// 
    /// Sphere-Triangle Intersection code adapted from 
    /// http://www.geometrictools.com/LibFoundation/Intersection/Intersection.html
    /// http://www.geometrictools.com/LibFoundation/Intersection/Wm4IntrTriangle3Sphere3.cpp
    /// </summary>
    public class IntersectHelper
    {
       


        /// <summary>
        /// (C-P)*n < r 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool spherePlaneIntersect(BoundingSphere b, Plane p)
        {
            return (Vector3.Dot(b.Center - p.getPlanePoint(), p.getPlaneNormal())
                < b.Radius);
        }

        /// <summary>
        /// Sphere-Triangle Intersection
        /// Instantaneous check, 
        /// simplified and less featured versus original source code
        /// </summary>
        /// <param name="ball"></param>
        /// <param name="t">Array of Vector3, size 3</param>
        /// <returns>Returns true if the sphere is inside of the triangle</returns>
        public static bool sphereTriangleIntersect(BoundingSphere s, Vector3[] t)
        {
            //t contains triangle verticies
            Vector3[] verticies = { t[0], t[1], t[2] };

            Vector3[] edges = {   verticies[1] - verticies[0], 
                                  verticies[2] - verticies[1],
                                  verticies[0] - verticies[2] };
            
            // get the triangle normal
            Vector3 tNormal = Vector3.Cross(edges[1], edges[0]);
 
            //sphere center direction on triangle normal
            float scProj = Vector3.Dot(tNormal, s.Center);

            //Radius projected length in normal direction
            float fRSqr = s.Radius * s.Radius;
            float fNormRadSqr = tNormal.LengthSquared() * fRSqr;

            //triangle projection on triangle normal
            float tProj = Vector3.Dot(tNormal, verticies[0]);

            //distance from sphere to triangle along the normal
            float fDist = scProj - tProj;

            //normals for the plane formed by the edge i and the triangle normal
            Vector3[] edgeCrossNormals = {  Vector3.Cross(edges[0], tNormal),
                                            Vector3.Cross(edges[1], tNormal),
                                            Vector3.Cross(edges[2], tNormal) };

            if (fDist * fDist <= fNormRadSqr) { 
                //sphere intersects the plane of the triangle
                bool[] bInside = new bool[3];

                // See which edges the sphere center is inside/outside.
                for (int i = 0; i < 3; i++)
                {
                    bInside[i] = (Vector3.Dot(edgeCrossNormals[i], s.Center) >= Vector3.Dot(edgeCrossNormals[i], verticies[i]));
                }

                if (bInside[0])
                {
                    if (bInside[1])
                    {
                        if (bInside[2])
                        {
                            // Triangle inside sphere.
                            return true; // for my purposes
                        }
                        else // !bInside[2]
                        {
                            // Potential intersection with edge <V2,V0>.
                            //?????????
                            return false;
                        }
                    }
                    else // !bInside[1]
                    {
                        if (bInside[2])
                        {
                            // Potential intersection with edge <V1,V2>.
                            //?????????
                            return false;
                        }
                        else // !bInside[2]
                        {
                            // Potential intersection with edges <V1,V2>, <V2,V0>.
                            //return FindTriangleSphereCoplanarIntersection(2,akV,
                            //  akExN[2],akE[2],fTMax,rkVelocity0,rkVelocity1);

                            //hmmm...
                            return true;
                        }
                    }
                }
                else // !bInside[0]
                {
                    if (bInside[1])
                    {
                        if (bInside[2])
                        {
                            // Potential intersection with edge <V0,V1>.
                           //?????????????
                            return false;
                        }
                        else // !bInside[2]
                        {
                            // Potential intersection with edges <V2,V0>, <V0,V1>.
                            //return FindTriangleSphereCoplanarIntersection(0,akV,
                             //   akExN[0],akE[0],fTMax,rkVelocity0,rkVelocity1);
                            return true;
                        }
                    }
                    else // !bInside[1]
                    {
                        if (bInside[2])
                        {
                            // Potential intersection with edges <V0,V1>, <V1,V2>.
                            //return FindTriangleSphereCoplanarIntersection(1,akV,
                              //  akExN[1],akE[1],fTMax,rkVelocity0,rkVelocity1);
                            return true;
                        }
                        else // !bInside[2]
                        {
                            // We should not get here.
                            //assert(false);
                            return false;
                        }            
                    }
                }
            }
            else
            {
                // Sphere does not currently intersect the plane of the triangle.
                return false;
            }
            //return false;           
        }



    //end class
    }
}
