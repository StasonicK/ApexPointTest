using UnityEngine;
using UnityEngine.InputSystem;

namespace GameObjects.Tank
{
    public class TankRotation : MonoBehaviour
    {
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private float _speed;

        private Vector3 _directionToLook;
        private Quaternion _targetRotation;
        private TankInput _tankInput;
        private bool _isEnabled;
        private Vector3 _rotateInput = Vector3.zero;

        private void Update()
        {
            if (_isEnabled)
                _bodyTransform.Rotate(_rotateInput * _speed * Time.deltaTime);
        }

        public void Construct()
        {
            _tankInput = new TankInput();
            ResetDirection();
        }

        public void ResetDirection() =>
            transform.Rotate(Vector3.forward);

        public void On()
        {
            if (_tankInput != null)
            {
                _tankInput.Enable();
                _tankInput.Tank.RotateHull.started += Rotate;
                _tankInput.Tank.RotateHull.canceled += Stop;
            }

            _isEnabled = true;
        }

        public void Off()
        {
            if (_tankInput != null)
            {
                _tankInput.Disable();
                _tankInput.Tank.RotateHull.started -= Rotate;
                _tankInput.Tank.RotateHull.canceled -= Stop;
            }

            _isEnabled = true;
        }

        private void Rotate(InputAction.CallbackContext ctx) =>
            _rotateInput = new Vector3(0f, ctx.ReadValue<Vector2>().x, 0f);

        private void Stop(InputAction.CallbackContext ctx) =>
            _rotateInput = Vector3.zero;
    }
}