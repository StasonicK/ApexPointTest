using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameObjects.Tank
{
    public class TankWeaponChanger : MonoBehaviour
    {
        [SerializeField] private GameObject _weapon1;
        [SerializeField] private GameObject _weapon2;

        private TankInput _tankInput;

        public WeaponTypeId CurrentWeaponTypeId { get; private set; }

        public event Action GotWeapon1;
        public event Action GotWeapon2;

        public void Construct()
        {
            _tankInput = new TankInput();
            SetWeapon1();
        }

        private void SetWeapon1()
        {
            _weapon1.SetActive(true);
            _weapon2.SetActive(false);
            GotWeapon1?.Invoke();
        }

        private void SetWeapon2()
        {
            _weapon2.SetActive(true);
            _weapon1.SetActive(false);
            GotWeapon2?.Invoke();
        }

        public void On()
        {
            if (_tankInput != null)
            {
                _tankInput.Enable();
                _tankInput.Tank.ChangeWeapon1.performed += SetWeapon1;
                _tankInput.Tank.ChangeWeapon2.performed += SetWeapon2;
            }
        }

        private void SetWeapon1(InputAction.CallbackContext obj)
        {
            if (CurrentWeaponTypeId != WeaponTypeId.BigGun)
            {
                CurrentWeaponTypeId = WeaponTypeId.BigGun;
                SetWeapon1();
            }
        }

        private void SetWeapon2(InputAction.CallbackContext obj)
        {
            if (CurrentWeaponTypeId != WeaponTypeId.MachineGun)
            {
                CurrentWeaponTypeId = WeaponTypeId.MachineGun;
                SetWeapon2();
            }
        }

        public void Off()
        {
            if (_tankInput != null)
            {
                _tankInput.Disable();
                _tankInput.Tank.ChangeWeapon1.performed -= SetWeapon1;
                _tankInput.Tank.ChangeWeapon2.performed -= SetWeapon2;
            }
        }
    }
}