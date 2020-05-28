namespace Tanks.Mobs.Brain.FSMBrain
{
    public interface ITargetLocator
    {
        void TryAddTarget(IEntity entity);
        
        IEntity GetTarget();
    }
}