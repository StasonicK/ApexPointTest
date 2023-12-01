using System;
using Audio;
using Infrastructure;
using Infrastructure.States;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Windows
{
    public class FinishLevelWindow : MonoBehaviour
    {
        [FormerlySerializedAs("_button")] [SerializeField]
        private Button _nextLevelButton;

        [FormerlySerializedAs("_animator")] [SerializeField]
        private Animator _nextLevelAnimator;

        // [SerializeField] private TextMeshProUGUI _lanternsCountText;

        private const string LanternCountStart = "Collected";

        private IGameStateMachine _gameStateMachine;
        private SceneId _nextSceneId;
        private int _victoryCount;

        public event Action NextLevel;

        public void Construct(SceneId nextSceneId, int victoryCount)
        {
            _victoryCount = victoryCount;
            _nextSceneId = nextSceneId;
            // SetLanternsCountText(_victoryCount);
        }

        // private void SetLanternsCountText(int lanternsCount) =>
        //     _lanternsCountText.text = $"{LanternCountStart} {lanternsCount}/{lanternsCount}";

        public void Open()
        {
            gameObject.SetActive(true);
            _nextLevelAnimator.Play(Constants.AnimationButtonRotation);
            _nextLevelButton.onClick.AddListener(OnButtonClick);
            AudioManager.Instance.PlayAudio(AudioTrack.VictorySoundFx, false, AudioLayer.Sound);
        }

        public void Close()
        {
            _nextLevelButton.onClick.RemoveListener(OnButtonClick);
            gameObject.SetActive(false);
        }

        private void OnButtonClick()
        {
            if (_nextSceneId != SceneId.None)
            {
                AudioManager.Instance.PlayAudio(AudioTrack.ClickSoundFx, false, AudioLayer.Sound);
                NextLevel?.Invoke();
                _gameStateMachine.Enter<LoadSceneState, SceneId>(_nextSceneId);
            }
        }
    }
}