using UnityEngine;
public static class BezierCurve
{
    public static Vector2 Quadratic(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        float u = 1f - t;
        return (u * u) * p0 + 2f * (u * t) * p1 + (t * t) * p2;
    }
    public static Vector2 Cubic(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        float u = 1f - t;
        return u * u * u * p0
               + 3f * u * u * t * p1
               + 3f * u * t * t * p2
               + t * t * t * p3;
    }
}