using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeConsuming : MonoBehaviour
{
    float TimeCost = 0f;

    bool CheckValidTime()
    {
        return GameState.IsPlayerTurn ? PlayerTime.CheckDecrease(TimeCost) : EnemyTime.CheckDecrease(TimeCost);
    }
}