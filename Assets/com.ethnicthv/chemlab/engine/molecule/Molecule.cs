using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.molecule;
using com.ethnicthv.chemlab.engine.api.molecule.formula;
using com.ethnicthv.chemlab.engine.api.molecule.group;
using com.ethnicthv.chemlab.engine.formula;

namespace com.ethnicthv.chemlab.engine.molecule
{
    public class Molecule : IMutableMolecule
    {
        private readonly Formula _formula;
        private Dictionary<MoleculeGroup, List<Atom>> _groups = new();
        
        private bool _isOrganic;
        
        private Molecule() {}
        
        public Molecule(Formula formula)
        {
            _formula = formula;
        }

        public IFormula GetFormula()
        {
            return _formula;
        }
        
        public IReadOnlyCollection<MoleculeGroup> GetGroups()
        {
            return _groups.Keys;
        }
        
        public void DeleteGroup(MoleculeGroup group)
        {
            _groups.Remove(group);
        }
        
        public void AddGroup(MoleculeGroup group)
        {
            if (!_groups.ContainsKey(group)) _groups.Add(group, new List<Atom>());
        }
        
        public void AddAtomToGroup(MoleculeGroup group, Atom atom)
        {
            if (!_groups.ContainsKey(group)) return;
            _groups[group].Add(atom);
        }
        
        public void AddAtomToGroup(MoleculeGroup group, Atom[] atom)
        {
            if (!_groups.ContainsKey(group)) return;
            _groups[group].AddRange(atom);
        }
        
        public void RemoveAtomFromGroup(MoleculeGroup group, Atom atom)
        {
            if (!_groups.ContainsKey(group)) return;
            _groups[group].Remove(atom);
        }
        
        public IReadOnlyCollection<Atom> GetAtomsInGroup(MoleculeGroup group)
        {
            return _groups[group];
        }

        public bool IsOrganic()
        {
            return _isOrganic;
        }

        public bool IsIon()
        {
            return _formula.GetChargeAtom().Count > 0;
        }

        public bool IsAromatic()
        {
            return _formula.IsAromatic;
        }

        public bool IsCyclic()
        {
            return _formula.IsCyclic;
        }

        public void ClearGroups()
        {
            _groups.Clear();
        }
    }
}