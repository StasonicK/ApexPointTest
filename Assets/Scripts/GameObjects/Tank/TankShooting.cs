using System.Collections;
using GameObjects.Projectiles;
using Pool.Projectiles;
using Services.StaticData;
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

        private TankInput _tankInput;
        private TankWeaponChanger _tankWeaponChanger;
        private BaseGunType _currentProjectilePrefab;
        private Transform _currentTransformPosition;
        private float _currentWeaponCooldown;
        private WaitForSeconds _coroutineCurrentGunLaunchCooldown;
        private Coroutine _startCoroutine;
        private GameObject _projectile;
        private IStaticDataService _staticDataService;
        private WeaponTypeId _weaponTypeId;
        private float _cooldown;
        private IProjectilesGenerator _projectilesGenerator;

        public void Construct(TankWeaponChanger tankWeaponChanger, IStaticDataService staticDataService,
            IProjectilesGenerator projectilesGenerator)
        {
            _projectilesGenerator = projectilesGenerator;
            _staticDataService = staticDataService;
            _currentWeaponCooldown = 0f;
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
            _weaponTypeId = WeaponTypeId.BigGun;
        }

        private void SelectWeapon2()
        {
            _currentProjectilePrefab = _machineGunProjectilePrefab;
            _currentTransformPosition = _machineGunSpawnTransform;
            _weaponTypeId = WeaponTypeId.MachineGun;
        }

        public void On()
        {
            if (_tankInput != null)
            {
                _tankInput.Enable();
                _tankInput.Tank.Shoot.started += Shoot;
            }
        }


        public void Off()
        {
            if (_tankInput != null)
            {
                _tankInput.Disable();
                _tankInput.Tank.Shoot.started -= Shoot;
            }
        }

        private void Shoot(InputAction.CallbackContext obj)
        {
            if (_currentWeaponCooldown <= 0)
            {
                _projectilesGenerator.GetProjectile(_weaponTypeId, _currentTransformPosition);
                // Instantiate(_currentProjectilePrefab, _currentTransformPosition.position,
                //         _currentTransformPosition.rotation)
                //     .gameObject.GetComponent<ProjectileCollisionHandler>()
                //     .Construct(_staticDataService.ForWeapon(_weaponTypeId).Damage);
                // _projectile = 
                //     Instantiate(_currentProjectilePrefab, _currentTransformPosition.position,
                //     _currentTransformPosition.rotation).gameObject;
                // _projectile.GetComponent<ProjectileCollisionHandler>()
                //     .Construct(_staticDataService.ForWeapon(_weaponTypeId).Damage);

                SetCoroutineCooldown();

                if (_startCoroutine != null)
                    StopCoroutine(_startCoroutine);

                _startCoroutine = StartCoroutine(CoroutineLaunchCooldown());
            }
        }

        private void SetCoroutineCooldown()
        {
            _cooldown = _staticDataService.ForWeapon(_weaponTypeId).Cooldown;
            _coroutineCurrentGunLaunchCooldown = new WaitForSeconds(_cooldown);
        }

        private IEnumerator CoroutineLaunchCooldown()
        {
            yield return _coroutineCurrentGunLaunchCooldown;
        }
    }
}