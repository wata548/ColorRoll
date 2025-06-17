using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InputSystem;
using Networking;
using UnityEngine;
using UnityEngine.InputSystem;
using InputControl = InputSystem.InputControl;

namespace Player {
    public abstract class StateBase<STATE, MACHINE> {

        public readonly STATE Key;

        public StateBase(STATE key) =>
            Key = key;
        
        public abstract void Enter(STATE previousState, MACHINE machine);
        public abstract void Exit(STATE nextState, MACHINE machine);
        public abstract void Update(MACHINE machine);
    }
}