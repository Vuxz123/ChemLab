using UnityEngine;

namespace com.ethnicthv.chemlab.client.model.position.topology
{
    public class DefaultTopology
    {
        public Vector3 GetNextPriorityPosition(Vector3 inDirection, int maxBranch, int branchIndex)
        {
            var angle = 360f / maxBranch;
            var randomToken = Random.Range(0, 1f) > 0.5f ? 1 : -1;
            var rotation = Quaternion.Euler(0, randomToken * angle * branchIndex, 0);
            return rotation * inDirection;
        }
    }
}