using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using escapefromtampere.Manager;


namespace escapefromtampere.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _playerRigidBody;

        private InputManager _inputManager;

        private Animator _animator;

        private bool _hasAnimator;

        private int _xVelHash;

        private int _yVelHash;

        private const float _walkSpeed = 2f;
        private const float _runSpeed = 6f;

        private Vector2 _currentVelocity;

        private void Start()
        {
            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _playerRigidBody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();

            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (!_hasAnimator) return;

            float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
            if(_inputManager.Move == Vector2.zero) targetSpeed = 0.1f;

            _currentVelocity.x = targetSpeed * _inputManager.Move.x;
            _currentVelocity.y = targetSpeed * _inputManager.Move.y;

            var xVelDifference = _currentVelocity.x - _playerRigidBody.velocity.x;
            var zVelDifference = _currentVelocity.y - _playerRigidBody.velocity.z;


            _playerRigidBody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);
            
            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
        }


    }


}



