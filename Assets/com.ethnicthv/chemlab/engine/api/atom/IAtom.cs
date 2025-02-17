using System;
using com.ethnicthv.chemlab.engine.api.element;

namespace com.ethnicthv.chemlab.engine.api.atom
{
    public interface IAtom : ICloneable
    {
        public Element GetElement();
        public float GetMass();
        public int GetMaxConnectivity();
        public ElementProperty GetProperty();
    }
}