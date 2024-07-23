using UnityEngine;

namespace Lion.Utility
{
    public static class Vector2Extensions
    {
        public static Vector2 GetRandomDirection()
        {
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);

            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            return new Vector2(x, y);
        }
    }
}