using Tanks.FSM;

namespace Tanks.Mobs.Brain.FSMBrain
{
    public class FollowingState : StateMachine<ZombieBrainContextFactory, ZombieBrainContext, ZombieContextUpdater, IEntity>.State
    {
        private readonly TargetChaser _targetChaser;

        public FollowingState(TargetChaser targetChaser)
        {
            _targetChaser = targetChaser;
        }
        
        public override void Update(in ZombieBrainContext context)
        {
            _targetChaser.SetTarget(context.Target);
        }
    }
}