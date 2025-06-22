using System;
using System.Collections.Generic;
using MapInfo;
using UnityEngine;


namespace Networking.InGame {
    
    public interface IData{}
    
    public class GameData: IData {
        public Vector3 HostPosition;
        public Vector3 HostVelocity;
        //use bit mask
        public int HostChargeLevel;
        public bool HostBreak;
        
        public Vector3 ClientPosition;
        public Vector3 ClientVelocity;
        //use bit mask
        public int ClientChargeLevel;
        public bool ClientBreak;
        
        //blockInfo
        public List<Vector3Int> BreakBlocks;
        public MapState[,] Map;
    }
}