using System;
using Extensions;
using Player;
using UnityEngine;
using InputSystem;

namespace Test {
    public class InputTest: MonoBehaviour {

        private Rigidbody _rigidbody;
        private PlayerState _satate;
        private InputControl _input;
        private PlayerStateBase _move;

        private void Awake() {
            _input = new("DefaultInput.csv");
            foreach (Actions action in Enum.GetValues(typeof(Actions))) {
                if (ExEnum.IsFlag(action))
                    Debug.Log($"{action}: {_input[action]}");
            }
        }
    }
}