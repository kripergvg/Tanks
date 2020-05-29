using Tanks.Mobs;

namespace Tanks.Entities
{
    public interface IEntityFactory<out T>
        where T : IEntity
    {
        T Create(in EntityDeps deps);
    }
}