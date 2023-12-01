using UnityEngine;

namespace GameObjects.Enemy1
{
    public class DragonCollisionHandler : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(Constants.LanternTag))
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}