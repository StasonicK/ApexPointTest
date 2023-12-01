using UnityEngine;
using UnityEngine.InputSystem;

namespace GameObjects.Tank
{
    public class TankShooting : MonoBehaviour
    {
        private bool _isEnabled;
        private TankInput _tankInput;

        public void Construct(Vector3 startPosition)
        {
            _tankInput = new TankInput();
        }

        public void On()
        {
            if (_tankInput != null)
            {
                _tankInput.Enable();
                _tankInput.Tank.Move.performed += Shoot;
            }

            _isEnabled = true;
        }


        public void Off()
        {
            if (_tankInput != null)
            {
                _tankInput.Disable();
                _tankInput.Tank.Shoot.performed -= Shoot;
            }

            _isEnabled = false;
        }

        private void Shoot(InputAction.CallbackContext obj)
        {
            // TODO
        }
    }
}