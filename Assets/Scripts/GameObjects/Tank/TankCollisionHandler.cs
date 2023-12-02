using UnityEngine;

namespace GameObjects.Tank
{
    public class TankCollisionHandler : MonoBehaviour
    {
        // private PlayerRabbitCounter _playerRabbitCounter;
        // private PlayerWithRabbit _playerWithRabbit;

        private void Awake()
        {
            // _playerRabbitCounter = GetComponent<PlayerRabbitCounter>();
            // _playerWithRabbit = GetComponent<PlayerWithRabbit>();
        }

        private void OnCollisionEnter(Collision other)
        {
            // if (other.gameObject.CompareTag(Constants.RabbitTag))
            // {
            //     if (!_playerWithRabbit.RabbitGrabbed)
            //     {
            //         _playerWithRabbit.Show();
            //         other.gameObject.SetActive(false);
            //     }
            // }
        }

        private void OnTriggerEnter(Collider other)
        {
            // if (other.gameObject.CompareTag(Constants.HomeEntranceTag))
            // {
            //     if (_playerWithRabbit.RabbitGrabbed)
            //     {
            //         AudioController.Instance.PlaySong(AudioSong.SuccessSoundFx, false, AudioLayers.Sound);
            //         _playerRabbitCounter.Increase();
            //         _playerWithRabbit.Hide();
            //     }
            // }
        }
    }
}