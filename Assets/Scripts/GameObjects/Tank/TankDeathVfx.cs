using UnityEngine;

namespace GameObjects.Tank
{
    public class TankDeathVfx : MonoBehaviour
    {
        [SerializeField] private GameObject _deathVfx;

        private void Awake()
        {
            if (_deathVfx != null)
                _deathVfx.SetActive(false);
        }

        public void Show()
        {
            if (_deathVfx != null)
                _deathVfx.SetActive(true);
        }
    }
}