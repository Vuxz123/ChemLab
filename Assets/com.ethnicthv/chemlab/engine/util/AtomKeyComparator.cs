using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.atom;
using UnityEngine;

namespace com.ethnicthv.chemlab.engine.util
{
    public class AtomKeyComparator : IComparer<Atom>
    {
        public int Compare(Atom x, Atom y)
        {
            if (x == null || y == null)
            {
                Debug.LogWarning("AtomKeyComparator: One of the atoms is null.");
                return 0;
            }
            var t = x.GetElement() - y.GetElement();
            return t == 0? x.GetCharge() - y.GetCharge() : t;
        }
    }
}