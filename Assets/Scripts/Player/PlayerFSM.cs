using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Networking;
using PlayerStateBase = Player.StateBase<Player.PlayerState, Player.PlayerFSM>;

namespace Player {
    public class PlayerFSM: MonoBehaviour {
        //==================================================||Field

        [field: SerializeField] public ParticleSystem Charge1 { get; private set; }
        [field: SerializeField] public ParticleSystem Charge2 { get; private set; }
        [field: SerializeField] public ParticleSystem Charge3 { get; private set; }
        
        //==================================================||Constant 
        private static readonly ReadOnlyDictionary<PlayerState, PlayerStateBase> StateMath = new(
            new Dictionary<PlayerState, PlayerStateBase>() {
                { PlayerState.Working,  new WorkingStateBase(PlayerState.Working)},
                { PlayerState.Charging, new ChargingStateBase(PlayerState.Charging)},
            }
        );
        
        //==================================================||Properties            
        public static implicit operator FiniteStateMachine<PlayerState, PlayerFSM>(PlayerFSM fsm) =>
            fsm.Fsm;

        public FiniteStateMachine<PlayerState, PlayerFSM> Fsm { get; private set; } = new();
        public InputData Data { get; private set; } = null;
        public Rigidbody Rigid { get; private set; }

        public PlayerState CurrentState => Fsm.CurrentState;
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
            Fsm.Change(StateMath[state], state, this);

        //==================================================||Unity 
        private void Awake() {
            Rigid = GetComponent<Rigidbody>();
        }
    }
}