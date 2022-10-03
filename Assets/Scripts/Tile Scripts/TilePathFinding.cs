using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class TilePathFinding : MonoBehaviour
{
    GameObject[,] grid;
    
    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<TerrainGen>().CreateTerrain();
    }

    // this will reset all the tiles back to their normal colors
    public void clearPathTrail()
    {
        foreach (GameObject g in grid)
        {
            g.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public GameObject[,] getGrid()
    {
        return grid;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("i")) {
            grid = FindObjectOfType<TerrainGen>().CreateTerrain();
            /*foreach (GameObject g in FindShortestPath(Vector2.zero, new Vector2(20, 20))) {
                //print("something");
                g.GetComponent<SpriteRenderer>().color = Color.gray;
            }*/
        }
    }




    // returns the shortest path as a list of GameObjects that are tiles in the grid
    public static List<GameObject> FindShortestPath(GameObject[,] grid, Vector2 playerpos, Vector2 targetpos)
    {
        //print("player: " + playerpos + " target: " + targetpos);

        List<GameObject> farthest = new() { };
        farthest.AddRange(notNull(adjacentToPoint(grid, playerpos)));
        //print(farthest.Count);

        HashSet<GameObject> used = new();
        foreach (GameObject g in grid)
        {
            if (g.CompareTag("UnWalkable"))
            {
                used.Add(g);
            }
        }
        farthest = Enumerable.Except(farthest, used).ToList();
        Comparer<GameObject> sorter = Comparer<GameObject>.Create((a, b) => a.GetComponent<Tile>().pathFromRoot.Count - b.GetComponent<Tile>().pathFromRoot.Count);

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

            foreach (GameObject g in Enumerable.Except(notNull(adjacentToPoint(grid, active.GetComponent<Tile>().gridPos)), used))
            {
                //set the path
                g.GetComponent<Tile>().pathFromRoot.AddRange(active.GetComponent<Tile>().pathFromRoot);
                g.GetComponent<Tile>().pathFromRoot.Add(active);


                //add to unique if not already used.
                if (!used.Contains(g))
                {
                    //g.GetComponent<Tile>().pathCost = active.GetComponent<Tile>().pathCost + active.GetComponent<Tile>().travelCost;
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
        temp.AddRange(farthest[0].GetComponent<Tile>().pathFromRoot); // out of range index exception?
        foreach (GameObject g in temp)
        {
            //print(g.GetComponent<Tile>().gridPos);
        }
        resetUsed(grid, used);
        foreach (GameObject g in grid)
        {
            if (g.GetComponent<Tile>().pathFromRoot.Count > 0)
            {
                print(g.GetComponent<Tile>().pathFromRoot.Count);
            }
            //print(g.GetComponent<Tile>().pathFromRoot.Count);
        }
        return temp;
            //FindObjectOfType<TerrainGen>().DisplayTerrain();
        }

    private static void resetUsed(GameObject[,] grid, HashSet<GameObject> used)
    {
        foreach (GameObject g in grid)
        {
            g.GetComponent<Tile>().pathFromRoot.Clear();
        }
    }

    public static List<T> notNull<T>(IEnumerable<T> input) {
        List<T> ret = new();

        foreach (T b in input) {
            if (b != null) {
                ret.Add(b);
            }
        }

        return ret;
    }


    public static GameObject[] adjacentToPoint(GameObject[,] actingArray, Vector2 point)
    {
        GameObject [] ret = new GameObject[4];
        //x column from bot left
        //y row from bot left
        if (point.x != 0)
        {
            ret[0] = (actingArray[(int)point.x - 1, (int)point.y]);
        }
        if (point.x != actingArray.GetLength(0) - 1)
        {
            ret[1] = (actingArray[(int)point.x + 1, (int)point.y]);
        }
        if (point.y != 0)
        {
            ret[2] =(actingArray[(int)point.x, (int)point.y - 1]);
        }
        if (point.y != actingArray.GetLength(1) - 1)
        {
            ret[3] =(actingArray[(int)point.x, (int)point.y + 1]);
        }

        return ret;
    }
}
