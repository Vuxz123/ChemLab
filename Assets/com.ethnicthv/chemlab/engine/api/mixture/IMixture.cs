using com.ethnicthv.chemlab.engine.molecule;

namespace com.ethnicthv.chemlab.engine.api.mixture
{
    public interface IMixture
    {
        public MixtureType GetMixtureType();
        public void Tick();
        public void AddMolecule(Molecule molecule, double moles);
        public void RemoveMolecule(Molecule molecule);
        public void SetMoles(Molecule molecule, double moles);
        public double GetMoles(Molecule molecule);
        public double AddMoles(Molecule molecule, double moles, out bool isMutating);
        public double SubtractMoles(Molecule molecule, double moles, out bool isMutating);
    }
}