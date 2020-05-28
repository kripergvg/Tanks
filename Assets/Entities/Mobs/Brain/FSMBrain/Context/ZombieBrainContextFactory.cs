using Tanks.FSM;

namespace Tanks.Mobs.Brain.FSMBrain
{
    public readonly struct ZombieBrainContextFactory : IContextFactory<ZombieBrainContext>
    {
        public ZombieBrainContext Create()
        {
            return new ZombieBrainContext();
        }
    }
}