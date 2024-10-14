using com.ethnicthv.chemlab.engine;

namespace com.ethnicthv.chemlab.client.api.model
{
    public interface IAtomModel : IModel
    {
        public Atom GetAtom();
        public float GetRadius();
    }
}