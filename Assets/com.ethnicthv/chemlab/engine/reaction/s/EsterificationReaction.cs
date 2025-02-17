using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.molecule.group;
using com.ethnicthv.chemlab.engine.api.reaction;
using com.ethnicthv.chemlab.engine.formula;
using com.ethnicthv.chemlab.engine.molecule;
using com.ethnicthv.chemlab.engine.molecule.group.functional;
using com.ethnicthv.chemlab.engine.util;

namespace com.ethnicthv.chemlab.engine.reaction.s
{
    class EsterificationReactionResult : AbstractReactionResult
    {
        public override int GetPriority()
        {
            return PriorityDefault;
        }

        public override Dictionary<Molecule, float> GetConsumedMolecules(ReactionTickContext context)
        {
            throw new System.NotImplementedException();
        }

        public override Dictionary<Molecule, float> GetProducedMolecules(ReactionTickContext context)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class EsterificationReaction : IReaction, INeedReactantGroups
    {
        private static List<MoleculeGroup> _reactantGroups = new()
        {
            MoleculeGroup.Alcohol,
            MoleculeGroup.Carboxyl
        };
        
        public void ForwardReaction(ReactionContext context, in IOnlyPushList<IReactionResult> result)
        {
            var alcohol = context.GetGroupMembers(MoleculeGroup.Alcohol);
            var carboxyl = context.GetGroupMembers(MoleculeGroup.Carboxyl);
            
            if (alcohol == null || carboxyl == null)
            {
                return;
            }
            
            foreach (var alcoholMolecule in alcohol)
            {
                foreach (var carboxylMolecule in carboxyl)
                {
                    foreach (var alcoholPart in alcoholMolecule.GetAtomsInGroup(MoleculeGroup.Alcohol))
                    {
                        foreach (var carboxylPart in carboxylMolecule.GetAtomsInGroup(MoleculeGroup.Carboxyl))
                        {
                            var alcoholCopy = (Formula) alcoholMolecule.GetFormula().Clone();
                            var carboxylCopy = (Formula) carboxylMolecule.GetFormula().Clone();
                            
                            var alcoholFunctionalGroup = (AlcoholFunctionalGroup) alcoholPart;
                            var carboxylFunctionalGroup = (CarboxylFunctionGroup) carboxylPart;
                            
                            alcoholCopy.RemoveBond(alcoholFunctionalGroup.Oxygen, alcoholFunctionalGroup.Hydrogen);
                            carboxylCopy.RemoveBond(carboxylFunctionalGroup.Carbon, carboxylFunctionalGroup.Oxygen);

                            carboxylCopy.MoveToAtom(carboxylFunctionalGroup.Carbon).AddStructure(alcoholCopy);
                            
                            //TODO: Add the new molecule to the result
                        }
                    }
                }
            }
        }

        public List<MoleculeGroup> GetReactantGroups()
        {
            return _reactantGroups;
        }
    }
}