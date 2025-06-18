using RayFire;
using UnityEngine;
using FSMBase;

namespace Player {
    public class BrokenState: StateBase<PlayerState, PlayerFSM> {

        private const float BreakTime = 2;
        private float _enterTime = 0;
        
        public BrokenState(PlayerState key) : base(key) { }
        
        public override void Enter(PlayerState previousState, PlayerFSM machine) {

            machine.Rigid.useGravity = false;
            machine.Collider.isTrigger = true;
            
            _enterTime = Time.time;
            
            machine.Renderer.enabled = false;
            GameObject.Instantiate(machine.Halucination, machine.transform.position, Quaternion.identity)
                .GetComponent<RayfireRigid>()
                .Demolish();
        }

        public override void Exit(PlayerState nextState, PlayerFSM machine) {
            machine.Rigid.useGravity = true;
            machine.Collider.isTrigger = false;
            machine.Renderer.enabled = true;
        }

        public override void Update(PlayerFSM machine) {
            
            machine.Rigid.linearVelocity = Vector3.zero;
            
            if (Time.time - _enterTime >= BreakTime)
                machine.Change(PlayerState.Working);
        }
    }
}