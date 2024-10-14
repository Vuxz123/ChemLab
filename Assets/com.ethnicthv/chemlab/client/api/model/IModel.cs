﻿using com.ethnicthv.chemlab.client.api.render;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.api.model
{
    public interface IModel : IRenderable
    {
        public Mesh GetMesh();
        public Vector3 GetPosition();
        public Quaternion GetRotation();
    }
}