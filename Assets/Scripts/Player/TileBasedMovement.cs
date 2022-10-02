using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBasedMovement : MonoBehaviour
{
    //PlayerTime timeObj = new PlayerTime();
    private bool isMoving = false;
    private Vector3 origPos;
    private Vector3 newPos;
    private float timeToMove = 0.2f;
    private GameObject[,] grid;
    private Vector2 gridPosition = Vector2.zero;



    public Vector2 getGridPos()
    {
        return gridPosition;
    }

    public Vector2 getWorldPos()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        //grid = FindObjectOfType<TilePathFinding>().getGrid();
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
        grid = FindObjectOfType<TilePathFinding>().getGrid();
        if (Input.GetKeyDown(KeyCode.G))
        {
            grid = FindObjectOfType<TilePathFinding>().getGrid();
            this.transform.position = grid[0, 0].transform.position;
        }
        if (Input.GetKey(KeyCode.W) && !isMoving && (PlayerTime.DecreaseTime(1f) != -1f))
        {
            StartCoroutine(MovePlayer(grid[(int)gridPosition.x, (int)gridPosition.y + 1].transform.position - transform.position));
            gridPosition += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A) && !isMoving && (PlayerTime.DecreaseTime(1f) != -1f))
        {
            StartCoroutine(MovePlayer(grid[(int)gridPosition.x - 1, (int)gridPosition.y].transform.position - transform.position));
            gridPosition += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S) && !isMoving && (PlayerTime.DecreaseTime(1f) != -1f))
        {
            StartCoroutine(MovePlayer(grid[(int)gridPosition.x, (int)gridPosition.y - 1].transform.position - transform.position));
            gridPosition += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D) && !isMoving && (PlayerTime.DecreaseTime(1f) != -1f))
        {
            StartCoroutine(MovePlayer(grid[(int)gridPosition.x + 1, (int)gridPosition.y].transform.position - transform.position));
            gridPosition += Vector2.right;
        }

        if (PlayerTime.currPlayerTime == 0f) // remove this for final game, grants infinite moves by reseting time
        {
            PlayerTime.ResetTime();
        }
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
