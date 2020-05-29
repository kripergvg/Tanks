namespace Tanks.Mobs.Brain.FSMBrain
{
    public interface ITargetLocator
    {
        void TryAddTarget(IEntity entity);

        void TryRemoveTarget(IEntity entity);
        
        IEntity GetTarget();
    }
}