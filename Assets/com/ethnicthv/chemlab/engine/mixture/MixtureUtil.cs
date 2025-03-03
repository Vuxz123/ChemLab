using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.molecule;
using UnityEngine;

namespace com.ethnicthv.chemlab.engine.mixture
{
    public static class MixtureUtil
    {
        public static float AddMoles(Molecule molecule, float moles,
            in Queue<Molecule> newMolecules,
            in Dictionary<Molecule, int> toRemove, 
            in Dictionary<Molecule, float> mixtureComposition, 
            in Dictionary<Molecule, float> states,
            in List<Molecule> novels, 
            ref bool mutatingState)
        {
            Debug.Log("Adding " + moles + " moles of " + molecule.GetFullID() + " to mixture");
            //Note: if the molecule is not in the mixture, check if it is a novel molecule
            if (mixtureComposition.ContainsKey(molecule))
                return NormalAdd(molecule, moles, toRemove, mixtureComposition, ref mutatingState);
            
            //Note: if the molecule is novel, check if
            if (molecule.IsNovel())
            {
                //Note: check if any molecule in the mixture has the same full id as the novel molecule
                Molecule found = null;
                foreach (var m in mixtureComposition.Keys)
                {
                    if (!m.IsNovel() || !m.GetFullID().Equals(molecule.GetFullID())) continue;
                    found = m;
                    break;
                }

                //Note: if a molecule with the same full id is found, add the moles to it
                if (found != null)
                {
                    NormalAdd(found, moles, toRemove, mixtureComposition, ref mutatingState);
                    return mixtureComposition[found];
                }
            }
                
            //Note: else, add the molecule to the mixture
            AddMoleculeNonCheck(molecule, moles, newMolecules, mixtureComposition, states, out mutatingState);
            mutatingState = true;
            return mixtureComposition[molecule];

        }

        public static void AddMolecule(Molecule molecule, float moles,
            in Queue<Molecule> newMolecules,
            in Dictionary<Molecule, float> mixtureComposition,
            in Dictionary<Molecule, float> states,
            in List<Molecule> novels,
            out bool mutatingState)
        {
            if (molecule.IsNovel()) novels.Add(molecule);
            AddMoleculeNonCheck(molecule, moles, newMolecules, mixtureComposition, states, out mutatingState);
        }
        
        public static void RemoveMolecule(Molecule molecule, 
            in Dictionary<Molecule, int> toRemove, 
            out bool mutatingState)
        {
            toRemove.Add(molecule, 10);
            mutatingState = true;
        }
        
        private static void AddMoleculeNonCheck(Molecule molecule, float moles,
            in Queue<Molecule> newMolecules,
            in Dictionary<Molecule, float> mixtureComposition, 
            in Dictionary<Molecule, float> states,
            out bool mutatingState)
        {
            mixtureComposition[molecule] = moles;
            states[molecule] = 0.0f;
            newMolecules.Enqueue(molecule);
            mutatingState = true;
        }

        private static float NormalAdd(Molecule molecule, float moles, 
            in Dictionary<Molecule, int> toRemove, 
            in Dictionary<Molecule, float> mixtureComposition, 
            ref bool mutatingState)
        {
            mixtureComposition[molecule] = Mathf.Max(0, mixtureComposition[molecule] + moles);
            
            if (mixtureComposition[molecule] > 0)
            {
                toRemove.Remove(molecule);
                return mixtureComposition[molecule];
            }
            
            RemoveMolecule(molecule, toRemove, out mutatingState);
            mutatingState = true;
            return mixtureComposition[molecule];
        }
    }
}