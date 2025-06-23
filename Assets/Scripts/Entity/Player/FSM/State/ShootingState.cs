using Extensions;
using UnityEngine;
using FSMBase;
using Unity.Mathematics.Geometry;

namespace Player {
    public class ShootingState: StateBase<PlayerState, PlayerFSM> {
        
       //==================================================||Constant 
        
        private static readonly float[] Power = new[] { 2f,4.5f, 7f };
        private static readonly float[] ChargeDamage = new float[]{1, 5f, 12f};
        private const float ShootingTime = 1.5f;

       //==================================================||Fields 
        private int _curLevel = 0;
        private float _startTime = 0;
        private Vector3 _direction = Vector3.zero;

       //==================================================||Constructors 
        public ShootingState(PlayerState key) : base(key) { }

       //==================================================||Methods 
        public float Progress => (Time.time - _startTime) / ShootingTime;
        public float CalculatePower(float power) {
            var progress = Progress;
            return power * Mathf.Sqrt(1 - progress);
        } 
        
        public override void Enter(PlayerState previousState, PlayerFSM machine) {

            _startTime = Time.time;

            var chargingState = machine.StateMatch[PlayerState.Charging] as ChargingState;
            _curLevel = chargingState
                        ?.ChargeLevel 
                        ?? 0;
            _curLevel = Mathf.Max(0, _curLevel);
            chargingState.InitLevel();

            var realDirection = (Quaternion)machine.Data.ViewDirection;
            _direction = realDirection
                .YAxis()
                .Direction();
        }

        public override void Exit(PlayerState nextState, PlayerFSM machine) {

            foreach (var target in machine.Collision) {
                
                target.GiveDamage(CalculatePower(Power[_curLevel]));
            }
        }

        public override void Update(PlayerFSM machine) {

            foreach (var particle in machine.ChargeParticles) {
                particle.transform.localPosition = Vector3.zero;
                particle.transform.rotation = Quaternion.identity;
            }
            
            if (Progress >= 1) {
                
                machine.Change(PlayerState.Working);
                return;
            }
            
            machine.Rigid.linearVelocity = _direction * CalculatePower(Power[_curLevel]);
            
            if(machine.Collision.Count != 0) {
                
                machine.Rigid.linearVelocity  = Vector3.zero;  
                machine.Change(PlayerState.Working);
            }
        }
    }
}