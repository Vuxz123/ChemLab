namespace com.ethnicthv.chemlab.engine.formula.topology
{
    public class Linear : FormulaTopology
    {
        public Linear() : base(LinearFactory, "linear")
        {
        }
        
        private static Formula LinearFactory(Formula formula = null)
        {
            return Formula.CreateNewCarbonFormula();
        }
    }
}