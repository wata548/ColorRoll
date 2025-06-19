using System.Collections.Generic;
using System.Collections.ObjectModel;
using Entitty;
using UnityEngine;
using Networking;
using FSMBase;
using RayFire;
using PlayerStateBase = FSMBase.StateBase<Player.PlayerState, Player.PlayerFSM>;

namespace Player {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(Collider))]
    public class PlayerFSM: MonoBehaviour {
        
        //==================================================||Properties            
        [field: SerializeField] public ParticleSystem Charge1 { get; private set; }
        [field: SerializeField] public ParticleSystem Charge2 { get; private set; }
        [field: SerializeField] public ParticleSystem Charge3 { get; private set; }
        
        [field: SerializeField] public RayfireRigid Halucination { get; private set; }
        public Collider Collider { get; private set; }
        public MeshRenderer Renderer { get; private set; } = null; 
        
        public FiniteStateMachine<PlayerState, PlayerFSM> Fsm { get; private set; } = new();
        public InputData Data { get; private set; } = null;
        public Rigidbody Rigid { get; private set; }
        public List<Damageable> Collision { get; private set; } = new();

        public PlayerState CurrentState => Fsm.CurrentState;
        
        //==================================================||Field
        private int _lastUpdateFrame = -1;
        private List<Damageable> _currentCollision = new();
        
        //==================================================||Constant 
        public readonly ReadOnlyDictionary<PlayerState, PlayerStateBase> StateMatch = new(
            new Dictionary<PlayerState, PlayerStateBase>() {
                { PlayerState.Working,  new WorkingState(PlayerState.Working)},
                { PlayerState.Charging, new ChargingState(PlayerState.Charging)},
                { PlayerState.Broken, new BrokenState(PlayerState.Broken)},
                { PlayerState.Shooting, new ShootingState(PlayerState.Shooting)},
            }
        );
        
        //==================================================||Methods 

        private void UpdateFsm() {

            if (Data == null)
                return;
            
            Fsm.Update(this);
            Data = null;
        }

        public void SetInputData(InputData data) {
            
            Data = data;
            UpdateFsm();
        }
        
        public void Change(PlayerState state) =>
            Fsm.Change(StateMatch[state], state, this);

        private void CollisionListUpdate() {
            if (_lastUpdateFrame != Time.frameCount) {
                
                _lastUpdateFrame = Time.frameCount;
                (Collision, _currentCollision) = (_currentCollision, Collision);
                _currentCollision.Clear();
            }
        }
        
        //==================================================||Unity 
        private void Awake() {
            Renderer = GetComponent<MeshRenderer>();
            Rigid = GetComponent<Rigidbody>();
            Collider = GetComponent<Collider>();
        }

        private void Update() {
            CollisionListUpdate();
        }

        private void OnCollisionEnter(Collision other) {

            if (!other.gameObject.TryGetComponent(typeof(Damageable), out var damageable)) 
                return;

            CollisionListUpdate();

            _currentCollision.Add(damageable as Damageable);
        }
    }
}