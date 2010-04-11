using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SuperDragonBall
{
    /// <summary>
    /// A class to extract triangle and vertex data from a mesh
    /// This code was found online at: http://www.enchantedage.com/node/30
    /// </summary>
    class MeshDataExtractor
    {

        public struct TriangleVertexIndices
        {
            public int I0;
            public int I1;
            public int I2;
        }

        public static void ExtractData(Model mdl, List<Vector3> vtcs, List<TriangleVertexIndices> idcs, bool includeNoncoll)
        {
            Matrix m = Matrix.Identity;
            foreach (ModelMesh mm in mdl.Meshes)
            {
                m = GetAbsoluteTransform(mm.ParentBone);
                ExtractModelMeshData(mm, ref m, vtcs, idcs, "Collision Model", includeNoncoll);
            }
        }

        public static Matrix GetAbsoluteTransform(ModelBone bone)
        {
            if (bone == null)
                return Matrix.Identity;
            return bone.Transform * GetAbsoluteTransform(bone.Parent);
        }

        public static void ExtractModelMeshData(ModelMesh mm, ref Matrix xform,
            List<Vector3> vertices, List<TriangleVertexIndices> indices, string name, bool includeNoncoll)
        {
            foreach (ModelMeshPart mmp in mm.MeshParts)
            {

                if (!includeNoncoll)
                {
                    EffectAnnotation annot = mmp.Effect.CurrentTechnique.Annotations["collide"];
                    if (annot != null && annot.GetValueBoolean() == false)
                    {
                        Console.WriteLine("Ignoring model mesh part {0}:{1} because it's set to not collide.",
                            name, mm.Name);
                        continue;
                    }
                }
                ExtractModelMeshPartData(mm, mmp, ref xform, vertices, indices, name);
            }
        }

        public static void ExtractModelMeshPartData(ModelMesh mm, ModelMeshPart mmp, ref Matrix xform,
            List<Vector3> vertices, List<TriangleVertexIndices> indices, string name)
        {
            int offset = vertices.Count;
            Vector3[] a = new Vector3[mmp.NumVertices];
            mm.VertexBuffer.GetData<Vector3>(mmp.StreamOffset + mmp.BaseVertex * mmp.VertexStride,
                a, 0, mmp.NumVertices, mmp.VertexStride);
            for (int i = 0; i != a.Length; ++i)
                Vector3.Transform(ref a[i], ref xform, out a[i]);
            vertices.AddRange(a);

            if (mm.IndexBuffer.IndexElementSize != IndexElementSize.SixteenBits)
                throw new Exception(
                    String.Format("Model {0} uses 32-bit indices, which are not supported.",
                                  name));
            short[] s = new short[mmp.PrimitiveCount * 3];
            mm.IndexBuffer.GetData<short>(mmp.StartIndex * 2, s, 0, mmp.PrimitiveCount * 3);
            TriangleVertexIndices[] tvi = new TriangleVertexIndices[mmp.PrimitiveCount];
            for (int i = 0; i != tvi.Length; ++i)
            {
                tvi[i].I0 = s[i * 3 + 0] + offset;
                tvi[i].I1 = s[i * 3 + 1] + offset;
                tvi[i].I2 = s[i * 3 + 2] + offset;
            }
            indices.AddRange(tvi);
        }

    }
}
