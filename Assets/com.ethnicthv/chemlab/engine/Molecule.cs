using Unity.VisualScripting;

namespace com.ethnicthv.chemlab.engine
{
    public class Molecule
    {
        public readonly Formula Formula;
        
        public Molecule(Formula formula)
        {
            Formula = formula;
        }
    }
}