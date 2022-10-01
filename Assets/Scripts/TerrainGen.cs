using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//can be used to create terrain
public class TerrainGen : MonoBehaviour
{

    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeField] int seed = -1;

    [SerializeField]
    public List<GameObject> prefabs;

    private List<Tile> tiles = new();

    

    public void Start() {
        prefabs.ForEach(g => tiles.Add(g.GetComponent<Tile>()));


        Random.InitState(seed == -1 ? System.DateTime.Now.GetHashCode() : seed);
    }


    public Tile[,] CreateTerrain() {
       Tile[,] ret = new Tile[width, height];



        return ret;
    }

}
