using UnityEngine;

namespace GameObjects.Enemies
{
    public class EnemyCollisionHandler : MonoBehaviour
    {
        private const string TankTag = "Tank";

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(TankTag))
            {
                // TODO tank gets hit
                other.gameObject.SetActive(false);
            }
        }
    }
}