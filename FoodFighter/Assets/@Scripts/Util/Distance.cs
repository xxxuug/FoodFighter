using UnityEngine;

public static class Distance
{
    public static float GetDistance(Transform A, Transform B)
    {
        return Mathf.Abs(A.position.x - B.position.x);
    }
}
