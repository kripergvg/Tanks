using UnityEngine;
using Object = UnityEngine.Object;

namespace Tanks
{
    public class TankModel
    {
        public int Health { get; set; }
    }

    public class TankFactory
    {
        private readonly GameObject _tankPrefab;

        public TankFactory(GameObject tankPrefab)
        {
            _tankPrefab = tankPrefab;
        }

        public Instance<TankView, TankModel, TankController> Create()
        {
            var instance = Object.Instantiate(_tankPrefab);
            var view = instance.GetComponent<TankView>();
            var model = new TankModel();
            var controller = new TankController(model, view);
            return new Instance<TankView, TankModel, TankController>(view, model, controller);
        }
    
        public readonly struct Instance<TView,TModel,TController>
        {
            public Instance(TView view, TModel model, TController controller)
            {
                View = view;
                Model = model;
                Controller = controller;
            }

            public TView View { get; }
            public TModel Model { get; }
            public TController Controller { get; }
        }
    }

    public class TankController
    {
        public TankController(TankModel model, TankView view)
        {
            view.OnFire += OnFire;
        }

        private void OnFire()
        {
        
        }
    }
}
