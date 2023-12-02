using UnityEngine;

namespace GameObjects.Projectiles
{
    public class ProjectileCollisionHandler : MonoBehaviour
    {
        private const string WallTag = "Wall";
        private const string ObstacleTag = "Obstacle";
        private const string ProjectileTag = "Projectile";
        private const string EnemyTag = "Enemy";

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(WallTag) ||
                other.gameObject.CompareTag(ObstacleTag) ||
                other.gameObject.CompareTag(ProjectileTag)
               )
            {
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag(EnemyTag))
            {
                Destroy(gameObject);
            }
        }
    }
}