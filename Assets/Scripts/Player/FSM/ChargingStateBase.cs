using System;
using System.Collections.Generic;
using UnityEngine;
using InputSystem;
using Networking;

namespace Player {
    public class ChargingStateBase: StateBase<PlayerState, PlayerFSM> {
        
       //==================================================||Constant
       private static readonly float[] LevelTime = new float[]{0, 1f, 2.8f};

       //==================================================||Fields
        private PlayerState _prev;
        private float _startTime = 0;
        private int _chargeLevel = -1;
        private List<ParticleSystem> _particles = null;

       //==================================================||Constructors 
        public ChargingStateBase(PlayerState key) : base(key) {}
        
       //==================================================||Methods 
        public override void Enter(PlayerState previousState, PlayerFSM machine) {
            _prev = previousState;
            _startTime = Time.time;
            _chargeLevel = -1;

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
                machine.Change(_prev);
                foreach (var particle in _particles) {
                    particle.Stop();
                }
            }

            machine.Charge1.gameObject.transform.position = machine.transform.position;
            machine.Charge2.gameObject.transform.position = machine.transform.position;
            machine.Charge3.gameObject.transform.position = machine.transform.position;

            float progress = Time.time - _startTime;
            if (_chargeLevel + 1 < LevelTime.Length && progress >= LevelTime[_chargeLevel + 1]) {
                if (_chargeLevel >= 0 && _chargeLevel < LevelTime.Length) {
                    _particles[_chargeLevel].Stop();
                }
                
                _chargeLevel++;
                if (_chargeLevel >= 0 && _chargeLevel < LevelTime.Length) {
                    _particles[_chargeLevel].Play();
                }
            }
            
            machine.Rigid.linearVelocity = Vector3.zero;
        }
    }
}