using System;

namespace Tanks.FSM
{
    public readonly struct Transition<TContextFactory, TContext, TContextUpdater, TEntity>
        where TContextFactory :  IContextFactory<TContext>
        where TContextUpdater: IContextUpdater<TContext>
    {
        private readonly Func<TContext, bool> _predicate;
        private readonly StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State _destinationState;
        private readonly StateMachineFactory.IOnTransition<TContext> _onOnTransition;

        public Transition(Func<TContext, bool> predicate,
            StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State destinationState,
            StateMachineFactory.IOnTransition<TContext> onOnTransition)
        {
            _predicate = predicate;
            _destinationState = destinationState;
            _onOnTransition = onOnTransition;
        }

        public bool ShouldTransit(in TContext context)
        {
            return _predicate(context);
        }

        public StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State Transit(in TContext context)
        {
            _onOnTransition?.Transit(context);
            return _destinationState;
        }
    }
}