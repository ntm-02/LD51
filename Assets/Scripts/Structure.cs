using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Structure : MonoBehaviour
{
    GameObject[,] resolvedStructure;

    public Rect bounds;


    public void Start()
    {
        bounds = new Rect(0, 0, Enumerable.Max(from Transform trans in transform select trans, (trans) => trans.childCount), transform.childCount);
        resolvedStructure = new GameObject[(int)bounds.width, (int)bounds.height];

        //print(bounds.height);//4
        //print(bounds.width);//3

        for (int x = 0; x < bounds.height; x++)
        {
            //print(transform.GetChild(x).name);
            for (int y = 0; y < transform.GetChild(x).childCount; y++)
            {
                //print("w");
                GameObject inputting = transform.GetChild(x).GetChild(y).gameObject;
                //print(x+" "+y+" "+inputting.name);
                resolvedStructure[(int)inputting.transform.localPosition.x, (int)inputting.transform.localPosition.y] = inputting;
            }
        }


    }

    public bool doesFitAtPoint(GameObject[,] grid, Vector2 checkPos, bool waterOK) {

        List<Tile> tiles = new();

        for (int x = 0; x < bounds.width; x++) {
            for (int y = 0; y < bounds.height; y++) {
                if ((int)checkPos.x + x > grid.GetLength(0)-1 || (int)checkPos.y + y > grid.GetLength(1)-1)
                {
                    return false;
                }
                else
                {
                    tiles.Add(grid[(int)checkPos.x + x, (int)checkPos.y + y].GetComponent<Tile>());
                }
            }
        }

        return !Enumerable.Any(tiles,(tile)=> tile.type == Tile.TileType.mountain || (!waterOK && tile.type == Tile.TileType.water));
    }


    public GameObject[,] replaceStructTiles(GameObject[,] grid, Vector2 placePos) {

        for (int x = 0; x < bounds.width; x++)
        {
            for (int y = 0; y < bounds.height; y++)
            {
                if (resolvedStructure[x, y] != null)
                {
                    Transform origTrans = grid[(int)placePos.x + x, (int)placePos.y + y].transform;
                    Vector3 pos = origTrans.position;
                    Quaternion rot = origTrans.rotation;
                    Transform parent = origTrans.parent;
                    Destroy(grid[(int)placePos.x + x, (int)placePos.y + y]);

                    GameObject tileObj = Instantiate(resolvedStructure[x, y], pos, rot, parent);
                    tileObj.GetComponent<Tile>().gridPos = new Vector2(placePos.x + x, placePos.y + y);
                    //print(tileObj.name + " " + tileObj.GetComponent<Tile>().gridPos);
                    grid[(int)placePos.x + x, (int)placePos.y + y] = tileObj;
                }
            }
        }

        return grid;

    }

}
