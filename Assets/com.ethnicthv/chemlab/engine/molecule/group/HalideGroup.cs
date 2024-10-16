using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;
using com.ethnicthv.chemlab.engine.api.molecule.group;
using com.ethnicthv.chemlab.engine.molecule.group.group;

namespace com.ethnicthv.chemlab.engine.molecule.group
{
    public class HalideGroup : IGroup<HalideGroupModel>
    {
        private static readonly HalideGroupModel Mmodel = new HalideGroupModel();
        
        public HalideGroupModel Model => Mmodel;
        
        private Atom HalogenAtom { get; }

        public HalideGroup(Atom halogenAtom)
        {
            HalogenAtom = halogenAtom;
        }

        public Atom GetAtom()
        {
            throw new System.NotImplementedException();
        }

        public bool IsContainsAtom(Element element)
        {
            return HalogenAtom.GetElement() == element;
        }
    }
}