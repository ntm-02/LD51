using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class TilePathFinding : MonoBehaviour
{
    

    //GameObject[,] tileMap = new GameObject[3,2] { {null, null}, {null, null}, {null, null }  };
    //[SerializeField] List<GameObject> tiles = new List<GameObject>();
    //private List<GameObject> queue = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
       /* for (int col = 0; col < tileMap.GetLength(0); col++)
        {
            for (int row = 0; row < tileMap.GetLength(1); row++)
            {
                tileMap[col, row] = tiles[row];
                //print(tileMap[col, row].name);
            }
        }*/

        //print(tileMap[0, 0].name);
        //GameObject start = tileMap[0, 0];
        //queue.Add(start);

        //start.GetComponent<MouseOnTile>().incrementMark();







    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("i")) {
            foreach (GameObject g in FindShortestPath(Vector2.zero, new Vector2(20, 20))) {
                //print("something");
                g.GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }
    }

    // returns the shortest path as a list of GameObjects that are tiles in the grid
    private List<GameObject> FindShortestPath(Vector2 playerpos, Vector2 targetpos)
    {
        GameObject[,] grid = FindObjectOfType<TerrainGen>().CreateTerrain();
        //grid[0, 0].GetComponent<SpriteRenderer>().color = Color.black;
        /* for (int col = 0; col < grid.GetLength(0); col++)
         {
             for (int row = 0; row < grid.GetLength(1); row++)
             {
                 grid[col, row].GetComponent<SpriteRenderer>().color = Color.white;  // this needs to be reset each time
                //PrefabUtility.UnpackPrefabInstance(grid[col, row], PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                 //print(grid[col, row].name);
             }
         }
        //grid[6, 2].transform.GetComponent<SpriteRenderer>().color = Color.black;
        //print(grid[6, 2].GetComponent<SpriteRenderer>().color);*/


        List<GameObject> farthest = new() { };
        farthest.AddRange(adjacentToPoint(grid, playerpos));
        foreach (GameObject g in farthest)
        {
            //print(g.GetComponent<Tile>().gridPos);
        }

        HashSet<GameObject> used = new();

        Comparer<GameObject> sorter = Comparer<GameObject>.Create((a, b) => a.GetComponent<Tile>().pathCost - b.GetComponent<Tile>().pathCost);

        farthest.Sort(sorter);

        while (farthest.Count > 0)
        {
            //active is the node with the shortest path. (sorted above.)
            GameObject active = farthest[0];
            if (active == grid[(int)targetpos.x, (int)targetpos.y])  // if destination reached, return path
            {
                active.GetComponent<Tile>().pathFromRoot.Add(active);
                //print("made it here");
                break;
            }
            //convert farthest to set to disallow dupes.
            HashSet<GameObject> unique = new HashSet<GameObject>(farthest);

            foreach (GameObject g in Enumerable.Except(adjacentToPoint(grid, active.GetComponent<Tile>().gridPos),used))
            {
                //set the path
                g.GetComponent<Tile>().pathFromRoot.AddRange(active.GetComponent<Tile>().pathFromRoot);
                g.GetComponent<Tile>().pathFromRoot.Add(active);


                //add to unique if not already used.
                if (!used.Contains(g))
                {
                    g.GetComponent<Tile>().pathCost = active.GetComponent<Tile>().pathCost +/* ( (int)Vector2.Angle(targetpos, active.GetComponent<Tile>().gridPos) */
                         active.GetComponent<Tile>().travelCost;
                    unique.Add(g);
                    used.Add(g);
                }
            }

            //reconvert to list to allow sorting
            farthest = new List<GameObject>(unique);

            //remove node checked in this iteration
            farthest.Remove(active);

            //sort
            farthest.Sort(sorter);

            //dirty this iterations node
            used.Add(active);

        }

        //clear out all the nodes. not sure why i don't do this with other returns, but whatever. it works.
        List<GameObject> temp = new() { };
        temp.AddRange(farthest[0].GetComponent<Tile>().pathFromRoot);
        foreach (GameObject g in temp)
        {
            //print(g.GetComponent<Tile>().gridPos);
        }
        resetUsed(used);
        foreach (GameObject g in temp)
        {
            print(g.GetComponent<Tile>().gridPos);
        }
        return temp;
            //FindObjectOfType<TerrainGen>().DisplayTerrain();
        }

    private void resetUsed(HashSet<GameObject> used)
    {
        foreach (GameObject g in used)
        {
            g.GetComponent<Tile>().pathFromRoot.Clear();
        }
    }

    public GameObject[] adjacentToPoint(GameObject[,] actingArray, Vector2 point)
    {
        List<GameObject> ret = new();
        //x column from bot left
        //y row from bot left
        if (point.x != 0)
        {
            ret.Add(actingArray[(int)point.x - 1, (int)point.y]);
        }
        if (point.x != actingArray.GetLength(0) - 1)
        {
            ret.Add(actingArray[(int)point.x + 1, (int)point.y]);
        }
        if (point.y != 0)
        {
            ret.Add(actingArray[(int)point.x, (int)point.y - 1]);
        }
        if (point.y != actingArray.GetLength(1) - 1)
        {
            ret.Add(actingArray[(int)point.x, (int)point.y + 1]);
        }

        return ret.ToArray();
    }
}
