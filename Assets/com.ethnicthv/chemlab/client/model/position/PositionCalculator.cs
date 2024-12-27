using UnityEngine;
using com.ethnicthv.chemlab.client.model.position.topology;
using com.ethnicthv.chemlab.client.model.position.topology.rings;

namespace com.ethnicthv.chemlab.client.model.position
{
    public class PositionCalculator
    {
        public readonly DefaultTopology NonRingsTopology = new();
        public readonly DefaultRingsTopology RingsTopology = new();
        
        public Vector3 GetNextPosition(Vector3 inDirection, int maxBranch, int branchIndex, bool isRing)
        {
            return isRing ? RingsTopology.GetNextPriorityPosition(inDirection, maxBranch, branchIndex) : NonRingsTopology.GetNextPriorityPosition(inDirection, maxBranch, branchIndex);
        }
    }
}