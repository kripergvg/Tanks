using UnityEngine;

namespace Tanks.Mobs.Brain.FSMBrain
{
    public interface IMover
    {
        void MoveToPoint(Vector3 position);

        void StopMoving();
    }
}