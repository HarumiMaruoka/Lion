using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Lion.CameraUtility
{
    public static class CameraExtensions
    {
        public static Vector2 GetWorldTopRight(this Camera camera)
        {
            return camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        }

        public static Vector2 GetWorldBottomLeft(this Camera camera)
        {
            return camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        }

        // カメラから一定距離離れているかどうかを返す。
        public static bool IsFarFromCamera(this Camera camera, Vector3 position, Vector2 distance)
        {
            var xDiff = Mathf.Abs(position.x - camera.transform.position.x);
            var yDiff = Mathf.Abs(position.y - camera.transform.position.y);

            var xDiffIsFar = xDiff > distance.x;
            var yDiffIsFar = yDiff > distance.y;

            return xDiffIsFar || yDiffIsFar;
        }

        // カメラの範囲外になったかどうかを返す。
        public static bool IsFarFromCamera(this Camera camera, Vector3 position)
        {
            var topRight = camera.GetWorldTopRight();
            var top = topRight.y;
            var right = topRight.x;

            var leftBottom = camera.GetWorldBottomLeft();
            var bottom = leftBottom.y;
            var left = leftBottom.x;

            var xDistance = (right - left) / 2f;
            var yDistance = (top - bottom) / 2f;

            return camera.IsFarFromCamera(position, new Vector2(xDistance, yDistance));
        }

        // カメラから遠すぎるかどうかを返す。
        public static bool IsTooFarFromCamera(this Camera camera, Vector3 position)
        {
            var topRight = camera.GetWorldTopRight();
            var top = topRight.y;
            var right = topRight.x;

            var leftBottom = camera.GetWorldBottomLeft();
            var bottom = leftBottom.y;
            var left = leftBottom.x;

            var xDistance = (right - left) / 2f + 5f;
            var yDistance = (top - bottom) / 2f + 5f;

            return camera.IsFarFromCamera(position, new Vector2(xDistance, yDistance));
        }

        // カメラの描画範囲内のランダムな座標を返す。
        public static Vector2 GetRandomCameraArea(this Camera camera)
        {
            var topRight = camera.GetWorldTopRight();
            var top = topRight.y;
            var right = topRight.x;

            var leftBottom = camera.GetWorldBottomLeft();
            var bottom = leftBottom.y;
            var left = leftBottom.x;

            var halfWidth = (right - left) / 2f;
            var halfHeight = (top - bottom) / 2f;

            var x = UnityEngine.Random.Range(left, right);
            var y = UnityEngine.Random.Range(bottom, top);

            return new Vector2(x, y);
        }
    }
}
