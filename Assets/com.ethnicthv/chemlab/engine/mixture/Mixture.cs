using System;
using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.mixture;
using com.ethnicthv.chemlab.engine.api.reaction;
using com.ethnicthv.chemlab.engine.molecule;
using com.ethnicthv.chemlab.engine.molecule.group;
using com.ethnicthv.chemlab.engine.reaction;
using NUnit.Framework;

namespace com.ethnicthv.chemlab.engine.mixture
{
    public class Mixture : IMixture
    {
        private readonly MixtureType _mixtureType;
        //Note: double is used to represent the number of moles of the element in mol in the mixture
        private readonly Dictionary<Molecule, double> _mixtureComposition = new();
        
        private LinkedList<IReactionResult> _reactionResults = new();
        
        private bool _isMixtureChecked = false;

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
        }

        public void AddMolecule(Molecule molecule, double moles)
        {
            _mixtureComposition[molecule] = moles;
            _isMixtureChecked = false;
        }
        
        public void RemoveMolecule(Molecule molecule)
        {
            _mixtureComposition.Remove(molecule);
            _isMixtureChecked = false;
        }
        
        public void SetMoles(Molecule molecule, double moles)
        {
            if (!_mixtureComposition.ContainsKey(molecule))
            {
                AddMolecule(molecule, moles);
                return;
            }
            _mixtureComposition[molecule] = moles;
        }
        
        public double GetMoles(Molecule molecule)
        {
            return _mixtureComposition[molecule];
        }

        public double AddMoles(Molecule molecule, double moles, out bool isMutating)
        {
            if (!_mixtureComposition.ContainsKey(molecule))
            {
                AddMolecule(molecule, moles);
                isMutating = true;
                return _mixtureComposition[molecule];
            }
            _mixtureComposition[molecule] += moles;
            isMutating = false;
            return _mixtureComposition[molecule];
        }

        public double SubtractMoles(Molecule molecule, double moles, out bool isMutating)
        {
            if (!_mixtureComposition.ContainsKey(molecule))
            {
                throw new Exception("Molecule not found in mixture composition.");
            }
            _mixtureComposition[molecule] -= moles;
            if (_mixtureComposition[molecule] <= 0)
            {
                RemoveMolecule(molecule);
                isMutating = true;
                return 0;
            }
            isMutating = false;
            return _mixtureComposition[molecule];
        }

        public Dictionary<Molecule, double> GetMixtureComposition()
        {
            return _mixtureComposition;
        }
        
        public void ClearMixture()
        {
            _mixtureComposition.Clear();
        }
        
        private void CheckMixture()
        {
            foreach (var molecule in _mixtureComposition.Keys)
            {
                GroupDetectingProgram.Instance.CheckMolecule(molecule);
            }
            
            ReactionProgram.Instance.CheckForReaction(_mixtureComposition, in _reactionResults);
        }

        private void RunReactions()
        {
            foreach (var reactionResult in _reactionResults)
            {
                var reactants = reactionResult.GetConsumedMolecules();
                var products = reactionResult.GetProducedMolecules();
                
                var needToCheck = false;
                
                foreach (var reactant in reactants.Keys)
                {
                    SubtractMoles(reactant, reactants[reactant], out var isMutating);
                    needToCheck = needToCheck || isMutating;
                }
                
                foreach (var product in products.Keys)
                {
                    AddMoles(product, products[product], out var isMutating);
                    needToCheck = needToCheck || isMutating;
                }
            }
        }
    }
}