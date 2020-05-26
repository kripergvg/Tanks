using UnityEngine;

namespace Tanks.Mobs
{
    public class MainCameraStorage
    {
        private Camera _camera;

        public void Set(Camera camera)
        {
            _camera = camera;
        }

        public bool Exists()
        {
            return _camera != null;
        }

        public Camera Get()
        {
            return _camera;
        }
    }
}