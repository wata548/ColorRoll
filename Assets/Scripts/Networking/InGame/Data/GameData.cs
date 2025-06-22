using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public override string ToString() =>
            $"{X}|{Y}|{Z}";

        public JsonVector3(string s) {
            var temp = s.Split('|');
            X = Convert.ToSingle(temp[0]);
            Y = Convert.ToSingle(temp[1]);
            Z = Convert.ToSingle(temp[2]);
        }
        
        public JsonVector3(float x, float y, float z) =>
            (X, Y, Z) = (x, y, z);
        
        public JsonVector3(Vector3 v) =>
            (X, Y, Z) = (v.x, v.y, v.z);
    }

    public class JsonQuaternion {
        public float X, Y, Z, W;

        public static explicit operator Quaternion(JsonQuaternion target) =>
            new(target.X, target.Y, target.Z, target.W);

        public override string ToString() =>
            $"{X}|{Y}|{Z}|{W}";

        public JsonQuaternion(string s) {
            var temp = s.Split('|');
            X = Convert.ToSingle(temp[0]);
            Y = Convert.ToSingle(temp[1]);
            Z = Convert.ToSingle(temp[2]);
            W = Convert.ToSingle(temp[3]);
        }
        
        public JsonQuaternion(float x, float y, float z, float w) =>
            (X, Y, Z, W) = (x, y, z, w);
        public JsonQuaternion(Quaternion q) =>
            (X, Y, Z, W) = (q.x, q.y, q.z, q.w);
    }

    public class GameData: IData {
        public JsonVector3 HostPosition;
        public JsonVector3 HostVelocity;
        //use bit mask
        public int HostChargeLevel;
        public bool HostBreak;
        
        public JsonVector3 ClientPosition;
        public JsonVector3 ClientVelocity;
        //use bit mask
        public int ClientChargeLevel;
        public bool ClientBreak;
        
        //blockInfo
        public List<JsonVector3> BreakBlocks;

        public override string ToString() {
            var builder = new StringBuilder();
            builder.AppendLine(HostPosition.ToString());
            builder.AppendLine(HostVelocity.ToString());
            builder.AppendLine(HostChargeLevel.ToString());
            builder.AppendLine(HostBreak.ToString());
            
            builder.AppendLine(ClientPosition.ToString());
            builder.AppendLine(ClientVelocity.ToString());
            builder.AppendLine(ClientChargeLevel.ToString());
            builder.AppendLine(ClientBreak.ToString());

            foreach (var line in BreakBlocks) {
                builder.AppendLine(line.ToString());
            }

            return builder.ToString();
        }

        public GameData(string data) {
            var temp = data.Split('\n');
            HostPosition = new(temp[0]);
            HostVelocity = new(temp[1]);
            HostChargeLevel = Convert.ToInt32(temp[2]);
            HostBreak = Convert.ToBoolean(temp[3]);
            
            ClientPosition = new(temp[4]);
            ClientVelocity = new(temp[5]);
            ClientChargeLevel = Convert.ToInt32(temp[6]);
            ClientBreak = Convert.ToBoolean(temp[7]);
            BreakBlocks = new();
            for (int i = 8; i < temp.Length - 1; i++) {
                BreakBlocks.Add(new(temp[i]));
            }
        }
        
        
        public GameData(PlayerFSM host, PlayerFSM client) {

            HostPosition = new(host.transform.position);
            HostVelocity = new(host.Rigid.linearVelocity);
            HostChargeLevel = (host.StateMatch[PlayerState.Charging] as ChargingState)!.ChargeLevel;
            HostBreak = host.CurrentState == PlayerState.Broken;
            
            ClientPosition = new(client.transform.position);
            ClientVelocity = new(client.Rigid.linearVelocity);
            ClientChargeLevel = (client.StateMatch[PlayerState.Charging] as ChargingState)!.ChargeLevel;
            ClientBreak = client.CurrentState == PlayerState.Broken;
            
            BreakBlocks = BlockGenerator.Instance.Break
                .Select(v => new JsonVector3(v))
                .ToList();
        }
    }
}