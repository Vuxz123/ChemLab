using System.Collections.Generic;
using System.Linq;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;
using com.ethnicthv.chemlab.engine.api.molecule.group;

namespace com.ethnicthv.chemlab.engine.molecule.group.detector
{
    public class OrganicAcidDetector : IGroupDetector
    {
        public bool ShouldApplyGroup(DetectingContext context, out Atom[] anchorAtom)
        {
            anchorAtom = null;
            var structure = context.Molecule.GetFormula().GetStructure();
            var oxygen = context.AtomList.FindAll(a => a.GetElement() == Element.Oxygen);
            
            if (oxygen.Count == 0) return false;

            var anchorAtoms = new List<Atom>();
            foreach (var atom in oxygen)
            {
                //Note: Rule 1 is to check if the oxygen atom is connected to a carbon atom and a hydrogen atom
                var bonds = structure[atom];
                if (bonds.Count != 2) continue;
                var x = bonds[0].GetDestinationAtom();
                var y = bonds[1].GetDestinationAtom();
                if (x.GetElement() == Element.Hydrogen && y.GetElement() == Element.Carbon)
                {
                    (x, y) = (y, x);
                    goto Rule2;
                }

                if (x.GetElement() == Element.Carbon || y.GetElement() == Element.Hydrogen)
                {
                    goto Rule2;
                }

                continue;
                
                //Note: Rule 2 is to check if the connected carbon atom is connected to 3 bonds and one of them is a double bond to an oxygen atom
                Rule2:
                var cBonds = structure[y];
                if (cBonds.Count != 3) continue;
                if (cBonds.Any(b=>b.GetDestinationAtom().GetElement() == Element.Oxygen && b.GetBondType() == Bond.BondType.Double))
                {
                    anchorAtoms.Add(atom);
                }
            }

            if (anchorAtoms.Count == 0) return false;

            anchorAtom = anchorAtoms.ToArray();
            return true;
        }

        public MoleculeGroup GetGroup()
        {
            return MoleculeGroup.Acid;
        }
    }
}