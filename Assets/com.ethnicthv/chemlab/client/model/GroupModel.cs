using com.ethnicthv.chemlab.client.api.model;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.model
{
    public abstract class GroupModel : IModel
    {
        public abstract Mesh GetMesh();

        public abstract Vector3 GetPosition();

        public abstract Quaternion GetRotation();
    }
}