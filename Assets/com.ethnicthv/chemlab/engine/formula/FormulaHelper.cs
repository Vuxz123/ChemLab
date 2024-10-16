using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.atom;

namespace com.ethnicthv.chemlab.engine.formula
{
    public abstract class FormulaHelper
    {
        public static void AddAtomToStructure(Atom rootAtom, Atom addedAtom, Dictionary<Atom, List<Bond>> mutableStructure, Bond.BondType bondType)
        {
            if (!mutableStructure.ContainsKey(rootAtom))
            {
                mutableStructure[rootAtom] = new List<Bond>();
            }
            mutableStructure.Add(addedAtom, new List<Bond>());
            mutableStructure[rootAtom].Add(new Bond(rootAtom, addedAtom, bondType));
            mutableStructure[addedAtom].Add(new Bond(addedAtom, rootAtom, bondType));
        }
        
        public static void AddBondToStructure(Atom srcAtom, Atom dstAtom, Dictionary<Atom, List<Bond>> mutableStructure, Bond.BondType bondType)
        {
            if (!mutableStructure.ContainsKey(srcAtom))
            {
                mutableStructure[srcAtom] = new List<Bond>();
            }
            if (!mutableStructure.ContainsKey(dstAtom))
            {
                mutableStructure[dstAtom] = new List<Bond>();
            }
            mutableStructure[srcAtom].Add(new Bond(srcAtom, dstAtom, bondType));
            mutableStructure[dstAtom].Add(new Bond(dstAtom, srcAtom, bondType));
        }
        
        public static int GetAvailableConnectivity(Atom atom, Dictionary<Atom, List<Bond>> structure)
        {
            if (!structure.ContainsKey(atom))
            {
                return atom.GetMaxConnectivity();
            }
            return atom.GetMaxConnectivity() - structure[atom].Count;
        }
    }
}