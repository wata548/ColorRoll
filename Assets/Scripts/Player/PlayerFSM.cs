using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Networking;
using PlayerStateBase = Player.StateBase<Player.PlayerState, Player.PlayerFSM>;

namespace Player {
    public class PlayerFSM: MonoBehaviour {

       //==================================================||Constant 
        private static readonly ReadOnlyDictionary<PlayerState, PlayerStateBase> StateMath = new(
            new Dictionary<PlayerState, PlayerStateBase>() {
                { PlayerState.Working,  new WorkingStateBase(PlayerState.Working)},
                { PlayerState.Charging, new ChargingStateBase(PlayerState.Charging)},
            }
        );
        
        //==================================================||Properties            
        public static implicit operator FSM<PlayerState, PlayerFSM>(PlayerFSM fsm) =>
            fsm.Fsm;
        public FSM<PlayerState, PlayerFSM> Fsm { get; private set; }
        public InputData Data { get; private set; }
        public Rigidbody Rigid { get; private set; }

        //==================================================||Methods 
        
        public void SetInputData(InputData data) =>
            Data = data;
        
        public void Change(PlayerState state) =>
            Fsm.Change(StateMath[state], state, this);

        private void Awake() {
            Rigid = GetComponent<Rigidbody>();
        }
    }
}