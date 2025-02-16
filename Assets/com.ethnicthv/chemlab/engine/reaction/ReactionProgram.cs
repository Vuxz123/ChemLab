using System.Collections.Generic;
using System.Linq;
using com.ethnicthv.chemlab.engine.api.molecule;
using com.ethnicthv.chemlab.engine.api.molecule.group;
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
        
        public void CheckForReaction(ReactionContext context , in LinkedList<IReactionResult> results)
        {
            //Note: Clear the old results list
            results.Clear();
            
            foreach (var reaction in _reactions)
            {
                var reactantGroups = reaction.GetReactantGroups();
                var found = reactantGroups.All(context.ContainsGroup);

                if (!found)
                {
                    continue;
                }

                var temp = new Dictionary<MoleculeGroup, IMutableMolecule>();
                

                reaction.ForwardReaction(context, out var result);
                results.AddLast(result);
            }
        }
    }
}