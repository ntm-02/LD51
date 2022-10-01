using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTime : TimeCurrency
{
    public float currPlayerTime;

    public override float getCurrTime()
    {
        return currPlayerTime;
    }

    public override void resetTime()
    {
        currPlayerTime = 10f;
    }

    public override float decreaseTime(float d)
    {
        if (checkDecrease(d))
        {
            currPlayerTime -= d;
            return d;
        }
        else
        {
            return -1f;
        }
    }

    public override bool checkDecrease(float d)
    {
        return d <= currPlayerTime ? true : false;
    }
}
