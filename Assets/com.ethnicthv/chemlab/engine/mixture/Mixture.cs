using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.mixture;
using com.ethnicthv.chemlab.engine.api.molecule.group;
using com.ethnicthv.chemlab.engine.api.reaction;
using com.ethnicthv.chemlab.engine.molecule;
using com.ethnicthv.chemlab.engine.molecule.group;
using com.ethnicthv.chemlab.engine.reaction;
using Util = com.ethnicthv.chemlab.engine.mixture.MixtureUtil;

namespace com.ethnicthv.chemlab.engine.mixture
{
    public class Mixture : IMixture
    {
        private readonly MixtureType _mixtureType;
        //Note: float is used to represent the number of moles of the element in mol in the mixture
        private readonly Dictionary<Molecule, float> _mixtureComposition = new();
        private readonly Dictionary<MoleculeGroup, List<Molecule>> _moleculeGroups = new();
        
        private readonly LinkedList<IReactionResult> _reactionResults = new();
        
        private bool _isMixtureChecked;

        public static Mixture CreateMixture(MixtureType mixtureType)
        {
            return new Mixture(mixtureType);
        }
        
        private Mixture(MixtureType mixtureType)
        {
            _mixtureType = mixtureType;
        }
        
        public MixtureType GetMixtureType()
        {
            return _mixtureType;
        }

        public void Tick()
        {
            if (!_isMixtureChecked)
            {
                CheckMixture();
                _isMixtureChecked = true;
            }
            
            RunReactions();
        }

        public void AddMolecule(Molecule molecule, float moles)
        {
            Util.AddMolecule(molecule, moles, in _mixtureComposition, out _isMixtureChecked);
        }

        public void RemoveMolecule(Molecule molecule)
        {
            Util.RemoveMolecule(molecule, in _mixtureComposition, out _isMixtureChecked);
        }

        public void SetMoles(Molecule molecule, float moles)
        {
            if (!_mixtureComposition.ContainsKey(molecule))
            {
                Util.AddMolecule(molecule, moles, in _mixtureComposition, out _isMixtureChecked);
                return;
            }
            _mixtureComposition[molecule] = moles;
        }
        
        public float GetMoles(Molecule molecule)
        {
            return _mixtureComposition[molecule];
        }

        public float AddMoles(Molecule molecule, float moles, out bool isMutating)
        {
            var t = Util.AddMoles(molecule, moles, in _mixtureComposition, ref _isMixtureChecked);
            isMutating = _isMixtureChecked;
            return t;
        }

        public float SubtractMoles(Molecule molecule, float moles, out bool isMutating)
        {
            var t = Util.SubtractMoles(molecule, moles, in _mixtureComposition, ref _isMixtureChecked);
            isMutating = _isMixtureChecked;
            return t;
        }

        public Dictionary<Molecule, float> GetMixtureComposition()
        {
            return _mixtureComposition;
        }
        
        public void ClearMixture()
        {
            _mixtureComposition.Clear();
            _isMixtureChecked = false;
        }
        
        private void CheckMixture()
        {
            foreach (var molecule in _mixtureComposition.Keys)
            {
                GroupDetectingProgram.Instance.CheckMolecule(molecule, this);
            }
            
            ReactionProgram.Instance.CheckForReaction(new ReactionContext(_moleculeGroups, _mixtureComposition), in _reactionResults);
        }

        private void RunReactions()
        {
            foreach (var reactionResult in _reactionResults)
            {
                var reactants = reactionResult.GetConsumedMolecules();
                var products = reactionResult.GetProducedMolecules();
                
                foreach (var reactant in reactants.Keys)
                {
                    Util.SubtractMoles(reactant, reactants[reactant], in _mixtureComposition, ref _isMixtureChecked);
                }
                
                foreach (var product in products.Keys)
                {
                    Util.AddMoles(product, products[product], in _mixtureComposition, ref _isMixtureChecked);
                }
            }
        }

        public void AddToGroup(MoleculeGroup getGroup, Molecule molecule)
        {
            if (!_moleculeGroups.ContainsKey(getGroup)) _moleculeGroups.Add(getGroup, new List<Molecule>());
            _moleculeGroups[getGroup].Add(molecule);
        }
    }
}