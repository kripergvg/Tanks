namespace Tanks.FSM
{
    public interface IStateMachineTyped<TContextFactory, TContext, TContextUpdater, TEntity> : IStateMachine
        where TContextFactory : IContextFactory<TContext>
        where TContextUpdater : IContextUpdater<TContext>
    {
        void Start(StateMachine<TContextFactory, TContext, TContextUpdater, TEntity>.State state);

        void OnEntityTriggerEnter(TEntity entity);

        void OnEntityTriggerExit(TEntity entity);
    }
}