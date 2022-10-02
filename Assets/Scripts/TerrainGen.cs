using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


//can be used to create terrain
public class TerrainGen : MonoBehaviour
{
    //number of tiles to generate
    [SerializeField] int width;
    [SerializeField] int height;


    //random seed. default -1 is unseeded. Only changes on prog start, should prob be fixed, eh.
    [SerializeField] int seed = -1;


    //all tile prefabs
    [SerializeField]
    public List<GameObject> prefabs;

    public List<GameObject> borders;
    //tile components of prefabs
    private List<Tile> tiles = new();

    //maps tiles back to there gameobjects
    private Dictionary<Tile, GameObject> mapBack = new();

    //theoretical "zoom" factor. acceptable values about 1/10 of width to 1/4 (~2 to ~8 for 25 width)
    public float PerlinScale = 2.0f;

    private float offset = 0;

    //how random to make the noise. 0 is a set seed (essentially), 1 is meh. 20+ works well, what I use.
    public float offsetMultiplier = 1;

    //best set in editor. for 3 tiles (current), values below .33 are tile1, .33->.66 is tile2, and .66->1 is tile3
    public AnimationCurve change = AnimationCurve.Linear(0,0,1,1);

    public void Start() {
        //grab tiles from prefabs
        prefabs.ForEach(g => { tiles.Add(g.GetComponent<Tile>()); mapBack.Add(g.GetComponent<Tile>(), g);  });

        tiles.Sort((i1,i2)=> { return i1.height - i2.height; });
        //init seed
        Random.InitState(seed == -1 ? System.DateTime.Now.GetHashCode() : seed);
        //Debug.Log(Random.seed);
        //get an offset
        offset = Random.value;

        
        
    }

    //display a created terrain
    public void DisplayTerrain() {
        //redo offset
        offset = Random.value;
        //destroy current set
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject[,] output = CreateTerrain();

    }

    //create tile based on the height of perlin noise
    public GameObject[,] CreateTerrain() {
        GameObject[,] ret = new GameObject[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Tile temp = tiles[(int)(tiles.Count*Mathf.Clamp(change.Evaluate(CalcNoise(x,y)), 0, .99f))];
                Quaternion zero = new();
                zero.eulerAngles = Vector3.zero;
                GameObject tileObj = Instantiate(mapBack[temp], new Vector3(x / 1.6f, y / 1.6f/*, -output[x,y].height/8*/), zero, transform);
                tileObj.GetComponent<Tile>().gridPos = new Vector2(x, y);
                ret[x, y] = tileObj; 
            }
        }


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject g = ret[x, y];
                int th = g.GetComponent<Tile>().height;

                GameObject[] adj = TilePathFinding.notNull(TilePathFinding.adjacentToPoint(ret, g.GetComponent<Tile>().gridPos)).ToArray();

                List<GameObject> li = new();
                li.AddRange(from a in adj let c = a.GetComponent<Tile>() where c.height >= th select a);//up or down

                Quaternion zero = new();
                zero.eulerAngles = Vector3.zero;

                Tile m = g.GetComponent<Tile>();

                if (
                    (li.Count == 2 
                        && (TilePathFinding.notNull(TilePathFinding.adjacentToPoint(ret, m.gridPos)).Count > 3)
                    ) || (li.Count == 1 
                        && (TilePathFinding.notNull(TilePathFinding.adjacentToPoint(ret, m.gridPos)).Count <= 3)
                    )
                )
                {
                    ret[x, y].GetComponent<SpriteRenderer>().sprite = borders[(int)m.height/16].GetComponent<SpriteRenderer>().sprite;

                    Tile[] neighbors = (from go in TilePathFinding.adjacentToPoint(ret, m.gridPos) let r = go == null ? null : go.GetComponent<Tile>() select r).ToArray();

                    //left, right, bottom, top lol

                    int[] states = new int[4];



                    //up is null, right is good OR both are good 
                    if ((neighbors[1] == null && (neighbors[3] != null && neighbors[3].height == m.height)) || (neighbors[3] == null && (neighbors[1] != null && neighbors[1].height == m.height)) || (neighbors[3] != null && neighbors[3].height == m.height && neighbors[1] != null && neighbors[1].height == m.height)) {
                        ret[x, y].transform.Rotate(0, 0, 90);//top and right
                    } else//bottom null, right fine or bot right fine
                    if ((neighbors[1] == null && (neighbors[2] != null && neighbors[2].height == m.height)) || (neighbors[2] == null && (neighbors[1] != null && neighbors[1].height == m.height)) || (neighbors[2] != null && neighbors[2].height == m.height && neighbors[1] != null && neighbors[1].height == m.height))
                    {
                        ret[x, y].transform.Rotate(0, 0, 0);//bottom and right
                    } else//up null, left good or topleft fine
                    if ((neighbors[0] == null && (neighbors[3] != null && neighbors[3].height == m.height)) || (neighbors[3] == null && (neighbors[0] != null && neighbors[0].height == m.height)) || (neighbors[3] != null && neighbors[3].height == m.height && neighbors[0] != null && neighbors[0].height == m.height))
                    {
                        ret[x, y].transform.Rotate(0, 0, 180);//top and left
                    } else//bottom null, left good or left null, bottom good or botleft fine
                    if ((neighbors[0] == null && (neighbors[2] != null && neighbors[2].height == m.height)) || (neighbors[2] == null && (neighbors[0] != null && neighbors[0].height == m.height)) || (neighbors[2] != null && neighbors[2].height == m.height && neighbors[0] != null && neighbors[0].height == m.height))
                    {
                        ret[x, y].transform.Rotate(0, 0, 270);//bottom and left
                    }

                }
            }
        }

        return ret;
    }


    //gen a perlin noise point
    private float CalcNoise(float x, float y)
    {
        
        float xCoord = (offset * offsetMultiplier) + x / (width / PerlinScale);
        float yCoord = (offset * offsetMultiplier) + y / (height / PerlinScale);
        return Mathf.PerlinNoise(xCoord, yCoord);

    }
}
