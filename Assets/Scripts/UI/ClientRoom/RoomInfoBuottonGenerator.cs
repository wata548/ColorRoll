using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Networking.RoomSystem;
using UnityEngine;

namespace UI.ClientRoom {
    public class RoomInfoBuottonGenerator: MonoSingleton<RoomInfoBuottonGenerator> {

        private static readonly Vector2 DefaultPos = new(500, 960);
        private const float YInterval = 160;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private RoomInfoButton _prefab;
        private List<RoomInfoButton> _rooms = new();
        private int _count = 0;

        
       //==================================================||Methods 
        private void UpdateRoomInfo(List<(string roomName, string ip)> datas) {
            foreach (var room in _rooms) {
                Destroy(room.gameObject);
            }
            _rooms.Clear();

            int idx = 0;
            foreach (var data in datas) {
                var target = Instantiate(_prefab.gameObject, _canvas.transform);
                
                var pos = DefaultPos;
                pos.y -= YInterval * idx;
                target.transform.position = pos;
                var targetCommponent = target.GetComponent<RoomInfoButton>();
                targetCommponent.Set(data.roomName, data.ip);
                _rooms.Add(targetCommponent);
                idx++;

            }
            Debug.Log($"Updated (count: {datas.Count})");
        }

        public void Refresh() {
            
            var target = (NetworkManager.Instance.Room as RoomClient)!;
            target.Refresh();

            StartCoroutine(
                ExCoroutine.WaitStart(
                    RoomClient.WaitReceiveTime,
                    () => UpdateRoomInfo(target.FindRoom)
                )
            );
            
        }

        public void TurnOff() {
            foreach (var room in _rooms) {
                Destroy(room.gameObject);
            }
            _rooms.Clear();
            enabled = false;
        }
        
       //==================================================||Unity 
        private new void Awake() {
            base.Awake();
            Refresh();
        }
    }
}