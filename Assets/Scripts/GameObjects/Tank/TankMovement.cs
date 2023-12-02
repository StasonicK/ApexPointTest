using UnityEngine;
using UnityEngine.InputSystem;

namespace GameObjects.Tank
{
    public class TankMovement : MonoBehaviour
    {
        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private float _speed;

        private float _movementEpsilon = 0.05f;
        private CharacterController _characterController;
        private TankInput _tankInput;
        private Vector2 _moveInput = Vector2.zero;
        private Vector2 _currentMoveInput = Vector2.zero;
        private bool _isEnabled;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            DontDestroyOnLoad(this);
        }

        private void Start() =>
            ResetPosition();

        private void Update()
        {
            if (_isEnabled)
                transform.position += transform.forward * _moveInput.y * _speed * Time.deltaTime;
        }

        public void Construct(Vector3 startPosition)
        {
            _startPosition = startPosition;
            _tankInput = new TankInput();
        }

        public void ResetPosition() =>
            transform.position = _startPosition;

        public void On()
        {
            if (_tankInput != null)
            {
                _tankInput.Enable();
                _tankInput.Tank.Move.performed += Move;
                _tankInput.Tank.Move.canceled += Stop;
            }

            _isEnabled = true;
        }

        public void Off()
        {
            if (_tankInput != null)
            {
                _tankInput.Disable();
                _tankInput.Tank.Move.performed -= Move;
                _tankInput.Tank.Move.canceled -= Stop;
            }

            _isEnabled = false;
        }

        private void Stop(InputAction.CallbackContext obj) =>
            _moveInput = Vector2.zero;

        private void Move(InputAction.CallbackContext ctx)
        {
            _currentMoveInput = ctx.ReadValue<Vector2>();

            if (_moveInput != _currentMoveInput)
                _moveInput = _currentMoveInput;
        }
    }
}