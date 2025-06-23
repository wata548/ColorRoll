using Networking.InGame;
using RayFire;
using UnityEngine;

namespace MapInfo {
    public partial class BlockGenerator {
        public void Apply(string data) {
            var gameData = new GameData(data);
            
            
            foreach (var @break in gameData.BreakBlocks) {
                if(_blocks.TryGetValue((Vector3)@break, out var result)) {
                    result.GetComponent<RayfireRigid>().Demolish();
                    BreakBlock((Vector3)@break);
                }
            }
        }
    }
}