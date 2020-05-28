namespace Tanks.FSM
{
    public interface IContextUpdater<TContext>
    {
        void Update(ref TContext context);
    }
}