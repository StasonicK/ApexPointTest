using Services;
using Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace UI.Screens.GameLoop
{
    public class MoneyCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyText;

        private IPlayerProgressService _playerProgressService;

        public void Construct()
        {
            _playerProgressService = AllServices.Container.Single<IPlayerProgressService>();

            ChangeMoneyCount();
        }


        private void ChangeMoneyCount() =>
            _moneyText.text = $"{_playerProgressService.GameData.MoneyCount} $";
    }
}