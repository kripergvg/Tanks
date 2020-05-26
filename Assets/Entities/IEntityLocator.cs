using System.Collections.Generic;

namespace Tanks.Mobs
{
    public interface IEntityLocator
    {
        List<IEntity> GetEntities();
    }
}