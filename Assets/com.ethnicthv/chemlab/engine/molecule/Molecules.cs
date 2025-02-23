using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;
using com.ethnicthv.chemlab.engine.api.molecule;
using com.ethnicthv.chemlab.engine.formula;

namespace com.ethnicthv.chemlab.engine.molecule
{
    public class Molecules
    {
        private static Molecule.Builder Builder()
        {
            return new Molecule.Builder();
        }

        public static readonly Molecule Water = Builder().ID("water")
            .Structure(Formula.Deserialize("linear:HOH")).BoilingPoint(100.0F).Density(1000.0F)
            .SpecificHeatCapacity(4160.0F).Tag(MoleculeTag.Solvent)
            .Build();

        public static readonly Molecule Proton = Builder().ID("proton")
            .Structure(Formula.CreateNewFormula(new Atom(Element.Hydrogen, 1))).Build();

        public static readonly Molecule Hydroxide = Builder().ID("hydroxide")
            .Structure(Formula.CreateNewFormula(new Atom(Element.Hydrogen))
                .AddAtom(new Atom(Element.Oxygen, -1))).Density(900.0F).Build();

        public static readonly Molecule Oleum = Builder().ID("oleum")
            .Structure(Formula.Deserialize("linear:HOS(=O)(=O)OS(=O)(=O)OH"))
            .BoilingPoint(10.0F).Density(1820.0F).SpecificHeatCapacity(2600.0F)
            .Tag(MoleculeTag.AcutelyToxic).Build();

        public static readonly Molecule SulfuricAcid = Builder().ID("sulfuric_acid")
            .Structure(Formula.Deserialize("linear:OS(=O)(=O)O")).BoilingPoint(337.0F)
            .Density(1830.2F).MolarHeatCapacity(83.68F).Tag(MoleculeTag.AcutelyToxic)
            .Build();

        public static readonly Molecule SodiumIon = Builder().ID("sodium_ion")
            .Structure(Formula.CreateNewFormula(new Atom(Element.Sodium, 1))).Density(900.0F).Build();

        public static readonly Molecule Chloride = Builder().ID("chloride")
            .Structure(Formula.CreateNewFormula(new Atom(Element.Chlorine, -1))).Build();
    }
}