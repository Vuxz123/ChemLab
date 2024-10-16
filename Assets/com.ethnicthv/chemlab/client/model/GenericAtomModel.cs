using com.ethnicthv.chemlab.client.api.model;
using com.ethnicthv.chemlab.client.model.util;
using com.ethnicthv.chemlab.engine.api.atom;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.model
{
    public class GenericAtomModel : IAtomModel
    {
        private const float AtomRadius = 0.5f;
        
        private readonly Mesh _mesh;
        public Vector3 Position;
        public Quaternion Rotation;
        private readonly Atom _atom;
        private readonly float _radius;
        
        public GenericAtomModel(Mesh mesh, Vector3 position, Quaternion rotation, Atom atom, float radius)
        {
            _mesh = mesh;
            Position = position;
            Rotation = rotation;
            _atom = atom;
            _radius = radius;
        }

        public GenericAtomModel(Atom atom)
        {
            // calculate radius based on element
            _atom = atom;
            _radius = ElementAtomRadius.Radius.TryGetValue(atom.GetElement(), out var radius) ? radius : AtomRadius;
            _mesh = GenerateMesh(atom, _radius);
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

        public Atom GetAtom()
        {
            return _atom;
        }

        public float GetRadius()
        {
            return _radius;
        }

        private static Mesh GenerateMesh(Atom atom, float radius)
        {
            return SphereModelUtil.GenerateIcoSphereMesh(radius);
        }
    }
}