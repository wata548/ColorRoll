using System;
using RayFire;
using UnityEngine;

namespace Entitty {
    
    [RequireComponent(typeof(RayfireRigid))]
    public class Damageable: MonoBehaviour {
        public float MaxHp { get; private set; }
        public float CurHp { get; private set; }
        public bool IsDeath { get; private set; } = false;
        
        
        public void SetHp(float target) =>
            (MaxHp, CurHp) = (target, target);

        public void Damage(float damage) {

            if (damage < 0)
                throw new Exception("damage is positive value");
            if (IsDeath)
                return;
            
            CurHp -= damage;
            
            if (CurHp <= 0) {
                IsDeath = true;
                CurHp = 0;
                OnDeath();
            }
            else
                OnDamaged();
        }

        private RayfireRigid _rayfireRigid = null; 

        protected virtual void OnDamaged() { }

        protected virtual void OnDeath() {

            _rayfireRigid.Demolish();
        }

        private void Awake() {
            
            _rayfireRigid = GetComponent<RayfireRigid>();
        }
    }
}