using System.Linq;
using com.ethnicthv.chemlab.engine.api.element;

namespace com.ethnicthv.chemlab.engine.api.atom
{
    public class Atom : IAtom
    {
        private readonly Element _element;
        
        public Atom(Element element)
        {
            _element = element;
        }
        
        public ElementProperty GetProperty()
        {
            return ElementProperty.GetElementProperty(_element);
        }
        
        public int GetMaxConnectivity()
        {
            return GetProperty().Valences.Max() ;
        }
        
        public override string ToString()
        {
            return _element.ToString();
        }

        public object Clone()
        {
            return new Atom(_element);
        }

        public Element GetElement()
        {
            return _element;
        }

        public float GetMass()
        {
            return GetProperty().AtomicMass;
        }
    }
}