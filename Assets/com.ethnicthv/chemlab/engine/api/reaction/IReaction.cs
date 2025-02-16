using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.molecule;
using com.ethnicthv.chemlab.engine.api.molecule.group;
using com.ethnicthv.chemlab.engine.molecule;

namespace com.ethnicthv.chemlab.engine.api.reaction
{
    public interface IReaction
    {
        public List<MoleculeGroup> GetReactantGroups();

        public void ForwardReaction(ReactionContext context, out IReactionResult result);
    }
}