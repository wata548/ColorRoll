using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InputSystem;
using Networking;
using UnityEngine;
using UnityEngine.InputSystem;
using InputControl = InputSystem.InputControl;

namespace Player {
    public abstract class PlayerStateBase {

        protected static ReadOnlyDictionary<PlayerState, Type> StateType = new(
            new Dictionary<PlayerState, Type>() {
                { PlayerState.Working , typeof(WorkingState)},
                { PlayerState.Charging , typeof(ChargingState)},
            } 
        );

        public static PlayerStateBase MakeByState(PlayerState state, Rigidbody rigid) =>
            MakeByState(state, state, rigid);
        public static PlayerStateBase MakeByState(PlayerState curState, PlayerState targetState, Rigidbody rigid) =>
            Activator.CreateInstance(StateType[targetState], rigid, curState) as PlayerStateBase;
        
        protected Rigidbody _rigid;

        protected PlayerStateBase(Rigidbody rigid, PlayerState prev) {
            _rigid = rigid;
            Enter(prev);
        }
        
        protected abstract void Enter(PlayerState previousState);
        protected abstract void Exit(PlayerState nextState);

        public abstract PlayerStateBase Update(InputData data, ref PlayerState state);
    }
}