using com.ethnicthv.chemlab.engine.api.element;

namespace com.ethnicthv.chemlab.engine.formula.topology
{
    public class Toluene : FormulaTopology
    {
        public Toluene() : base(TolueneFactory, "toluene")
        {
        }

        private static Formula TolueneFactory(Formula formula = null)
        {
            var f = formula != null
                ? formula.AddRing(6, new Atom(Element.Carbon))
                : Formula.CreateNewRingCarbonFormula(6);
            return f
                    .SetAtom(new Atom(Element.Carbon), Bond.BondType.Double)
                    .SetAtom(new Atom(Element.Carbon))
                    .SetAtom(new Atom(Element.Carbon), Bond.BondType.Double)
                    .SetAtom(new Atom(Element.Carbon))
                    .SetAtom(new Atom(Element.Carbon), Bond.BondType.Double)
                    .FormRing(5)
                .AddAtom(new Atom(Element.Carbon));
        }
    }
}