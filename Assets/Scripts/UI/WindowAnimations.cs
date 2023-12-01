using System;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class WindowAnimations : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public event Action Disabled;

        public void Show() =>
            _animator.Play(Constants.AnimationWindowShow);

        public void Hide()
        {
            _animator.Play(Constants.AnimationWindowHide);
            StartCoroutine(Disable());
        }

        private IEnumerator Disable()
        {
            yield return new WaitForSeconds(Constants.OpenCloseAnimationTime);
            Disabled?.Invoke();
            gameObject.SetActive(false);
        }
    }
}