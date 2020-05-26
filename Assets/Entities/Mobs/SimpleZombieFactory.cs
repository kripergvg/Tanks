using UnityEngine;

namespace Tanks.Mobs
{
    public class ZombieFactory : IEntityFactory<Zombie>
    {
        private readonly Zombie _prefab;

        public ZombieFactory(Zombie prefab)
        {
            _prefab = prefab;
        }
        
        public Zombie Create(in EntityDeps deps)
        {
            var zombie = Object.Instantiate(_prefab);
            zombie.Init(deps.MainCameraStorage);
            return zombie;
        }
    }
}