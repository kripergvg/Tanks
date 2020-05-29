using System;
using Tanks.FSM;
using Tanks.Mobs.Brain.FSMBrain.Context;

namespace Tanks.Mobs.Brain.FSMBrain.States
{
    public class AttackState : StateMachine<ZombieBrainContextFactory, ZombieBrainContext, ZombieContextUpdater, IEntity>.State
    {
        private readonly ITimeProvider _timeProvider;
        private readonly TimeSpan _attackInterval;
        private readonly IAttacker _attacker;
        private float _lastAttackTime;

        public AttackState(ITimeProvider timeProvider, TimeSpan attackInterval, IAttacker attacker)
        {
            _timeProvider = timeProvider;
            _attackInterval = attackInterval;
            _attacker = attacker;
        }
        
        public override void Update(in ZombieBrainContext context)
        {
            if (TimeSpan.FromSeconds(_timeProvider.Time - _lastAttackTime) >= _attackInterval)
            {
                _attacker.Attack(context.Target);
                _lastAttackTime = _timeProvider.Time;
            }
        }
    }
}