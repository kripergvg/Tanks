using Tanks.Entities;

namespace Tanks.Mobs.Brain.FSMBrain.Context
{
    public struct ZombieBrainContext
    {
        public bool TargetReached;

        public IEntity Target;

        public bool IsInsideScene;
    }
}