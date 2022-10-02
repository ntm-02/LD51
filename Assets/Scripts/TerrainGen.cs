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
                GameObject tileObj = Instantiate(mapBack[temp], new Vector3(x / 1.05f, y / 1.05f/*, -output[x,y].height/8*/), zero, transform);
                tileObj.GetComponent<Tile>().gridPos = new Vector2(x, y);
                ret[x, y] = tileObj; 
            }
        }


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject selection = ret[x, y];
                int tileHeight = selection.GetComponent<Tile>().height;

                GameObject[] adjacentGOs = TilePathFinding.notNull(TilePathFinding.adjacentToPoint(ret, selection.GetComponent<Tile>().gridPos)).ToArray();

                List<GameObject> adjacentSameOrGreaterHeight = new();
                adjacentSameOrGreaterHeight.AddRange(from singleGO in adjacentGOs let singlesTile = singleGO.GetComponent<Tile>() where singlesTile.height >= tileHeight select singleGO);//up or down

                Quaternion zero = new();
                zero.eulerAngles = Vector3.zero;

                Tile selectionTile = selection.GetComponent<Tile>();

                if (
                    (adjacentSameOrGreaterHeight.Count == 2 
                        && (TilePathFinding.notNull(TilePathFinding.adjacentToPoint(ret, selectionTile.gridPos)).Count > 3)
                    ) || (adjacentSameOrGreaterHeight.Count == 1 
                        && (TilePathFinding.notNull(TilePathFinding.adjacentToPoint(ret, selectionTile.gridPos)).Count <= 3)
                    )
                )
                {                                                                                      //v magic number only works because curr heights are 0,8,16
                    ret[x, y].GetComponent<SpriteRenderer>().sprite = borders[(int)selectionTile.height/16].GetComponent<SpriteRenderer>().sprite;

                    Tile[] neighbors = (from gameObj in TilePathFinding.adjacentToPoint(ret, selectionTile.gridPos) let result = gameObj == null ? null : gameObj.GetComponent<Tile>() select result).ToArray();
                    //left, right, bottom, top lol



                    //up is null, right is good OR both are good 
                    if ((neighbors[1] == null && (neighbors[3] != null && neighbors[3].height == selectionTile.height)) || (neighbors[3] == null && (neighbors[1] != null && neighbors[1].height == selectionTile.height)) || (neighbors[3] != null && neighbors[3].height == selectionTile.height && neighbors[1] != null && neighbors[1].height == selectionTile.height)) {
                        ret[x, y].transform.Rotate(0, 0, 90);//top and right
                    } else//bottom null, right fine or bot right fine
                    if ((neighbors[1] == null && (neighbors[2] != null && neighbors[2].height == selectionTile.height)) || (neighbors[2] == null && (neighbors[1] != null && neighbors[1].height == selectionTile.height)) || (neighbors[2] != null && neighbors[2].height == selectionTile.height && neighbors[1] != null && neighbors[1].height == selectionTile.height))
                    {
                        ret[x, y].transform.Rotate(0, 0, 0);//bottom and right
                    } else//up null, left good or topleft fine
                    if ((neighbors[0] == null && (neighbors[3] != null && neighbors[3].height == selectionTile.height)) || (neighbors[3] == null && (neighbors[0] != null && neighbors[0].height == selectionTile.height)) || (neighbors[3] != null && neighbors[3].height == selectionTile.height && neighbors[0] != null && neighbors[0].height == selectionTile.height))
                    {
                        ret[x, y].transform.Rotate(0, 0, 180);//top and left
                    } else//bottom null, left good or left null, bottom good or botleft fine
                    if ((neighbors[0] == null && (neighbors[2] != null && neighbors[2].height == selectionTile.height)) || (neighbors[2] == null && (neighbors[0] != null && neighbors[0].height == selectionTile.height)) || (neighbors[2] != null && neighbors[2].height == selectionTile.height && neighbors[0] != null && neighbors[0].height == selectionTile.height))
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
