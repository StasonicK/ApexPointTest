using UnityEngine;

namespace GameObjects.Enemy2
{
    public class LanternCollisionHandler : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(Constants.LanternTag))
            {
            }
        }
    }
}