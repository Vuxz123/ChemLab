using System.Collections.Generic;

namespace com.ethnicthv.chemlab.engine.formula
{
    public delegate Formula FormulaFactory(Formula formula = null);
    public abstract class FormulaTopology
    {
        public string TopologyNamespace { get; }
        public FormulaFactory Factory { get; }
        public FormulaTopology(FormulaFactory factory, string topologyNamespace)
        {
            Factory = factory;
            TopologyNamespace = topologyNamespace;
            
            RegisterTopology(this);
        }
        
        private static readonly Dictionary<string, FormulaTopology> Topologies = new();
        public static void RegisterTopology(FormulaTopology formulaTopology)
        {
            Topologies[formulaTopology.TopologyNamespace] = formulaTopology;
        }
    }
}