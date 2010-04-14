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
            //variables I'm not using
            //float fTMax = 0;
            
            Vector3 rkVelocity0 = Vector3.Zero;
            Vector3 rkVelocity1 = Vector3.Zero;

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

                //instantaneous check only
                if (bInside[0] && bInside[1] && bInside[2])
                {
                    return true;
                }
                else {
                    return false;
                }
                
                //this information is for time based collision subroutines and the like
                // ... it's incomplete and doesn't work
                /*
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
                            return FindTriangleSphereCoplanarIntersection(s, 2,verticies,
                              edgeCrossNormals[2],edges[2]);

                         
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
                            return FindTriangleSphereCoplanarIntersection(s, 0,verticies,
                                edgeCrossNormals[0],edges[0]); 
                        }
                    }
                    else // !bInside[1]
                    {
                        if (bInside[2])
                        {
                            // Potential intersection with edges <V0,V1>, <V1,V2>.
                            return FindTriangleSphereCoplanarIntersection(s, 1, verticies,
                                edgeCrossNormals[1],edges[1]);
                        }
                        else // !bInside[2]
                        {
                            // We should not get here.
                            //assert(false);
                            return false;
                        }            
                    }
                }
                 */
            }
            else
            {
                // Sphere does not currently intersect the plane of the triangle.
                //Console.WriteLine("Sphere not intersect plane");
                return false;
            }
                    
        }

        /// <summary>
        /// Helper function for 
        /// </summary>
        /// <param name="iVertex"></param>
        /// <param name="verticies"></param>
        /// <param name="rkSideNorm"></param>
        /// <param name="rkSide"></param>
        /// <returns></returns>
        private static bool FindTriangleSphereCoplanarIntersection(BoundingSphere s, int iVertex, Vector3[] verticies, 
            Vector3 rkSideNorm, Vector3 rkSide) {

            // iVertex is the "hinge" vertex that the two potential edges that can
            // be intersected by the sphere connect to, and it indexes into akV.
            //
            // rkSideNorm is the normal of the plane formed by (iVertex,iVertex+1)
            // and the tri norm, passed so as not to recalculate

            // Check for intersections at time 0.
               Vector3 kDist = verticies[iVertex] - s.Center;
            if (kDist.LengthSquared() < s.Radius * s.Radius)
            {
                // Already intersecting that vertex.
                //m_fContactTime = (Real)0;
                return false;
            }


            return false;
            //return true;
        }

    //end class
    }
}
