using System;
using Tanks.DI;
using Tanks.FSM;

namespace Tanks.Mobs.Brain.FSMBrain
{
    public class EnterDoorState : StateMachine<ZombieBrainContextFactory, ZombieBrainContext,  ZombieContextUpdater, IEntity>.State
    {
        private readonly IDoor[] _doors;
        private readonly IEntity _entity;
        private readonly IMover _mover;

        private IDoor _selectedDoor;

        public EnterDoorState(IDoor[] doors, IEntity entity, IMover mover)
        {
            _doors = doors;
            _entity = entity;
            _mover = mover;
        }

        public override void Update(in ZombieBrainContext context)
        {
            if (_selectedDoor == null)
            {
                _selectedDoor = GetClosestDoor();
                _mover.MoveToPoint(_selectedDoor.Enter);
            }
        }

        private IDoor GetClosestDoor()
        {
            IDoor selectedDoor = null;
            var minDistance = float.MaxValue;
            foreach (var door in _doors)
            {
                var distanceToDoor = _mover.CalculateDistance(door.Enter);
                if (distanceToDoor != null)
                {
                    if (minDistance > distanceToDoor)
                    {
                        selectedDoor = door;
                        minDistance = distanceToDoor.Value;
                    }
                }
            }

            if (selectedDoor == null)
            {
                throw new Exception("Couldn't find door");
            }

            return selectedDoor;
        }
    }
}