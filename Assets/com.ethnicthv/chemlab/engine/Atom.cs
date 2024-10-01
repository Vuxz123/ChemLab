using System.Linq;

namespace com.ethnicthv.chemlab.engine
{
    public class Atom
    {
        public readonly Element Element;
        public readonly int Charge;
        
        public Atom(Element element, int charge = 0)
        {
            Element = element;
            Charge = charge;
        }
        
        public ElementProperty GetProperty()
        {
            return ElementProperty.GetElementProperty(Element);
        }
        
        public int GetMaxConnectivity()
        {
            // return max Valence value
            return GetProperty().Valences.Max() ;
        }
        
        public override string ToString()
        {
            return Element.ToString();
        }
    }
}