using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeConsuming : MonoBehaviour
{
    // any interactable object
    // 

    float TimeCost = 0f;

    bool CheckValidTime()
    {
        return PlayerTime.CheckDecrease(TimeCost);
    }
}