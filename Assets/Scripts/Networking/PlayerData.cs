using System.Collections.Generic;
using UnityEngine;

namespace Networking {
    public class PlayerData {
        public readonly Vector3 HostPlayerPos;
        public readonly Vector3 HostPlayerVelo;
        public readonly Vector3 HostPlayerScale;
        public readonly Vector3 ClientPlayerPos;
        public readonly Vector3 ClientPlayerVelo;
        public readonly Vector3 ClientPlayerScale;
        public readonly List<Vector3Int> BreakList;

        public Vector3 Pos(bool isHost) => isHost
            ? HostPlayerPos
            : ClientPlayerPos;
            
        public Vector3 Scale(bool isHost) => isHost
            ? HostPlayerScale
            : ClientPlayerScale;
        
        public Vector3 Velocity(bool isHost) => isHost
            ? HostPlayerVelo
            : ClientPlayerVelo;
        
        public PlayerData(Transform host, Vector3 hostVelo, Transform client, Vector3 clientVelo, List<Vector3Int> breaks) {
            (HostPlayerPos, HostPlayerScale) = (host.position, host.localScale);
            (ClientPlayerPos, ClientPlayerScale) = (client.position, client.localScale);
            HostPlayerVelo = hostVelo;
            ClientPlayerVelo = clientVelo;
            BreakList = breaks;
        }
    }
}