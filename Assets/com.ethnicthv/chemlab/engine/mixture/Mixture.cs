using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.mixture;
using com.ethnicthv.chemlab.engine.molecule;

namespace com.ethnicthv.chemlab.engine
{
    public class Mixture : IMixture
    {
        private readonly MixtureType _mixtureType;
        //Note: double is used to represent the number of moles of the element in mol in the mixture
        private readonly Dictionary<Molecule, double> _mixtureComposition = new();

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
        
        public void AddMolecule(Molecule molecule, double moles)
        {
            if (_mixtureComposition.ContainsKey(molecule))
            {
                _mixtureComposition[molecule] += moles;
            }
            else
            {
                _mixtureComposition.Add(molecule, moles);
            }
        }
        
        public void RemoveMolecule(Molecule molecule)
        {
            _mixtureComposition.Remove(molecule);
        }
        
        public void SetMoles(Molecule molecule, double moles)
        {
            _mixtureComposition[molecule] = moles;
        }
        
        public double GetMoles(Molecule molecule)
        {
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
    }
}