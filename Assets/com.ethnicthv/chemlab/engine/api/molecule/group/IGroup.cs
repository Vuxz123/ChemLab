using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;

namespace com.ethnicthv.chemlab.engine.api.molecule.group
{
    public interface IGroup<M> where M : IGroupModel
    {
        public M Model { get; }
        public Atom GetAtom();
        public bool IsContainsAtom(Element element);
    }
}