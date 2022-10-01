using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeCurrency
{
    protected float currentTime;

    public abstract bool checkDecrease(float d);

    public abstract float decreaseTime(float d);

    public abstract float getCurrTime();

    public abstract void resetTime();
}