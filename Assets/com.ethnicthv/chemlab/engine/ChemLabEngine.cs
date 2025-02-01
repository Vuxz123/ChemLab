using com.ethnicthv.chemlab.engine.molecule.group;
using com.ethnicthv.chemlab.engine.molecule.group.detector;

namespace com.ethnicthv.chemlab.engine
{
    public class ChemLabEngine
    {
        public static ChemLabEngine Instance { get; } = new();
        
        private ChemLabEngine() { }
        
        public void Setup()
        {
            // Register detectors
            GroupDetectingProgram.Instance.RegisterDetector(new AlcoholGroupDetector());
            GroupDetectingProgram.Instance.RegisterDetector(new OrganicAcidDetector());
        }
    }
}
