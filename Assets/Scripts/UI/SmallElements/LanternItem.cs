using UnityEngine;

namespace UI.Level.GameUI
{
    // [RequireComponent(typeof(Animator))]
    public class LanternItem : MonoBehaviour
    {
        [SerializeField] private GameObject _empty;
        [SerializeField] private GameObject _filled;
        [SerializeField] private Animator _animator;

        private bool _isFilled;

        public bool IsFilled => _isFilled;

        private void Awake() =>
            SetEmpty();

        public void SetEmpty()
        {
            _isFilled = false;
            _empty.SetActive(true);
            _filled.SetActive(false);
        }

        public void Fill()
        {
            _filled.SetActive(true);
            _isFilled = true;
            _animator.Play(Constants.AnimationLanternShow);
        }
    }
}