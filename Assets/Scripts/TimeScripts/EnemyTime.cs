using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTime : MonoBehaviour
{
    public static float currEnemyTime = 10f;

    public static void ResetTime()
    {
        currEnemyTime = 10f;
    }

    public static float DecreaseTime(float d)
    {
        if (CheckDecrease(d))
        {
            currEnemyTime -= d;
            return d;
        }
        else
        {
            return -1f;
        }
    }

    public static bool CheckDecrease(float d)
    {
        return d <= currEnemyTime ? true : false;
    }
}
