using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

// This script holds miscellanious prefabs which will be instantiated via other scripts.
// credit : https://www.youtube.com/watch?v=iD1_JczQcFY&t=1004s&ab_channel=CodeMonkey 
public class GameAssetsContainer : MonoBehaviour
{
    private static GameAssetsContainer _i;

    public static GameAssetsContainer i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssetsContainer>("GameAssetsContainer"));
            return _i;
        }
    }

    public Transform pfMoveIndicator;

}
