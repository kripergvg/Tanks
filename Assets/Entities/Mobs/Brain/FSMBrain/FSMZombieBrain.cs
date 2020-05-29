using System;
using Tanks.DI;
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
        private readonly IDoor[] _doors;
        private readonly IMover _mover;
        private readonly IPositionDetector _positionDetector;
        private IStateMachineTyped<ZombieBrainContextFactory, ZombieBrainContext, ZombieContextUpdater, IEntity> _stateMachine;

        public FSMZombieBrain(ITimeProvider timeProvider,
            ITargetLocator targetLocator,
            StateMachineFactory stateMachineFactory, 
            IEntity mob,
            TargetChaser targetChaser,
            IAttacker attacker,
            TimeSpan attackInterval,
            IDoor[] doors,
            IMover mover,
            IPositionDetector positionDetector)
        {
            _timeProvider = timeProvider;
            _targetLocator = targetLocator;
            _stateMachineFactory = stateMachineFactory;
            _mob = mob;
            _targetChaser = targetChaser;
            _attacker = attacker;
            _attackInterval = attackInterval;
            _doors = doors;
            _mover = mover;
            _positionDetector = positionDetector;
        }

        public void Start()
        {
            var idleState = new IdleState();
            var enterDoorState = new EnterDoorState(_doors, _mob, _mover);
            var followingState = new FollowingState(_targetChaser);
            var attackState = new AttackState(_timeProvider, _attackInterval, _attacker);

            var brainStateMachineBuilder = _stateMachineFactory.CreateBuilder<ZombieBrainContextFactory, ZombieBrainContext, ZombieContextUpdater, IEntity>();

            brainStateMachineBuilder.ConfigureState(idleState, b =>
            {
                b.If(c => !c.IsInsideScene)
                    .ThenSetState(enterDoorState);
            });
            
            brainStateMachineBuilder.ConfigureState(enterDoorState, b =>
            {
                b.If(c => c.Target == null)
                    .ThenSetState(idleState);
                b.If(c => c.IsInsideScene && c.Target != null)
                    .ThenSetState(followingState);
            });
            
            brainStateMachineBuilder.ConfigureState(followingState, b =>
            {
                b.If(c => c.Target == null)
                    .ThenSetState(idleState);
                b.If(c => c.Target != null && c.TargetReached)
                    .ThenSetState(attackState);
            });
            
            brainStateMachineBuilder.ConfigureState(attackState, b =>
            {
                b.If(c => c.Target == null)
                    .ThenSetState(idleState);
                b.If(c => c.Target != null && !c.TargetReached)
                    .ThenSetState(followingState);
            });

            var contextFactory = new ZombieBrainContextFactory();
            var contextUpdater = new ZombieContextUpdater(_targetChaser, _targetLocator, _positionDetector);
            
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
        private readonly IPositionDetector _positionDetector;

        public ZombieContextUpdater(TargetChaser chaser,
            ITargetLocator targetLocator,
            IPositionDetector positionDetector)
        {
            _chaser = chaser;
            _targetLocator = targetLocator;
            _positionDetector = positionDetector;
        }
        
        public void Update(ref ZombieBrainContext context)
        {
            context.TargetReached = _chaser.TargetReached;
            context.Target = _targetLocator.GetTarget();
            context.IsInsideScene = _positionDetector.IsInsideScene();
        }
    }
}