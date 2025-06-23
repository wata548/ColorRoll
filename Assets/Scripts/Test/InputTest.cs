using Player;
using UnityEngine;
using InputSystem;
using MapInfo;
using TMPro;
using Networking.InGame;

namespace Test {
    
    [RequireComponent(typeof(PlayerFSM))]
    [RequireComponent(typeof(Rigidbody))]
    public class InputTest: MonoBehaviour {

        private Rigidbody _rigidbody;
        private InputControl _input;
        private PlayerFSM _fsm;
        private Vector3 _prevPos = Vector3.zero;

        [SerializeField] private GameObject _temp;
        
        public int ChargeLevel { get; private set; }
        public float Movement { get; private set; } = 0;
        
        private void Start() {
            
            _rigidbody = GetComponent<Rigidbody>();
            _fsm = GetComponent<PlayerFSM>();
            _fsm.Change(PlayerState.Working);
            _input = new("DefaultInput.csv");
            _prevPos = transform.position;
            BlockGenerator.Instance.SetUp();
            FloorDrawer.Instance.SetTarget(gameObject, _temp);
        }

        private void Update() {

            ChargeLevel = (_fsm.StateMatch[PlayerState.Charging] as ChargingState)!.ChargeLevel;
            
            Movement += (transform.position - _prevPos).magnitude;
            _prevPos = transform.position;
            
            CameraFocus.Update(gameObject);
            var inputData = new InputData(CameraFocus.CurRotation, _input);
            _fsm.SetInputData(inputData);
        }
    }
}