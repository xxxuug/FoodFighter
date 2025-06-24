using UnityEngine;

public static class Utils
{
    public static float GetDistance(Transform A, Transform B)
    {
        return Mathf.Abs(A.position.x - B.position.x);
    }

    public static string FormatKoreanNumber(long num)
    {
        if (num == 0) return "0";

        long eok = num / 100000000;
        long man = (num % 100000000) / 10000;
        long il = num % 10000;

        string result = "";
        if (eok > 0) result += $"{eok}억 ";
        if (man > 0) result += $"{man}만 ";
        if (il > 0 || result == "") result += $"{il}";

        return result.Trim(); // 소수점 제거
    }
}
