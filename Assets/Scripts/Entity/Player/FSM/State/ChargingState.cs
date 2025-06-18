using System.Collections.Generic;
using UnityEngine;
using InputSystem;
using FSMBase;

namespace Player {
    public class ChargingState: StateBase<PlayerState, PlayerFSM> {
        
        //==================================================||Constant
        private static readonly float[] LevelTime = new float[]{0, 1f, 2.8f};
        private static readonly float[] ChargeDamage = new float[]{1, 5f, 12f};

        //==================================================||Fields
        private float _startTime = 0;
        private List<ParticleSystem> _particles = null;
        
       //==================================================||Properties 
        public int ChargeLevel { get; private set; }= -1;

        //==================================================||Constructors 
        public ChargingState(PlayerState key) : base(key) {}
        
        //==================================================||Methods 
        public override void Enter(PlayerState previousState, PlayerFSM machine) {
            _startTime = Time.time;
            ChargeLevel = -1;

            if (_particles == null) {
                _particles = new();
                _particles.Add(machine.Charge1);
                _particles.Add(machine.Charge2);
                _particles.Add(machine.Charge3);
            }
        }

        public override void Exit(PlayerState nextState, PlayerFSM machine) { }

        public override void Update(PlayerFSM machine) {
            if ((machine.Data.GetKey & Actions.Charge) == Actions.None) {
                
                foreach (var particle in _particles) 
                    particle.Stop();
                machine.Change(PlayerState.Shooting);
                return;
            }

            machine.Charge1.gameObject.transform.position = machine.transform.position;
            machine.Charge2.gameObject.transform.position = machine.transform.position;
            machine.Charge3.gameObject.transform.position = machine.transform.position;

            float progress = Time.time - _startTime;
            if (ChargeLevel + 1 < LevelTime.Length && progress >= LevelTime[ChargeLevel + 1]) {
                if (ChargeLevel >= 0 && ChargeLevel < LevelTime.Length) {
                    _particles[ChargeLevel].Stop();
                }
                
                ChargeLevel++;
                if (ChargeLevel >= 0 && ChargeLevel < LevelTime.Length) {
                    _particles[ChargeLevel].Play();
                }
            }
            
            machine.Rigid.linearVelocity = Vector3.zero;
        }
    }
}