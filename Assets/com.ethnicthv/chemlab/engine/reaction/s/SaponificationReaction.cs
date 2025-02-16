using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.molecule.group;
using com.ethnicthv.chemlab.engine.api.reaction;
using com.ethnicthv.chemlab.engine.molecule;

namespace com.ethnicthv.chemlab.engine.reaction.s
{
    public class SaponificationReaction : IReaction
    {
        public List<MoleculeGroup> GetReactantGroups()
        {
            throw new System.NotImplementedException();
        }

        public void ForwardReaction(ReactionContext context, out IReactionResult result)
        {
            throw new System.NotImplementedException();
        }
    }
}