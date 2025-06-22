using System;
using System.Collections.Generic;
using System.Linq;
using MapInfo;
using Player;
using UnityEngine;


namespace Networking.InGame {

    public interface IData {
    }

    public class JsonVector3 {
        public float X, Y, Z;

        public static explicit operator Vector3(JsonVector3 target) =>
            new(target.X, target.Y, target.Z);

        public JsonVector3(Vector3 v) =>
            (X, Y, Z) = (v.x, v.y, v.z);
    }

    public class JsonQuaternion {
        public float X, Y, Z, W;

        public static explicit operator Quaternion(JsonQuaternion target) =>
            new(target.X, target.Y, target.Z, target.W);

        public JsonQuaternion(Quaternion q) =>
            (X, Y, Z, W) = (q.x, q.y, q.z, q.w);
    }

public class GameData: IData {
        public readonly JsonVector3 HostPosition;
        public readonly JsonVector3 HostVelocity;
        //use bit mask
        public readonly int HostChargeLevel;
        public readonly bool HostBreak;
        
        public readonly JsonVector3 ClientPosition;
        public readonly JsonVector3 ClientVelocity;
        //use bit mask
        public readonly int ClientChargeLevel;
        public readonly bool ClientBreak;
        
        //blockInfo
        public readonly List<JsonVector3> BreakBlocks;
        public readonly MapState[,] Map;

        public GameData(PlayerFSM host, PlayerFSM client) {

            HostPosition = new(host.transform.position);
            HostVelocity = new(host.Rigid.linearVelocity);
            HostChargeLevel = (host.StateMatch[PlayerState.Charging] as ChargingState)!.ChargeLevel;
            HostBreak = host.CurrentState == PlayerState.Broken;
            
            ClientPosition = new(client.transform.position);
            ClientVelocity = new(client.Rigid.linearVelocity);
            ClientChargeLevel = (client.StateMatch[PlayerState.Charging] as ChargingState)!.ChargeLevel;
            ClientBreak = client.CurrentState == PlayerState.Broken;
            
            Map = BlockGenerator.Instance.Map;
            BreakBlocks = BlockGenerator.Instance.Break
                .Select(v => new JsonVector3(v))
                .ToList();
        }
    }
}