using com.ethnicthv.chemlab.engine.api.element;

namespace com.ethnicthv.chemlab.engine.formula.topology
{
    public class Benzene : FormulaTopology
    {
        public Benzene() : base(BenzeneFactory, "benzene") { }

        private static Formula BenzeneFactory(Formula formula = null)
        {
            var f = formula != null ? 
                formula.AddRing(6, new Atom(Element.Carbon)) : 
                Formula.CreateNewRingCarbonFormula(6);
            return f
                .SetAtom(new Atom(Element.Carbon), Bond.BondType.Double)
                .SetAtom(new Atom(Element.Carbon), Bond.BondType.Single)
                .SetAtom(new Atom(Element.Carbon), Bond.BondType.Double)
                .SetAtom(new Atom(Element.Carbon), Bond.BondType.Single)
                .SetAtom(new Atom(Element.Carbon), Bond.BondType.Double)
                .FormRing(5);
        }
    }
}