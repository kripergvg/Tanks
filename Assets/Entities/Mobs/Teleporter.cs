using Tanks.DI;
using UnityEngine;

namespace Tanks.Mobs.Brain.FSMBrain
{
    public class Teleporter : IPositionDetector
    {
        private readonly IEntity _entity;
        private bool _insideScene;

        public Teleporter(IEntity entity)
        {
            _entity = entity;
        }
        
        public bool IsInsideScene()
        {
            return _insideScene;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IDoor>(out var door))
            {
                _entity.Position = door.Exit;
                _insideScene = true;
            }
        }
    }
}