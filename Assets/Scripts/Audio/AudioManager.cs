using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private bool _debug = false;
        [SerializeField] private AudioClip _music;
        [SerializeField] private AudioClip _victorySoundFx;
        [SerializeField] private AudioClip _deathSoundFx;
        [SerializeField] private AudioClip _successSoundFx;
        [SerializeField] private AudioClip _errorSoundFx;
        [SerializeField] private AudioClip _clickSoundFx;
        [SerializeField] private AudioClip _swooshSoundFx;
        [SerializeField] private AudioClip _explosionSoundFx;
        [SerializeField] private AudioSource _backgroundAudio;
        [SerializeField] private AudioSource _soundFxAudio;

        private static AudioManager _instance;
        private float _volume;

        private void Awake() =>
            DontDestroyOnLoad(this);

        private void Start() =>
            PlayAudio(AudioTrack.Music, true, AudioLayer.Background);

        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<AudioManager>();

                return _instance;
            }
        }

        public void Mute()
        {
            _backgroundAudio.volume = Constants.Zero;
            _soundFxAudio.volume = Constants.Zero;
            _volume = Constants.Zero;
        }

        public void Unmute()
        {
            _backgroundAudio.volume = Constants.MaxValue;
            _soundFxAudio.volume = Constants.MaxValue;
            _volume = Constants.MaxValue;
        }

        public void PlayAudio(AudioTrack track, bool shouldLoop, AudioLayer layer)
        {
            switch (track)
            {
                case AudioTrack.Music:
                    DebugAudio("Play Audio File: " + _music);
                    AudioInnerManager(_music, shouldLoop, _volume, layer);
                    break;

                case AudioTrack.VictorySoundFx:
                    DebugAudio("Play Audio File: " + _victorySoundFx);
                    AudioInnerManager(_victorySoundFx, shouldLoop, _volume, layer);
                    break;

                case AudioTrack.ClickSoundFx:
                    DebugAudio("Play Audio File: " + _clickSoundFx);
                    AudioInnerManager(_clickSoundFx, shouldLoop, _volume, layer);
                    break;

                case AudioTrack.SwooshSoundFx:
                    DebugAudio("Play Audio File: " + _swooshSoundFx);
                    AudioInnerManager(_swooshSoundFx, shouldLoop, _volume, layer);
                    break;

                case AudioTrack.DeathSoundFx:
                    DebugAudio("Play Audio File: " + _deathSoundFx);
                    AudioInnerManager(_deathSoundFx, shouldLoop, _volume, layer);
                    break;

                case AudioTrack.SuccessSoundFx:
                    DebugAudio("Play Audio File: " + _successSoundFx);
                    AudioInnerManager(_successSoundFx, shouldLoop, _volume, layer);
                    break;

                case AudioTrack.ExplosionSoundFx:
                    DebugAudio("Play Audio File: " + _explosionSoundFx);
                    AudioInnerManager(_explosionSoundFx, shouldLoop, _volume, layer);
                    break;
            }
        }

        private void AudioInnerManager(AudioClip soundToPlay, bool loop, float audioLevel, AudioLayer layer)
        {
            switch (layer)
            {
                case AudioLayer.Background:
                    _backgroundAudio.clip = soundToPlay;
                    _backgroundAudio.Play();
                    _backgroundAudio.loop = loop;
                    _backgroundAudio.volume = audioLevel;
                    DebugAudio("Playing on " + layer);
                    break;

                case AudioLayer.Sound:
                    _soundFxAudio.clip = soundToPlay;
                    _soundFxAudio.Play();
                    _soundFxAudio.loop = loop;
                    _soundFxAudio.volume = audioLevel;
                    DebugAudio("Playing on " + layer);
                    break;
                default:
                    Debug.LogWarning("Audio Layer Does Not Exist");
                    break;
            }
        }

        private void DebugAudio(string log)
        {
            if (_debug)
                Debug.Log(log);
        }

        public void ChangeVolume(float value)
        {
            _backgroundAudio.volume = value;
            _soundFxAudio.volume = value;
        }
    }
}