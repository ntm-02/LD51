using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeConsuming : MonoBehaviour
{
    [SerializeField] float TimeCost = 0f;

    void SetTimeCost(float t)
    {
        TimeCost = t;
    }

    float GetTimeCost()
    {
        return TimeCost;
    }

    bool IsValidTimeUse()
    {
        return GameState.IsPlayerTurn ? PlayerTime.CheckDecrease(TimeCost) : EnemyTime.CheckDecrease(TimeCost);
    }
}