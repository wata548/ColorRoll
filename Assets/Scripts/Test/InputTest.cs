using System;
using Extensions;
using Player;
using UnityEngine;
using InputSystem;
using Networking;

namespace Test {
    public class InputTest: MonoBehaviour {

        private Rigidbody _rigidbody;
        private PlayerState _state;
        private InputControl _input;
        private PlayerStateBase _move;

        private void Awake() {
            
            _rigidbody = GetComponent<Rigidbody>();
            _input = new("DefaultInput.csv");
            _state = PlayerState.Working;
            _move = PlayerStateBase.MakeByState(_state, _rigidbody);
       }

        private void Update() {
            CameraFocus.Update(gameObject);
            var inputData = new InputData(CameraFocus.CurRotation, _input);
            _move = _move.Update(inputData, ref _state);
        }
    }
}