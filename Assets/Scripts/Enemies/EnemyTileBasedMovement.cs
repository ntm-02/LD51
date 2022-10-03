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
    [SerializeField] private Vector2 gridPosition = Vector2.zero; // initializes to 0,0 will get set by public method

    List<GameObject> path;
    private bool enemyTurn = false;


    private Vector2 getGridPos()
    {
        return gridPosition;
    }

    private Vector2 getWorldPos()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(() => { grid = FindObjectOfType<TilePathFinding>().getGrid(); return grid != null; });
        grid = FindObjectOfType<TilePathFinding>().getGrid();
        //print("world position given grid position: " + grid[(int)gridPosition.x, (int)gridPosition.y].transform.position);
        //print("actual world position: " + transform.position);

    }

    public void SetEnemyGridPos(Vector2 pos)
    {
        gridPosition = pos;
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
        // press space to reset enemy time to full
        if (Input.GetButtonDown("Jump")) // remove this for final game, grants infinite moves by reseting time
        {
            EnemyTime.ResetTime();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // toggle enemy turn
            enemyTurn = !enemyTurn;
        }

        if (enemyTurn)            // I think this breaks everything because TilePathFinding is getting used by many scripts at the same time
        {
            // find path to player, and go to the player as far as possible

            // clears the previous path trail
            FindObjectOfType<TilePathFinding>().clearPathTrail();

            // finding the tile with player on it
            Vector2 playerPos = GameManager.PlayerGridPos;
           // print(playerPos);
            //print(gridPosition);

            // generating the new path with enemy gridposition as start and the player as the end
            path = FindObjectOfType<EnemyTilePathFinding>().FindShortestPath(FindObjectOfType<TilePathFinding>().getGrid(), gridPosition, playerPos);

            float timePassed = 0f;
            foreach (GameObject g in path)
            {
                timePassed += 1f;
                // if the path is within the enemy's allotted time, display as gray tinted tiles
                if (EnemyTime.CheckDecrease(timePassed))
                {
                    g.GetComponent<SpriteRenderer>().color = Color.gray;
                }
                // else display as red tinted tiles
                else
                {
                    g.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }

           // StartCoroutine(FollowPath());
            enemyTurn = false;

        }

        IEnumerator FollowPath()
        {
            foreach (GameObject g in path)
            {
                // if the next tile is red stop moving--ran out of time
                if (g.GetComponent<SpriteRenderer>().color == Color.red)
                {
                    break;
                }

                // enemy is currently moving
                Vector2 enemyPos = this.transform.position;

                if (g.transform.position.y > enemyPos.y)
                {
                    FindObjectOfType<EnemyTileBasedMovement>().moveUp();
                }
                if (g.transform.position.y < enemyPos.y)
                {
                    FindObjectOfType<EnemyTileBasedMovement>().moveDown();
                }
                if (g.transform.position.x > enemyPos.x)
                {
                    FindObjectOfType<EnemyTileBasedMovement>().moveRight();
                }
                if (g.transform.position.x < enemyPos.x)
                {
                    FindObjectOfType<EnemyTileBasedMovement>().moveLeft();
                }
                yield return new WaitForSeconds(0.5f); // wait before moving the enemy
            }
        }

    }
    public IEnumerator MoveEnemy(Vector3 direction)
    {
        // decreases player time by 1 second for each tile moved
        EnemyTime.DecreaseTime(0.1f);
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
