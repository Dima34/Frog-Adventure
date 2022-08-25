using UnityEngine;
using System.Collections.Generic;

public static class Utils {
    public static float EaseInCirc(float x) {
        return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
    }

    public static float EaseInOut(float x)
    {
        return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
    }

    public static int GetRandomExceptNumList(int min, int max, List<int> exceptNumberList)
    {
        int number;

        do
        {
            number = Random.Range(min, max);
        } while (exceptNumberList.Contains(number));

        return number;
    }
}