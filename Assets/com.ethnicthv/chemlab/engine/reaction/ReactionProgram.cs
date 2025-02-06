using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.reaction;
using com.ethnicthv.chemlab.engine.molecule;

namespace com.ethnicthv.chemlab.engine.reaction
{
    public class ReactionProgram
    {
        public static ReactionProgram Instance { get; } = new();
        
        private readonly List<IReaction> _reactions = new(); 
        
        private ReactionProgram() { }
        
        public void RegisterReaction(IReaction reaction)
        {
            _reactions.Add(reaction);
        }
        
        public void CheckForReaction(Dictionary<Molecule, double> molecules, in LinkedList<IReactionResult> results)
        {
            //Note: Clear the old results list
            results.Clear();
            
            foreach (var reaction in _reactions)
            {
                if (reaction.CheckForReaction(molecules, out var result)) 
                    results.AddLast(result);
            }
        }
    }
}