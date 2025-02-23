using com.ethnicthv.chemlab.engine.molecule;

namespace com.ethnicthv.chemlab.engine.api.error.molecule
{
    public abstract class MoleculeException : ChemistryException
    {
        public readonly Molecule Molecule;

        protected MoleculeException(string message, Molecule molecule) : base("Problem with Molecule '" + molecule.GetFormula().Serialize() + "': " + message + ".")
        {
            Molecule = molecule;
        }
    }
}