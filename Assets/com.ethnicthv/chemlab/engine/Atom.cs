using System.Linq;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;

namespace com.ethnicthv.chemlab.engine
{
    public class Atom : IAtom
    {
        private readonly Element _element;
        private int _charge;
        
        public Atom(Element element, int charge = 0)
        {
            _element = element;
            _charge = charge;
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
            return new Atom(_element, _charge);
        }

        public Element GetElement()
        {
            return _element;
        }

        public float GetMass()
        {
            return GetProperty().AtomicMass;
        }

        public int GetCharge()
        {
            return _charge;
        }

        public void SetCharge(int charge)
        {
            _charge = charge;
        }
    }
}