using System.Collections;
using UnityEngine;

namespace GameObjects.Tank
{
    public class TankVictoryVfx : MonoBehaviour
    {
        [SerializeField] private GameObject _vfx;

        private float _vfxTime = 2.0f;

        private void OnEnable()
        {
            if (_vfx != null)
                _vfx.SetActive(false);
        }

        public void Show()
        {
            if (_vfx != null)
                _vfx.SetActive(true);
            StartCoroutine(Disable());
        }

        private IEnumerator Disable()
        {
            yield return new WaitForSeconds(_vfxTime);

            if (_vfx != null)
                _vfx.SetActive(false);
        }
    }
}