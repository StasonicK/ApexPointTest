using GameObjects.Enemies;
using Pool.Enemies;
using UnityEngine;

namespace GameObjects.Projectiles
{
    public class ProjectileCollisionHandler : MonoBehaviour
    {
        private float _damage;
        private IEnemiesGenerator _enemiesGenerator;
        private const string WallTag = "Wall";
        private const string ObstacleTag = "Obstacle";
        private const string ProjectileTag = "Projectile";
        private const string EnemyTag = "Enemy";

        public void Construct(float damage, IEnemiesGenerator enemiesGenerator)
        {
            _enemiesGenerator = enemiesGenerator;
            _damage = damage;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(WallTag) ||
                other.gameObject.CompareTag(ObstacleTag) ||
                other.gameObject.CompareTag(ProjectileTag))
            {
                other.gameObject.SetActive(false);
            }
            else if (other.gameObject.CompareTag(EnemyTag))
            {
                other.gameObject.GetComponent<EnemyHealth>().GetHit(_damage);
                _enemiesGenerator.ReduceActiveEnemies();
                other.gameObject.SetActive(false);
            }
        }
    }
}