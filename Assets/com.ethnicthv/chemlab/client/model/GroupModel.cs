using com.ethnicthv.chemlab.client.api.model;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.model
{
    public abstract class GroupModel : IModel
    {
        public Mesh GetMesh()
        {
            throw new System.NotImplementedException("GroupModel does not have a specific mesh");
        }
        public abstract Vector3 GetPosition();
        public abstract Quaternion GetRotation();
        public abstract Matrix4x4 GetModelMatrix();
    }
}