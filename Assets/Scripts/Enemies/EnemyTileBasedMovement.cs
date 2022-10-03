using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTileBasedMovement : MonoBehaviour
{
    private bool isMoving = false;
    private Vector3 origPos;
    private Vector3 newPos;
    private float timeToMove = 0.2f;
    private GameObject[,] grid;
    private static Vector2 gridPosition = Vector2.zero; // change this later


    private Vector2 getGridPos()
    {
        return gridPosition;
    }

    private Vector2 getWorldPos()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    private static void setGridPos(Vector2 newGridPos)
    {
        gridPosition = newGridPos;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {

        yield return new WaitUntil(() => { grid = FindObjectOfType<TilePathFinding>().getGrid(); return grid != null; });
        gridPosition = grid[1, 1].transform.position;       // sets the gridPosition to [1,1] initially
    }


    public void moveUp()
    {
        StartCoroutine(MoveEnemy(grid[(int)gridPosition.x, (int)gridPosition.y + 1].transform.position - transform.position));
        gridPosition += Vector2.up;
    }

    public void moveRight()
    {
        StartCoroutine(MoveEnemy(grid[(int)gridPosition.x + 1, (int)gridPosition.y].transform.position - transform.position));
        gridPosition += Vector2.right;
    }
    public void moveLeft()
    {
        StartCoroutine(MoveEnemy(grid[(int)gridPosition.x - 1, (int)gridPosition.y].transform.position - transform.position));
        gridPosition += Vector2.left;
    }
    public void moveDown()
    {
        StartCoroutine(MoveEnemy(grid[(int)gridPosition.x, (int)gridPosition.y - 1].transform.position - transform.position));
        gridPosition += Vector2.down;
    }

    // Update is called once per frame
    void Update()
    {
        // press space to reset player time to full
        if (Input.GetButtonDown("Jump")) // remove this for final game, grants infinite moves by reseting time
        {
            PlayerTime.ResetTime();
        }
        //grid = FindObjectOfType<TilePathFinding>().getGrid();
        //GameManager.EnemyGridPos = gridPosition; // updating the game manager's player grid position

    }
    public IEnumerator MoveEnemy(Vector3 direction)
    {
        // decreases player time by 1 second for each tile moved
        PlayerTime.DecreaseTime(1f);
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
