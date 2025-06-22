using System.Collections.Generic;
using System.Collections.ObjectModel;
using Entitty;
using UnityEngine;
using Networking;
using FSMBase;
using Networking.InGame;
using RayFire;
using UnityEngine.Serialization;
using PlayerStateBase = FSMBase.StateBase<Player.PlayerState, Player.PlayerFSM>;

namespace Player {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(Collider))]
    public class PlayerFSM: MonoBehaviour {
        
        //==================================================||Properties            
        [field: SerializeField] public ParticleSystem[] ChargeParticles { get; private set; }

        public Collider Collider { get; private set; }
        public MeshRenderer Renderer { get; private set; } = null; 
        
        public FiniteStateMachine<PlayerState, PlayerFSM> Fsm { get; private set; } = new();
        public InputData Data { get; private set; } = null;
        public Rigidbody Rigid { get; private set; }
        public List<Damageable> Collision { get; private set; } = new();

        public PlayerState CurrentState => Fsm.CurrentState;
        
        //==================================================||Field
        [SerializeField] private RayfireRigid _breakPrefab;
        [SerializeField] private ParticleSystem _defaultParticle;
        
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

        public void Break() {
            Rigid.useGravity = false;
            Collider.isTrigger = true;
                        
            Renderer.enabled = false;
            Instantiate(_breakPrefab, transform.position, Quaternion.identity)
                .GetComponent<RayfireRigid>()
                .Demolish();
        }

        public void Restore() {
            Rigid.useGravity = true;
            Collider.isTrigger = false;
            Renderer.enabled = true;
        }
        
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
            var pos = transform.position;
            pos.y -= 0.1f;
            _defaultParticle.transform.position = pos;
            _defaultParticle.transform.rotation = Quaternion.Euler(-90,0,-90);
        }

        private void OnCollisionEnter(Collision other) {

            if (!other.gameObject.TryGetComponent(typeof(Damageable), out var damageable)) 
                return;

            CollisionListUpdate();

            _currentCollision.Add(damageable as Damageable);
        }
    }
}