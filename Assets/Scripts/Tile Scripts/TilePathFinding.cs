using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePathFinding : MonoBehaviour
{
    

    GameObject[,] tileMap = new GameObject[3,2] { {null, null}, {null, null}, {null, null }  };
    [SerializeField] List<GameObject> tiles = new List<GameObject>();
    private List<GameObject> queue = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int col = 0; col < tileMap.GetLength(0); col++)
        {
            for (int row = 0; row < tileMap.GetLength(1); row++)
            {
                tileMap[col, row] = tiles[row];
                //print(tileMap[col, row].name);
            }
        }

        //print(tileMap[0, 0].name);
        GameObject start = tileMap[0, 0];
        queue.Add(start);

        //start.GetComponent<MouseOnTile>().incrementMark();



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
