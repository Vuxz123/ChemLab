using System;
using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.molecule;

namespace com.ethnicthv.chemlab.engine.mixture
{
    public static class MixtureUtil
    {
        public static float AddMoles(Molecule molecule, float moles, in Dictionary<Molecule, float> mixtureComposition, ref bool mutatingState)
        {
            if (!mixtureComposition.ContainsKey(molecule))
            {
                AddMolecule(molecule, moles, mixtureComposition, out mutatingState);
                mutatingState = true;
                return mixtureComposition[molecule];
            }
            mixtureComposition[molecule] += moles;
            return mixtureComposition[molecule];
        }

        public static float SubtractMoles(Molecule molecule, float moles, in Dictionary<Molecule, float> mixtureComposition, ref bool mutatingState)
        {
            if (!mixtureComposition.ContainsKey(molecule))
            {
                throw new Exception("Molecule not found in mixture composition.");
            }
            
            mixtureComposition[molecule] -= moles;
            
            if (mixtureComposition[molecule] > 0) return mixtureComposition[molecule];
            
            RemoveMolecule(molecule, mixtureComposition, out mutatingState);
            mutatingState = true;
            return 0;
        }
        
        public static void AddMolecule(Molecule molecule, float moles, in Dictionary<Molecule, float> mixtureComposition, out bool mutatingState)
        {
            mixtureComposition[molecule] = moles;
            mutatingState = true;
        }
        
        public static void RemoveMolecule(Molecule molecule, in Dictionary<Molecule, float> mixtureComposition, out bool mutatingState)
        {
            mixtureComposition.Remove(molecule);
            mutatingState = true;
        }
    }
}