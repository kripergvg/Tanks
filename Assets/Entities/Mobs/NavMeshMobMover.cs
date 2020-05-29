using System.Buffers;
using Tanks.Mobs.Brain.FSMBrain;
using UnityEngine;
using UnityEngine.AI;

namespace Tanks.Mobs
{
    public class NavMeshMobMover : IMover
    {
        private readonly NavMeshPath _calculateDistancePath = new NavMeshPath();
        private readonly NavMeshAgent _agent;
        
        private IEntity _target;
        private bool _targetReached;

        private const int MaxCorners = 10000;

        public NavMeshMobMover(NavMeshAgent agent)
        {
            _agent = agent;
        }
        
        public void MoveToPoint(Vector3 position)
        {
            // _agent.war
            _agent.isStopped = false;
            _agent.SetDestination(position);
        }

        public float? CalculateDistance(Vector3 position)
        {
            if (_agent.CalculatePath(position, _calculateDistancePath)
                && _calculateDistancePath.status == NavMeshPathStatus.PathComplete)
            {
                return GetDistance(_calculateDistancePath);
            }

            return null;
        }

        private float GetDistance(NavMeshPath path)
        {
            var cornersBuffer = ArrayPool<Vector3>.Shared.Rent(MaxCorners);
            try
            {
                var cornersCount = path.GetCornersNonAlloc(cornersBuffer);
                float distance = 0;
                if (cornersCount > 1)
                {
                    for (var cornerIndex = 1; cornerIndex < cornersCount; cornerIndex++)
                    {
                        distance += Vector3.Distance(cornersBuffer[cornerIndex - 1], cornersBuffer[cornerIndex]);
                    }
                }

                return distance;
            }
            finally
            {
                ArrayPool<Vector3>.Shared.Return(cornersBuffer);
            }
        }

        public void StopMoving()
        {
            _agent.isStopped = true;
        }
    }
}