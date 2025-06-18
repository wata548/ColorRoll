using UnityEngine;

namespace Extensions {
    public static class ExQuaternion {

        public static Vector3 Direction(this Quaternion dir) =>
            dir * Vector3.forward;

        public static Quaternion XAxis(this Quaternion dir) =>
            Quaternion.Euler(dir.eulerAngles.x, 0, 0);
        public static Quaternion YAxis(this Quaternion dir) =>
            Quaternion.Euler(0, dir.eulerAngles.y, 0);
        public static Quaternion ZAxis(this Quaternion dir) =>
            Quaternion.Euler(0, 0, dir.eulerAngles.z);
    }
}