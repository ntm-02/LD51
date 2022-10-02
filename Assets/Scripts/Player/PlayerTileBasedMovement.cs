using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTileBasedMovement : MonoBehaviour
{
    //PlayerTime timeObj = new PlayerTime();
    private bool isMoving = false;
    private Vector3 origPos;
    private Vector3 newPos;
    private float timeToMove = 0.2f;
    private GameObject[,] grid;
    private static Vector2 gridPosition = GameManager.PlayerGridPos;


    public Vector2 getGridPos()
    {
        return gridPosition;
    }

    public Vector2 getWorldPos()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    public static void setGridPos(Vector2 newGridPos)
    {
        gridPosition = newGridPos;
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    public void moveUp()
    {
        StartCoroutine(MovePlayer(grid[(int)gridPosition.x, (int)gridPosition.y + 1].transform.position - transform.position));
        gridPosition += Vector2.up;
    }

    public void moveRight()
    {
        StartCoroutine(MovePlayer(grid[(int)gridPosition.x + 1, (int)gridPosition.y].transform.position - transform.position));
        gridPosition += Vector2.right;
    }
    public void moveLeft()
    {
        StartCoroutine(MovePlayer(grid[(int)gridPosition.x - 1, (int)gridPosition.y].transform.position - transform.position));
        gridPosition += Vector2.left;
    }
    public void moveDown()
    {
        StartCoroutine(MovePlayer(grid[(int)gridPosition.x, (int)gridPosition.y - 1].transform.position - transform.position));
        gridPosition += Vector2.down;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (PlayerTime.currPlayerTime == 0f) // remove this for final game, grants infinite moves by reseting time
        {
            PlayerTime.ResetTime();
        }*/
        grid = FindObjectOfType<TilePathFinding>().getGrid();
        GameManager.PlayerGridPos = gridPosition; // updating the game manager's player grid position

    }
    public IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        float elapsedTime = 0;
        origPos = transform.position;
        newPos = origPos + (direction);

        while(elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, newPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // if we don't land exactly in the middle of a tile, this will make sure you end up there
        transform.position = newPos;
        isMoving = false;
    }
}
