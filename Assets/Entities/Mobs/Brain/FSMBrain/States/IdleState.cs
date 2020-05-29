using Tanks.Entities;
using Tanks.FSM;
using Tanks.Mobs.Brain.FSMBrain.Context;

namespace Tanks.Mobs.Brain.FSMBrain.States
{
    public class IdleState :  StateMachine<ZombieBrainContextFactory, ZombieBrainContext, ZombieContextUpdater, IEntity>.State
    {
        
    }
}