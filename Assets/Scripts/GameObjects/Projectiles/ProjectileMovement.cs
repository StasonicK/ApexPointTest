using UnityEngine;

namespace GameObjects.Projectiles
{
    public class ProjectileMovement : MonoBehaviour, IGameObjectMovement
    {
        [SerializeField] private float _speed;

        public bool IsRun { get; private set; }

        private void OnEnable()
        {
            Run();
        }

        private void Update()
        {
            if (IsRun)
                transform.position += transform.forward * _speed * Time.deltaTime;
        }

        public void Run() =>
            IsRun = true;

        public void Stop() =>
            IsRun = false;
    }
}