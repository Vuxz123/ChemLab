using com.ethnicthv.chemlab.client.api.model;
using com.ethnicthv.chemlab.client.model.util;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.model
{
    public class SingleBondModel : IModel
    {
        private readonly Mesh _mesh;
        public Vector3 Position;
        public Quaternion Rotation;
        private readonly float _length;
        
        public SingleBondModel(Mesh mesh, Vector3 position, Quaternion rotation, float length)
        {
            _mesh = mesh;
            Position = position;
            Rotation = rotation;
            _length = length;
        }
        
        public SingleBondModel(float length)
        {
            _mesh = GenerateMesh(length);
            Position = Vector3.zero;
            Rotation = Quaternion.identity;
        }
        
        public Mesh GetMesh()
        {
            return _mesh;
        }

        public Vector3 GetPosition()
        {
            return Position;
        }

        public Quaternion GetRotation()
        {
            return Rotation;
        }

        public Matrix4x4 GetModelMatrix()
        {
            return Matrix4x4.TRS(Position, Rotation, Vector3.one);
        }
        
        private static Mesh GenerateMesh(float length)
        {
            return BondModelUtil.GenerateSingleBond(0.05f, length);
        }
    }
}