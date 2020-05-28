using System.Collections.Generic;

namespace Tanks.FSM
{
    public interface IContextFactory<out TContext>
    {
        TContext Create();
    }

    public class StateMachine<TContextFactory, TContext, TContextUpdater, TEntity> : IStateMachineTyped<TContextFactory, TContext, TContextUpdater, TEntity>
        where TContextFactory : IContextFactory<TContext>
        where TContextUpdater: IContextUpdater<TContext>
    {
        private readonly TContextUpdater _contextUpdater;
        private readonly Dictionary<State, StateMachineFactory.StateInfo<TContextFactory, TContext, TContextUpdater, TEntity>> _stateInfos;
        private TContext _context;

        private State _currentState;
        private bool _started;
        private bool _stopped;

        public StateMachine(TContextFactory contextFactory, 
            TContextUpdater contextUpdater,
            Dictionary<State, StateMachineFactory.StateInfo<TContextFactory, TContext, TContextUpdater, TEntity>> stateInfos)
        {
            _context = contextFactory.Create();
            _contextUpdater = contextUpdater;
            _stateInfos = stateInfos;
        }

        public void Start(State state)
        {
            _started = true;
            SetNewState(state);
        }

        public void OnEntityTriggerEnter(TEntity entity)
        {
            if (IsActive())
            {
                _currentState.OnEntityTriggerEnter(entity);
            }
        }

        public void OnEntityTriggerExit(TEntity entity)
        {
            if (IsActive())
            {
                _currentState.OnEntityTriggerExit(entity);
            }
        }

        public void Stop()
        {
            _stopped = true;
        }

        private bool IsActive()
        {
            return _started && !_stopped;
        }

        public void Update()
        {
            if (IsActive())
            {
                _contextUpdater.Update(ref _context);
                
                foreach (var transition in _stateInfos[_currentState].Transitions)
                {
                    if (transition.ShouldTransit(in _context))
                    {
                        _currentState.OnEnd(in _context);

                        var newState = transition.Transit(in _context);
                        SetNewState(newState);
                    }
                }

                _currentState.Update(in _context);
            }
        }

        private void SetNewState(State newState)
        {
            _currentState = newState;
            _currentState.OnStart(in _context);
        }


        public abstract class State
        {
            public virtual void OnStart(in TContext context)
            {
            }

            public virtual void OnEnd(in TContext context)
            {
            }

            public virtual void Update(in TContext context)
            {
            }

            public virtual void OnEntityTriggerEnter(TEntity entity)
            {
            }

            public virtual void OnEntityTriggerExit(TEntity entity)
            {
                
            }
        }
    }
}