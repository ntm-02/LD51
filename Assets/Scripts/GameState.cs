using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static bool IsPlayerTurn = false;    // if false, enemy's turn
    public static bool HasDied = false;         // yoo fooken ded
    public static bool HasEscaped = false;      // player has won the game (or round)
    public static bool InCutscene = false;      // for engaging w/ enemies, interactables, etc.
}