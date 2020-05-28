using System;
using System.Collections.Generic;
using Tanks.FSM;

namespace Tanks.Mobs.Brain.FSMBrain
{
    public class FSMZombieBrain : IZombieBrain
    {
        private readonly IEntityLocator _entityLocator;
        private readonly ITimeProvider _timeProvider;
        private readonly ITargetLocator _targetLocator;
        private readonly StateMachineFactory _stateMachineFactory;
        private readonly IEntity _mob;
        private readonly TargetChaser _targetChaser;
        private readonly IAttacker _attacker;
        private readonly TimeSpan _attackInterval;
        private IStateMachineTyped<ZombieBrainContextFactory, ZombieBrainContext, ZombieContextUpdater, IEntity> _stateMachine;

        public FSMZombieBrain(ITimeProvider timeProvider,
            ITargetLocator targetLocator,
            StateMachineFactory stateMachineFactory, 
            IEntity mob,
            TargetChaser targetChaser,
            IAttacker attacker,
            TimeSpan attackInterval)
        {
            _timeProvider = timeProvider;
            _targetLocator = targetLocator;
            _stateMachineFactory = stateMachineFactory;
            _mob = mob;
            _targetChaser = targetChaser;
            _attacker = attacker;
            _attackInterval = attackInterval;
        }

        public void Start()
        {
            var idleState = new IdleState();
            var followingState = new FollowingState(_targetChaser);
            var attackState = new AttackState(_timeProvider, _attackInterval, _attacker);
            
            var brainStateMachineBuilder = _stateMachineFactory.CreateBuilder<ZombieBrainContextFactory, ZombieBrainContext, ZombieContextUpdater, IEntity>();
            brainStateMachineBuilder.ConfigureState(idleState, b =>
            {
                b.If(c => c.Target != null)
                    .ThenSetState(followingState);
            });
            
            brainStateMachineBuilder.ConfigureState(followingState, b =>
            {
                b.If(c => c.Target == null)
                    .ThenSetState(idleState);
                b.If(c => c.Target != null && c.Reached)
                    .ThenSetState(attackState);
            });
            
            brainStateMachineBuilder.ConfigureState(attackState, b =>
            {
                b.If(c => c.Target == null)
                    .ThenSetState(idleState);
                b.If(c => c.Target != null && !c.Reached)
                    .ThenSetState(followingState);
            });

            var contextFactory = new ZombieBrainContextFactory();
            var contextUpdater = new ZombieContextUpdater(_targetChaser, _targetLocator);
            
            _stateMachine = brainStateMachineBuilder.BuildRoot(contextFactory, contextUpdater);
            _stateMachine.Start(idleState);
        }

        public void Update()
        {
            _stateMachine.Update();
        }
        
        public void OnEntityTriggerEnter(IEntity entity)
        {
           _stateMachine.OnEntityTriggerEnter(entity);
        }

        public void OnEntityTriggerExit(IEntity entity)
        {
            _stateMachine.OnEntityTriggerExit(entity);
        }

        public void Stop()
        {
            _stateMachine.Stop();
        }
    }

    public readonly struct ZombieContextUpdater : IContextUpdater<ZombieBrainContext>
    {
        private readonly TargetChaser _chaser;
        private readonly ITargetLocator _targetLocator;

        public ZombieContextUpdater(TargetChaser chaser, ITargetLocator targetLocator)
        {
            _chaser = chaser;
            _targetLocator = targetLocator;
        }
        
        public void Update(ref ZombieBrainContext context)
        {
            context.Reached = _chaser.TargetReached;
            context.Target = _targetLocator.GetTarget();
        }
    }
}