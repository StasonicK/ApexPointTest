using System.Collections;
using GameObjects.Projectiles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameObjects.Tank
{
    public class TankShooting : MonoBehaviour
    {
        [SerializeField] private BigGunType _bigGunProjectilePrefab;
        [SerializeField] private MachineGunType _machineGunProjectilePrefab;
        [SerializeField] private Transform _bigGunSpawnTransform;
        [SerializeField] private Transform _machineGunSpawnTransform;
        [SerializeField] private float _bigGunWeaponCooldown;
        [SerializeField] private float _machineGunWeaponCooldown;

        private TankInput _tankInput;
        private TankWeaponChanger _tankWeaponChanger;
        private BaseGunType _currentProjectilePrefab;
        private Transform _currentTransformPosition;
        private float _currentWeaponCooldown;
        private WaitForSeconds _coroutineBigGunLaunchCooldown;
        private WaitForSeconds _coroutineMachineGunLaunchCooldown;
        private WaitForSeconds _coroutineCurrentGunLaunchCooldown;
        private Coroutine _startCoroutine;


        public void Construct(TankWeaponChanger tankWeaponChanger
            // , float weapon1Cooldown, float weapon2Cooldown
        )
        {
            _currentWeaponCooldown = 0f;
            _coroutineBigGunLaunchCooldown = new WaitForSeconds(_bigGunWeaponCooldown);
            _coroutineMachineGunLaunchCooldown = new WaitForSeconds(_machineGunWeaponCooldown);
            // _weapon2Cooldown = weapon2Cooldown;
            // _weapon1Cooldown = weapon1Cooldown;
            _tankWeaponChanger = tankWeaponChanger;
            _tankWeaponChanger.GotWeapon1 -= SelectWeapon1;
            _tankWeaponChanger.GotWeapon2 -= SelectWeapon2;
            _tankWeaponChanger.GotWeapon1 += SelectWeapon1;
            _tankWeaponChanger.GotWeapon2 += SelectWeapon2;
            _tankInput = new TankInput();
            SelectWeapon1();
        }

        private void SelectWeapon1()
        {
            _currentProjectilePrefab = _bigGunProjectilePrefab;
            _currentTransformPosition = _bigGunSpawnTransform;
        }

        private void SelectWeapon2()
        {
            _currentProjectilePrefab = _machineGunProjectilePrefab;
            _currentTransformPosition = _machineGunSpawnTransform;
        }

        public void On()
        {
            if (_tankInput != null)
            {
                _tankInput.Enable();
                _tankInput.Tank.Shoot.performed += Shoot;
            }
        }


        public void Off()
        {
            if (_tankInput != null)
            {
                _tankInput.Disable();
                _tankInput.Tank.Shoot.performed -= Shoot;
            }
        }

        private void Shoot(InputAction.CallbackContext obj)
        {
            if (_currentWeaponCooldown <= 0)
            {
                Instantiate(_currentProjectilePrefab, _currentTransformPosition.position,
                    _currentTransformPosition.rotation);

                SetCoroutineCooldown();

                if (_startCoroutine != null)
                    StopCoroutine(_startCoroutine);

                _startCoroutine = StartCoroutine(CoroutineLaunchCooldown());
            }
        }

        private void SetCoroutineCooldown()
        {
            if (_currentProjectilePrefab == _bigGunProjectilePrefab)
                _coroutineCurrentGunLaunchCooldown = _coroutineBigGunLaunchCooldown;
            else
                _coroutineCurrentGunLaunchCooldown = _coroutineMachineGunLaunchCooldown;
        }

        private IEnumerator CoroutineLaunchCooldown()
        {
            yield return _coroutineCurrentGunLaunchCooldown;
        }
    }
}