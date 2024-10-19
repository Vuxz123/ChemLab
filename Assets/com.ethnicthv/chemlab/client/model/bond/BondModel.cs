using com.ethnicthv.chemlab.client.api.model;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.model.bond
{
    public abstract class BondModel : IModel
    {
        public Vector3 Position;
        public Quaternion Rotation;
        protected readonly float Length;
        
        public BondModel(Vector3 position, Quaternion rotation, float length)
        {
            Position = position;
            Rotation = rotation;
            Length = length;
        }
        
        public abstract Mesh GetMesh();

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
    }
}