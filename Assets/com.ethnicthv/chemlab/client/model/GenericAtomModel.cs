using com.ethnicthv.chemlab.client.api.model;
using com.ethnicthv.chemlab.engine;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.model
{
    public class GenericAtomModel : IAtomModel
    {
        public const float ATOM_RADIUS = 0.5f;
        
        private Mesh _mesh;
        private Vector3 _position;
        private Quaternion _rotation;
        private Atom _atom;
        private float _radius;
        
        public GenericAtomModel(Mesh mesh, Vector3 position, Quaternion rotation, Atom atom, float radius)
        {
            _mesh = mesh;
            _position = position;
            _rotation = rotation;
            _atom = atom;
            _radius = radius;
        }

        public GenericAtomModel(Atom atom)
        {
            // calculate radius based on element
            _atom = atom;
            _radius = ElementAtomRadius.Radius.TryGetValue(atom.Element, out var radius) ? radius : ATOM_RADIUS;
            _mesh = GenerateMesh(atom, _radius);
            _position = Vector3.zero;
            _rotation = Quaternion.identity;
        }

        public Mesh GetMesh()
        {
            return _mesh;
        }

        public Vector3 GetPosition()
        {
            return _position;
        }

        public Quaternion GetRotation()
        {
            return _rotation;
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