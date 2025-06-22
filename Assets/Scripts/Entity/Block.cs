using System;
using MapInfo;
using RayFire;
using UnityEngine;

namespace Entitty {
    class Block: Damageable {
        protected override void OnDamaged() {
            base.OnDamaged();
        }

        protected override void OnDeath() {
            Debug.Log(transform.localPosition);
            BlockGenerator.Instance.BreakBlock(transform.position);
            base.OnDeath();
        }
    }
}