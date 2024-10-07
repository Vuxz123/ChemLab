using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;
using com.ethnicthv.chemlab.engine.api.molecule.formula;

namespace com.ethnicthv.chemlab.engine.api.molecule.group
{
    public interface IGroupDetection<M> where M : IGroupModel
    {
        public IReadOnlyList<Element> SubscribeToElements();
        public bool IsFormulaContainsGroup(IFormula formula);
    }
}