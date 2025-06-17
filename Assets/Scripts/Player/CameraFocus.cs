using Extensions;
using UnityEngine;

namespace Player {

    public static class CameraFocus {

        private const float Distance = 1.35f;
        private const float YSensitive = 0.2f;
        private const float XSensitive = 0.35f;

        private const float MinXAxis = 12f; 
        private const float MaxXAxis = 85f; 
        
        private static float _xAxis = 0;
        private static float _yAxis = 0;
        private static Camera _camera = null;
        
        public static Quaternion CurRotation =>
            Quaternion.Euler(_xAxis, _yAxis, 0);
        
        public static void Update(GameObject target) {
            var delta = Input.mousePositionDelta;

            var tempX = _xAxis;
            var tempY = _yAxis;
            
            _yAxis += delta.x * YSensitive;
            _xAxis -= delta.y * XSensitive;
            _xAxis = ExSingle.Clamp(MinXAxis, _xAxis, MaxXAxis);

            _camera ??= Camera.main!;
            var rotation = CurRotation;
            var pos = target.transform.position + rotation * Vector3.back * Distance;
            
            if (Physics.Raycast(pos, Vector3.zero, 0f)) {
                _xAxis = tempX;
                _yAxis = tempY;
                rotation = CurRotation;
                pos = target.transform.position + rotation * Vector3.back * Distance;   
            }
            
            _camera.transform.rotation = rotation;
            _camera.transform.position = pos;
        }
    }
}