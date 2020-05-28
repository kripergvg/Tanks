using Tanks.Mobs.Brain.FSMBrain;
using UnityEngine;
using UnityEngine.AI;

namespace Tanks.Mobs
{
    public class NavMeshMobMover : IMover
    {
        private readonly NavMeshAgent _agent;
        private IEntity _target;
        private bool _targetReached;

        public NavMeshMobMover(NavMeshAgent agent)
        {
            _agent = agent;
        }
        
        public void MoveToPoint(Vector3 position)
        {
            _agent.isStopped = false;
            _agent.SetDestination(position);
        }

        public void StopMoving()
        {
            _agent.isStopped = true;
        }
    }
}