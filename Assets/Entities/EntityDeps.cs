namespace Tanks.Entities
{
    public readonly struct EntityDeps
    {
        public EntityDeps(EntitiesSpawner entitiesStorage, MainCameraStorage mainCameraStorage)
        {
            EntitiesStorage = entitiesStorage;
            MainCameraStorage = mainCameraStorage;
        }

        public EntitiesSpawner EntitiesStorage { get; }
        public MainCameraStorage MainCameraStorage { get; }
    }
}