using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class ExtraMath
    {
        public static List<Vector2> GetCircleArrangement(int count, float radius = 1f)
        {
            List<Vector2> result = new();
            if (count == 1)
            {
                result.Add(Vector2.zero);
                return result;
            }
            for (int i = 0; i < count; i++)
            {
                float degree = i * 360f / count;
                Vector2 endPos = RotateVectorAroundOrigin(Vector2.up, degree);
                endPos *= radius;
                result.Add(endPos);
            }
            return result;
        }
        private static Vector2 RotateVectorAroundOrigin(Vector2 vector, float angle)
        {
            float angleInRadians = angle * Mathf.Deg2Rad;
            float cosAngle = Mathf.Cos(angleInRadians);
            float sinAngle = Mathf.Sin(angleInRadians);

            float x = vector.x * cosAngle - vector.y * sinAngle;
            float y = vector.x * sinAngle + vector.y * cosAngle;

            return new Vector2(x, y);
        }
    }
}