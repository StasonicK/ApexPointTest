using UnityEngine;

namespace GameObjects
{
    public class GameObjectMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private bool _isRun;
        protected int DirectionMultiplier;

        private void OnEnable() =>
            DirectionMultiplier = 1;

        private void Update()
        {
            if (_isRun)
                transform.Translate(0f, _speed * -DirectionMultiplier * Time.deltaTime, 0f);
        }

        public void Run() =>
            _isRun = true;

        public void Stop() =>
            _isRun = false;
    }
}