namespace Tanks.Mobs
{
    public class Zombie : Entity
    {
        public override EntityType EntityType { get; } = EntityType.Zombie;
        
        public FaceCamera Ui;
        
        public void Init(MainCameraStorage mainCameraStorage)
        {
            Ui.Init(mainCameraStorage);
        }
    }
}