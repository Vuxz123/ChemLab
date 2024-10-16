using System.Collections.Generic;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.model.util
{
    /// <summary>
    /// Model Generation Utility for Bonds <br/>
    ///     - All Model will have default rotation facing the positive y-axis <br/>
    ///     - All Model will have default position at the origin <br/>
    /// </summary>
    public static class BondModelUtil
    {
        public static Mesh GenerateSingleBond(float radius, float length)
        {
            var mesh = new Mesh();
            var (vertices, indices) = GenerateCylinder(radius, length, 4);
            mesh.vertices = vertices.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.RecalculateNormals();
            mesh.OptimizeReorderVertexBuffer();
            return mesh;
        }

        private static (List<Vector3>, List<int>) GenerateCylinder(float radius, float height, int nbSides)
        {
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            var step = 2 * Mathf.PI / nbSides;
            for (var i = 0; i < nbSides; i++)
            {
                var angle = i * step;
                var x = Mathf.Cos(angle) * radius;
                var z = Mathf.Sin(angle) * radius;
                vertices.Add(new Vector3(x, height, z));
                vertices.Add(new Vector3(x, 0, z));
            }

            for (var i = 0; i < nbSides; i++)
            {
                var i1 = i * 2;
                var i2 = (i * 2 + 1) % (nbSides * 2);
                var i3 = (i * 2 + 3) % (nbSides * 2);
                var i4 = (i * 2 + 2) % (nbSides * 2);
                indices.Add(i1);
                indices.Add(i2);
                indices.Add(i3);
                indices.Add(i1);
                indices.Add(i3);
                indices.Add(i4);
            }

            return (vertices, indices);
        }
    }
}