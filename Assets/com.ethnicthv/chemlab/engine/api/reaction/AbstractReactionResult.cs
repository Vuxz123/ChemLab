using System;
using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.molecule;

namespace com.ethnicthv.chemlab.engine.api.reaction
{
    public abstract class AbstractReactionResult : IReactionResult
    {
        public static readonly int PriorityDefault = 0;
        
        public int CompareTo(IReactionResult other)
        {
            return GetPriority().CompareTo(other.GetPriority());
        }

        public virtual int GetPriority()
        {
            return PriorityDefault;
        }
        public abstract Dictionary<Molecule, float> GetConsumedMolecules(ReactionTickContext context);
        public abstract Dictionary<Molecule, float> GetProducedMolecules(ReactionTickContext context);
    }
}