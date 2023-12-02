using GameObjects.Tank;
using UnityEngine;

namespace GameObjects.Enemies
{
    public class EnemyCollisionHandler : MonoBehaviour
    {
        private const string TankTag = "Tank";
        private const string GroundTag = "Ground";
        private float _damage;

        public void Construct(float damage) =>
            _damage = damage;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(TankTag))
                other.gameObject.GetComponent<TankHealth>().GetHit(_damage);
            // else if (other.gameObject.CompareTag(GroundTag))
            //     gameObject.GetComponent<EnemyMovement>().Run();
        }
    }
}