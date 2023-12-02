using System.Collections;
using UnityEngine;

namespace GameObjects.Tank
{
    public class TankHitVfx : MonoBehaviour
    {
        [SerializeField] private GameObject _hitVfx;

        private float _hitLifeTime = 2.5f;
        private Coroutine _coroutine;

        private void Awake()
        {
            if (_hitVfx != null)
                _hitVfx.SetActive(false);
        }

        public void Show()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            if (_hitVfx != null)
                _hitVfx.SetActive(true);

            _coroutine = StartCoroutine(Disable());
        }

        private IEnumerator Disable()
        {
            yield return new WaitForSeconds(_hitLifeTime);

            if (_hitVfx != null)
                _hitVfx.SetActive(false);
        }
    }
}