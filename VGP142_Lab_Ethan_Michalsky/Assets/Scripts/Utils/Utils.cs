using UnityEngine;

namespace Utilities {
    public static class Utils {
        private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

        public static Vector3 toIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
    }
}