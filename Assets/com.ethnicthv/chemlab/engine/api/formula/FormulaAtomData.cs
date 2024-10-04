using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.element;

namespace com.ethnicthv.chemlab.engine.api.formula
{
    public record FormulaAtomData(
        Element Element,
        int Charge,
        bool InRing,
        bool IsCarbon,
        int HydrogenCount,
        int AvailableConnectivity,
        IReadOnlyList<engine.Atom> Neighbors
    );
}