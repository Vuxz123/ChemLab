using com.ethnicthv.chemlab.client.api.render;
using com.ethnicthv.chemlab.client.model;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.render
{
    public abstract class GroupRenderer<GM> : IRenderer<GM> where GM : GroupModel
    {
        public abstract void Render(GM renderable, Camera camera);
        public abstract void RenderGizmos(GM renderable);
    }
}