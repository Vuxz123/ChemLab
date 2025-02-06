using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.molecule;

namespace com.ethnicthv.chemlab.engine.api.reaction
{
    public interface IReaction
    {
        public bool CheckForReaction(Dictionary<Molecule, double> molecules, out IReactionResult result);
    }
}