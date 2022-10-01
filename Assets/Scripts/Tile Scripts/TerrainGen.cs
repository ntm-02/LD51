using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//can be used to create terrain
public class TerrainGen
{

    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeField] int seed;


    //must be filled in code, unfortunately
    private Dictionary<GameObject, Tile> availableNodes = new() {
        { Resources.Load<GameObject>("/h"), new Tile(Tile.RotationStyle.ALL) }
    };
}
