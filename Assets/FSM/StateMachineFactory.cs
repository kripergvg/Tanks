using System;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;

namespace Tanks.FSM
{
   public class StateMachineFactory
    {
        public StateMachineFactory()
        {
        }

        public StateMachineBuilder<TContextFactory, TContext, TContextUpdater, TEntity> CreateBuilder<TContextFactory, TContext, TContextUpdater, TEntity>()
            where TContextFactory :  IContextFactory<TContext>
            where TContextUpdater: IContextUpdater<TContext>
        {
            var stateInfos = new Dictionary<StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State, StateInfo<TContextFactory, TContext, TContextUpdater, TEntity>>();
            return new StateMachineBuilder<TContextFactory, TContext, TContextUpdater, TEntity>(stateInfos);
        }

        public readonly struct StateInfo<TContextFactory, TContext, TContextUpdater, TEntity>
            where TContextFactory :  IContextFactory<TContext>
            where TContextUpdater: IContextUpdater<TContext>
        {
            public StateInfo(List<Transition<TContextFactory, TContext, TContextUpdater, TEntity>> transitions)
            {
                Transitions = transitions;
            }
            
            public List<Transition<TContextFactory, TContext, TContextUpdater, TEntity>> Transitions { get; }
        }
        
        public readonly struct StateMachineBuilder<TContextFactory, TContext, TContextUpdater, TEntity>
            where TContextFactory :  IContextFactory<TContext>
            where TContextUpdater: IContextUpdater<TContext>
        {
            private readonly Dictionary<StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State, StateInfo<TContextFactory, TContext, TContextUpdater, TEntity>> _stateInfos;
            
            public StateMachineBuilder(Dictionary<StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State, StateInfo<TContextFactory, TContext, TContextUpdater, TEntity>> stateInfos)
            {
                _stateInfos = stateInfos;
            }

            public void ConfigureState(StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State state,
                Action<StateTransitionsBuilder> transitionsBuilder)
            {
                _stateInfos[state] = new StateInfo<TContextFactory, TContext, TContextUpdater, TEntity>(new List<Transition<TContextFactory, TContext, TContextUpdater, TEntity>>());
                var stateTransitionBuilder = new StateTransitionsBuilder(_stateInfos, state);
                transitionsBuilder(stateTransitionBuilder);
            }

            public IStateMachineTyped<TContextFactory, TContext, TContextUpdater, TEntity> BuildRoot(TContextFactory contextFactory, TContextUpdater contextUpdater)
            {
                var stateMachine = new StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>(contextFactory, contextUpdater, _stateInfos);
                return stateMachine;
            }

            public readonly struct StateTransitionsBuilder
            {
                private readonly Dictionary<StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State, StateInfo<TContextFactory, TContext, TContextUpdater, TEntity>> _stateInfos;
                private readonly StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State _fromState;

                public StateTransitionsBuilder(Dictionary<StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State, StateInfo<TContextFactory, TContext, TContextUpdater, TEntity>> stateInfos,
                    StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State fromState)
                {
                    _stateInfos = stateInfos;
                    _fromState = fromState;
                }
                
                public IfBuilder If(Func<TContext, bool> predicate)
                {
                    return new IfBuilder(_stateInfos,_fromState,  predicate);
                }
                
                public readonly struct IfBuilder
                {
                    private readonly Dictionary<StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State, StateInfo<TContextFactory, TContext, TContextUpdater, TEntity>> _stateInfos;
                    private readonly StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State _fromState;
                    private readonly Func<TContext, bool> _predicate;

                    public IfBuilder(Dictionary<StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State, StateInfo<TContextFactory, TContext, TContextUpdater, TEntity>> stateInfos,
                        StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State fromState,
                        Func<TContext, bool> predicate)
                    {
                        _stateInfos = stateInfos;
                        _fromState = fromState;
                        _predicate = predicate;
                    }

                    public void ThenSetState(StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State state, IOnTransition<TContext> onTransition = null)
                    {
                        _stateInfos[_fromState].Transitions.Add(new Transition<TContextFactory, TContext, TContextUpdater, TEntity>(_predicate, state, onTransition));
                    }
                }
            }
        }
        
        public interface IOnTransition<TContext>
        {
            void Transit(TContext context);
        }
    }
}