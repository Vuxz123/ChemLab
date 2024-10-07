using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;

namespace com.ethnicthv.chemlab.engine.api.molecule.formula
{
    public record FormulaAtomData(
        Element Element,
        int Charge,
        bool InRing,
        bool IsCarbon,
        int HydrogenCount,
        int AvailableConnectivity,
        IReadOnlyList<Atom> Neighbors
    );
}