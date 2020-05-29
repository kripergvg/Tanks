using Tanks.FSM;

namespace Tanks.Mobs.Brain.FSMBrain.Context
{
    public readonly struct ZombieBrainContextFactory : IContextFactory<ZombieBrainContext>
    {
        public ZombieBrainContext Create()
        {
            return new ZombieBrainContext();
        }
    }
}