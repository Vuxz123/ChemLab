using System;
using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.molecule;

namespace com.ethnicthv.chemlab.engine.api.reaction
{
    public abstract class AbstractReactionResult : IReactionResult
    {
        public int CompareTo(IReactionResult other)
        {
            return GetPriority().CompareTo(other.GetPriority());
        }

        public abstract int GetPriority();
        public abstract Dictionary<Molecule, float> GetConsumedMolecules();
        public abstract Dictionary<Molecule, float> GetProducedMolecules();
    }
}