using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTime : MonoBehaviour
{
    public static float currPlayerTime = 10f;

    public static void ResetTime()
    {
        currPlayerTime = 10f;
    }

    public static float DecreaseTime(float d)
    {
        if (CheckDecrease(d))
        {
            currPlayerTime -= d;
            return d;
        }
        else
        {
            return -1f;
        }
    }

    public static bool CheckDecrease(float d)
    {
        return d <= currPlayerTime ? true : false;
    }
}
