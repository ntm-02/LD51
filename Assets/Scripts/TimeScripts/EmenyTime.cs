using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmenyTime : TimeCurrency
{
    public float currEnemyTime;

    public override float getCurrTime()
    {
        return currEnemyTime;
    }

    public override void resetTime()
    {
        currEnemyTime = 10f;
    }

    public override float decreaseTime(float d)
    {
        if (checkDecrease(d))
        {
            currEnemyTime -= d;
            return d;
        }
        else
        {
            return -1f;
        }
    }

    public override bool checkDecrease(float d)
    {
        return d <= currEnemyTime ? true : false;
    }
}
