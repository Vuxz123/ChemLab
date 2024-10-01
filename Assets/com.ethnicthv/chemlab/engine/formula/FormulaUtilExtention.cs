namespace com.ethnicthv.chemlab.engine.formula
{
    public static class FormulaUtilExtension
    {
        public static Formula AddCarbonyl(this Formula formula)
        {
            formula.AddAtom(new Atom(Element.Oxygen), Bond.BondType.Double);
            return formula;
        }

        public static Formula AddHydroxyl(this Formula formula)
        {
            formula.AddAtom(new Atom(Element.Oxygen));
            return formula;
        }

        public static Formula AddAmine(this Formula formula)
        {
            formula.AddAtom(new Atom(Element.Nitrogen));
            return formula;
        }

        public static Formula AddCarboxyl(this Formula formula)
        {
            formula.AddAtom(new Atom(Element.Oxygen), Bond.BondType.Double);
            formula.AddAtom(new Atom(Element.Oxygen));
            return formula;
        }
    }
}