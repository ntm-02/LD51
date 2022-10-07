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

        GameObject[,] output = CreateTerrain2();


    }

    //create tile based on the height of perlin noise
    public GameObject[,] CreateTerrain() {

        offset = Random.value;
        GameObject[,] ret = new GameObject[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Tile temp = tiles[(int)(tiles.Count*Mathf.Clamp(change.Evaluate(CalcNoise(x,y)), 0, .99f))];
                Quaternion zero = new();
                zero.eulerAngles = Vector3.zero;
                GameObject tileObj = Instantiate(mapBack[temp], new Vector3(x / 1.05f, y / 1.05f), zero, transform);
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
                    if ((neighbors[1] == null && (neighbors[3] != null && neighbors[3].type == selectionTile.type)) 
                        || (neighbors[3] == null && (neighbors[1] != null && neighbors[1].type == selectionTile.type)) 
                        || (neighbors[3] != null && neighbors[3].type == selectionTile.type && neighbors[1] != null && neighbors[1].type == selectionTile.type)) {
                        ret[x, y].transform.Rotate(0, 0, 90);//top and right
                    } else//bottom null, right fine or bot right fine
                    if ((neighbors[1] == null && (neighbors[2] != null && neighbors[2].type == selectionTile.type)) || (neighbors[2] == null && (neighbors[1] != null && neighbors[1].type == selectionTile.type)) || (neighbors[2] != null && neighbors[2].type == selectionTile.type && neighbors[1] != null && neighbors[1].type == selectionTile.type))
                    {
                        ret[x, y].transform.Rotate(0, 0, 0);//bottom and right
                    } else//up null, left good or topleft fine
                    if ((neighbors[0] == null && (neighbors[3] != null && neighbors[3].type == selectionTile.type)) || (neighbors[3] == null && (neighbors[0] != null && neighbors[0].type == selectionTile.type)) || (neighbors[3] != null && neighbors[3].type == selectionTile.type && neighbors[0] != null && neighbors[0].type == selectionTile.type))
                    {
                        ret[x, y].transform.Rotate(0, 0, 180);//top and left
                    } else//bottom null, left good or left null, bottom good or botleft fine
                    if ((neighbors[0] == null && (neighbors[2] != null && neighbors[2].type == selectionTile.type)) || (neighbors[2] == null && (neighbors[0] != null && neighbors[0].type == selectionTile.type)) || (neighbors[2] != null && neighbors[2].type == selectionTile.type && neighbors[0] != null && neighbors[0].type == selectionTile.type))
                    {
                        ret[x, y].transform.Rotate(0, 0, 270);//bottom and left
                    }

                }
            }
        }


        Structure startHut = (Resources.Load("starting") as GameObject).GetComponent<Structure>();
        startHut.Start();

        bool failed = true;

        Vector2 starting = Vector2.zero;
        for (int x = 0; x < height; x++) {
            //print(startHut.doesFitAtPoint(ret, new Vector2(x,0), false));
                if (startHut.doesFitAtPoint(ret, new Vector2(0, x), false)) {
                    ret = startHut.replaceStructTiles(ret, new Vector2(0, x));
                    starting = new Vector2(0, x+1);
                    failed = false;
                    break;
                }
                
        }

        Structure endGate = (Resources.Load("ending") as GameObject).GetComponent<Structure>();
        endGate.Start();
        bool failed2 = true;
        Vector2 ending = Vector2.zero;
        for (int x = height; x > 0; x--)
        {
            //print(startHut.doesFitAtPoint(ret, new Vector2(x,0), false));
            if (endGate.doesFitAtPoint(ret, new Vector2(width-3, x), false))
            {
                ret = endGate.replaceStructTiles(ret, new Vector2(width-3, x));
                ending = new Vector2(width-1, x+2);
                failed2 = false;
                break;
            }

        }



        if (failed || failed2)
        {
            foreach (GameObject g in ret) {
                Destroy(g);
            }
            ret = CreateTerrain();
        }
        else {
            try
            {
                List<GameObject> availablePath = TilePathFinding.FindShortestPath(ret, starting, ending);

                foreach (GameObject obj in availablePath) {
                    if (obj.GetComponent<Tile>().type == Tile.TileType.water) {
                        Transform origTrans = obj.transform;
                        Vector3 pos = origTrans.position;
                        Quaternion rot = origTrans.rotation;
                        Transform parent = origTrans.parent;
                        Vector2 origGridPos = obj.GetComponent<Tile>().gridPos;

                        GameObject tileObj = Instantiate(Resources.Load("Bridge") as GameObject, pos, rot, parent);
                        tileObj.GetComponent<Tile>().gridPos = origGridPos;
                        Destroy(obj);
                        ret[(int)origGridPos.x, (int)origGridPos.y] = tileObj;
                    }
                }
            }
            catch (System.ArgumentOutOfRangeException) {
                foreach (GameObject g in ret)
                {
                    Destroy(g);
                }
                ret = CreateTerrain();
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



    public int avgRoomSize;

    public int minRoomSize = 3;

    public GameObject[,] CreateTerrain2() {

        GameObject[,] ret = new GameObject[width+2, height+2];

        List<Room> rooms = new()
        {
            new Room(0, 25, 25, 0)
        };


        for (int x = 0; x < 27; x++)
        {
            Quaternion zero = new();
            zero.eulerAngles = Vector3.zero;
            GameObject tileObj = Object.Instantiate(Resources.Load("wall tile") as GameObject, new Vector3(x / 1.05f, 0 / 1.05f), zero, transform);
            tileObj.GetComponent<Tile>().gridPos = new Vector2(x, 0);
            ret[x, 0] = tileObj;

            tileObj = Object.Instantiate(Resources.Load("wall tile") as GameObject, new Vector3(x / 1.05f, 26 / 1.05f), zero, transform);
            tileObj.GetComponent<Tile>().gridPos = new Vector2(x, 25);
            ret[x, 25] = tileObj;
        }

        for (int y = 1; y < 27; y++)
        {
            Quaternion zero = new();
            zero.eulerAngles = Vector3.zero;
            GameObject tileObj = Object.Instantiate(Resources.Load("wall tile") as GameObject, new Vector3(0 / 1.05f, y / 1.05f), zero, transform);
            tileObj.GetComponent<Tile>().gridPos = new Vector2(0,y);
            ret[0, y] = tileObj;

            tileObj = Object.Instantiate(Resources.Load("wall tile") as GameObject, new Vector3(26 / 1.05f, y / 1.05f), zero, transform);
            tileObj.GetComponent<Tile>().gridPos = new Vector2(25,y);
            ret[25, y] = tileObj;
        }

        double currAvgSize = rooms.Average((r)=>r.size);
        bool willVert = false;

        willVert = (int)(Random.value * 2) == 0;

        //int n = 0;
        while (currAvgSize > avgRoomSize) {
            Room largest = rooms.Max();
            //rooms.Sort();
            //rooms.Reverse();
            //Room largest = rooms[rooms.Count * (int)Mathf.Clamp(change.Evaluate(Random.value), 0, .99f)];
            rooms.Remove(largest);

            willVert = !willVert;

            if (largest.width >= (2 * minRoomSize) + 1 && largest.height >= (2 * minRoomSize) + 1)
            {
                
            }
            else if (largest.width >= (2 * minRoomSize) + 1)
            {
                willVert = true;
            }
            else if (largest.height >= (2 * minRoomSize) + 1)
            {
                willVert = false;
            }
            else {
                throw new System.Exception("Rooms cannot generate with current settings!");
            }



            //int numDivs = 1 + (int)(Random.value * 2);//1-3 cuts, 2-4 rooms      future implementation

            //for (int x = numDivs; x > 0; x--) {
            int wallInd = willVert ? Random.Range(largest.left + minRoomSize, largest.right - minRoomSize) : Random.Range(largest.bot + minRoomSize, largest.top - minRoomSize);
                rooms.AddRange(largest.Split(this, ref ret, new Vector2Int(wallInd, willVert ? 0 : 1)));
            //}

            currAvgSize = rooms.Average((r) => r.size);

            //print("iter: " + n + " " + willVert + " " + wallInd);

            //n++;

        }


        return ret;

    }











}


public class Room : System.IComparable<Room>
{

    public int size;

    public int left;
    public int right;
    public int bot;
    public int top;

    public int height;
    public int width;

    public Room(int left, int right, int top, int bottom) {
        this.left = left;
        this.right = right;
        this.top = top;
        this.bot = bottom;

        height = top - bot;
        width = right - left;

        size = height * width;
    }

    public int CompareTo(Room room)
    {
        return size - room.size;
    }

    public List<Room> Split(TerrainGen parent, ref GameObject[,] map, Vector2Int along) {//coord, then (0 for vertical, 1 for horiz)


        List<Room> ret = new();

        bool isVertical = along.y == 0;

        
        if (isVertical)
        {
            ret.Add(new Room(left, along.x-1, top, bot));
            ret.Add(new Room(along.x+1, right, top, bot));

            for (int x = bot; x < top + (top != map.GetLength(1)-2 ? 1 : 0); x++) {
                Quaternion zero = new();
                zero.eulerAngles = Vector3.zero;
                GameObject tileObj = Object.Instantiate(Resources.Load("wall tile") as GameObject, new Vector3((along.x+1) / 1.05f, (x+1) / 1.05f), zero, parent.transform);
                tileObj.GetComponent<Tile>().gridPos = new Vector2(along.x, x);
                map[along.x+1, x+1] = tileObj;
            }
        }
        else {
            ret.Add(new Room(left, right, along.x-1, bot));
            ret.Add(new Room(left, right, top, along.x+1));


            for (int x = left; x < right + (right != map.GetLength(1) - 2 ? 1 : 0); x++)
            {
                Quaternion zero = new();
                zero.eulerAngles = Vector3.zero;
                GameObject tileObj = Object.Instantiate(Resources.Load("wall tile") as GameObject, new Vector3((x+1) / 1.05f, (along.x+1) / 1.05f), zero, parent.transform);
                tileObj.GetComponent<Tile>().gridPos = new Vector2(x, along.x);
                map[x+1, along.x+1] = tileObj;
            }
        }

        return ret;

    }


}
