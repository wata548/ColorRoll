using Extensions;
using UnityEngine;

namespace Player {

    public static class CameraFocus {

       //==================================================||Constant 
        private const float Distance = 1.35f;
        private const float YSensitive = 0.2f;
        private const float XSensitive = 0.35f;

        private const float MinXAxis = 12f; 
        private const float MaxXAxis = 85f; 
        private const float SpringCameraAssist = -0.5f;
        
        
       //==================================================||Fields 
        private static float _xAxis = 0;
        private static float _yAxis = 0;
        private static Camera _camera = null;
        
       //==================================================||Properties 
        public static Quaternion CurRotation =>
            Quaternion.Euler(_xAxis, _yAxis, 0);

       //==================================================||Methods 
        private static Vector3 SpringCamera(Vector3 origin, Vector3 direction) {

            var result = direction;
            if (Physics.Raycast(origin, direction, out RaycastHit hit)) {

                var distance = Mathf.Max(hit.distance + SpringCameraAssist, 0);
                result = result.normalized 
                         * Mathf.Min(result.magnitude, distance);
            }

            return origin + result;
        }
        
       //==================================================||Unity
       
        public static void Update(GameObject target) {
            var delta = Input.mousePositionDelta;

            _yAxis += delta.x * YSensitive;
            _xAxis -= delta.y * XSensitive;
            _xAxis = ExSingle.Clamp(MinXAxis, _xAxis, MaxXAxis);

            _camera ??= Camera.main!;
            var rotation = CurRotation;
            var direction = rotation * Vector3.back * Distance;
            var pos = SpringCamera(target.transform.position, direction);
            
            _camera.transform.rotation = rotation;
            _camera.transform.position = pos;
        }
    }
}