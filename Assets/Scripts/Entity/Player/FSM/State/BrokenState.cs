using RayFire;
using UnityEngine;
using FSMBase;

namespace Player {
    public class BrokenState: StateBase<PlayerState, PlayerFSM> {

        private const float BreakTime = 2;
        private float _enterTime = 0;
        
        public BrokenState(PlayerState key) : base(key) { }
        
        public override void Enter(PlayerState previousState, PlayerFSM machine) {

            _enterTime = Time.time;
            
            machine.Break();
        }

        public override void Exit(PlayerState nextState, PlayerFSM machine) {
            machine.Restore();
        }

        public override void Update(PlayerFSM machine) {
            
            machine.Rigid.linearVelocity = Vector3.zero;
            
            if (Time.time - _enterTime >= BreakTime)
                machine.Change(PlayerState.Working);
        }
    }
}