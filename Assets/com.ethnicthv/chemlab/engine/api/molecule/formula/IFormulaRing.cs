﻿using com.ethnicthv.chemlab.engine.api.atom;

namespace com.ethnicthv.chemlab.engine.api.molecule.formula
{
    public interface IFormulaRing: IFormulaAtomDataChecker, IBondBreaker
    {
        public void AddBranch(int position, Atom sideBranch,
            Bond.BondType bondType = Bond.BondType.Single);
        public void AddBranch(int position, IFormula sideBranch,
            Bond.BondType bondType = Bond.BondType.Single);
    }
}