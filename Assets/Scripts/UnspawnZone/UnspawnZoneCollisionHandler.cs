using UnityEngine;

namespace UnspawnZone
{
    public class UnspawnZoneCollisionHandler : MonoBehaviour
    {
        private void Awake() =>
            DontDestroyOnLoad(this);

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(Constants.LanternTag) ||
                other.gameObject.CompareTag(Constants.DragonTag) ||
                other.gameObject.CompareTag(Constants.ShurikenTag) ||
                other.gameObject.CompareTag(Constants.RocketTag))
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}