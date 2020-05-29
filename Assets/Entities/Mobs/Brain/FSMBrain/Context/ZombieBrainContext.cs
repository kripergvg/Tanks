namespace Tanks.Mobs.Brain.FSMBrain
{
    public struct ZombieBrainContext
    {
        public bool TargetReached;

        public IEntity Target;

        public bool IsInsideScene;
    }
}