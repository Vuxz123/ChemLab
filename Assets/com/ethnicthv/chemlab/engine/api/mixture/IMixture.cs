using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.molecule;
using com.ethnicthv.chemlab.engine.molecule;

namespace com.ethnicthv.chemlab.engine.api.mixture
{
    public interface IMixture
    {
        public void Tick();
        public void RemoveMolecule(Molecule molecule);
        public void SetMoles(Molecule molecule, float moles);
        public float GetMoles(Molecule molecule);
        public float AddMoles(Molecule molecule, float moles, out bool isMutating);
        IReadOnlyDictionary<Molecule, float> GetMixtureComposition();
        float GetTemperature();
    }
}