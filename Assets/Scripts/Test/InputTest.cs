using Player;
using UnityEngine;
using InputSystem;
using TMPro;
using Networking.InGame;

namespace Test {
    
    [RequireComponent(typeof(PlayerFSM))]
    [RequireComponent(typeof(Rigidbody))]
    public class InputTest: MonoBehaviour {

        [SerializeField] private TMP_Text _stateShower;
        
        private Rigidbody _rigidbody;
        private InputControl _input;
        private PlayerFSM _fsm;

        private void Awake() {
            
            _rigidbody = GetComponent<Rigidbody>();
            _fsm = GetComponent<PlayerFSM>();
            _fsm.Change(PlayerState.Working);
            _input = new("DefaultInput.csv");
        }

        private void Update() {
            
            CameraFocus.Update(gameObject);
            var inputData = new InputData(CameraFocus.CurRotation, _input);
            _fsm.SetInputData(inputData);
            _stateShower.text = _fsm.CurrentState.ToString();
        }
    }
}