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
        private Dictionary<MoleculeGroup, List<IFunctionalGroup>> _groups = new();
        
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
            if (!_groups.ContainsKey(group)) _groups.Add(group, new List<IFunctionalGroup>());
        }
        
        public void AddFunctionalGroup(MoleculeGroup group, IFunctionalGroup atom)
        {
            if (!_groups.ContainsKey(group)) return;
            _groups[group].Add(atom);
        }
        
        public void AddFunctionalGroup(MoleculeGroup group, IFunctionalGroup[] atom)
        {
            if (!_groups.ContainsKey(group)) return;
            _groups[group].AddRange(atom);
        }
        
        public void RemoveFunctionalGroup(MoleculeGroup group, IFunctionalGroup atom)
        {
            if (!_groups.ContainsKey(group)) return;
            _groups[group].Remove(atom);
        }
        
        public IReadOnlyCollection<IFunctionalGroup> GetAtomsInGroup(MoleculeGroup group)
        {
            return _groups[group];
        }

        public bool IsOrganic()
        {
            return _isOrganic;
        }

        public bool IsIon()
        {
            return false;
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