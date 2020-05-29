namespace Tanks.Mobs.Brain.FSMBrain
{
    public class TargetChaser
    {
        private readonly IMover _mover;

        private IEntity _targetEntity;
        private bool _targetReached;

        public TargetChaser(IMover mover)
        {
            _mover = mover;
        }

        public bool TargetReached => _targetReached;

        public IEntity Target => _targetEntity;

        public void SetTarget(IEntity entity)
        {
            _targetEntity = entity;
            StartChase();
        }

        public void OnEntityTriggerStay(IEntity entity)
        {
            CheckTrigger(entity);
        }

        public void OnEntityTriggerEnter(IEntity entity)
        {
            CheckTrigger(entity);
        }

        public void OnEntityTriggerExit(IEntity entity)
        {
            if (_targetEntity == entity && _targetReached)
            {
                _targetReached = false;
                _targetEntity = null;
            }
        }

        private void CheckTrigger(IEntity entity)
        {
            if (_targetEntity == entity && !_targetReached)
            {
                _targetReached = true;
                StopChase();
            }
        }

        private void StartChase()
        {
            _mover.MoveToPoint(_targetEntity.Position);
        }

        private void StopChase()
        {
            _mover.StopMoving();
        }
    }
}