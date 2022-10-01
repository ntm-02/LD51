using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCurrency : MonoBehaviour
{

    static float playerTime;
    static float enemyTime;

    static float getCurrPlayerTime()
    {
        return playerTime;
    }

    static float getCurrEmemyTime()
    {
        return enemyTime;
    }

    static void resetTime()
    {
        playerTime = 10f;
        Debug.Log("Current time available is 10 SECONDS.");
    }

    static bool validDecrease(float d) // can we keep currTime >= 0 if we subtract d? If not, return false
    {
        return d <= playerTime ? true : false;
    }

    static float decreaseTime(float d) // decrease time; if not, return -1f
    {
        if (validDecrease(d))
        {
            playerTime -= d;
            return d;
        }
        else
        {
            return -1f;
        }
    }

}
