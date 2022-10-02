using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnTile : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    GameObject playerTile;
    List<GameObject> path;
    Vector2 playerPos = Vector2.zero;

    private bool mouseHovering = false;
    /*public void GotToTile()
    {
        StartCoroutine(FindObjectOfType<TileBasedMovement>().MovePlayer(Vector3.up));
    }*/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //playerPos = FindObjectOfType<TileBasedMovement>().getGridPos();
        if (mouseHovering && !GameManager.IsPlayerMoving)
        {
            GeneratePathTrail();
        }
    }

    private void GeneratePathTrail()
    {
        // clears the previous path trail
        FindObjectOfType<TilePathFinding>().clearPathTrail();

        // finding the tile with player on it
        //playerPos = FindObjectOfType<PlayerTileBasedMovement>().getGridPos();
        playerPos = GameManager.PlayerGridPos;
        // generating the new path with playerPos as start and the tile that was hovered over as the end
        path = TilePathFinding.FindShortestPath(FindObjectOfType<TilePathFinding>().getGrid(), playerPos, transform.parent.parent.GetComponent<Tile>().gridPos);

        float timePassed = 0f;
        //PlayerTime.ResetTime();  // reset player time every time we move the pointer/mouse NOTE:This could be done with it's own local time calculation, so we don't affect the player's actual time
        foreach (GameObject g in path)
        {
            timePassed += 1f;
            // if the path is within the player's allotted time, display as gray tinted tiles
            if (PlayerTime.CheckDecrease(timePassed)) 
            {
                g.GetComponent<SpriteRenderer>().color = Color.gray;
            }
            // else display as red tinted tiles
            else
            {
                g.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

   
    // on mouse hover, this will generate a path to the destination
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GameManager.IsPlayerMoving)
        {
            GeneratePathTrail();
        }
        mouseHovering = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameManager.IsPlayerMoving)
        {
            StartCoroutine(MouseClickCoroutine());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseHovering = false;
    }

    public IEnumerator MouseClickCoroutine()
    {
        if (!GameManager.IsPlayerMoving)  // probably unneccessary, this is checked in the method that calls it
        {
            foreach (GameObject g in path)
            {
                // if the next tile is red stop moving--ran out of time
                if (g.GetComponent<SpriteRenderer>().color == Color.red)
                {
                    break;
                }
                // player is currently moving
                GameManager.IsPlayerMoving = true;
                Vector2 playerPos = FindObjectOfType<PlayerTileBasedMovement>().getWorldPos();
                print(playerPos);

                if (g.transform.position.y > playerPos.y)
                {
                    FindObjectOfType<PlayerTileBasedMovement>().moveUp();
                }
                if (g.transform.position.y < playerPos.y)
                {
                    FindObjectOfType<PlayerTileBasedMovement>().moveDown();
                }
                if (g.transform.position.x > playerPos.x)
                {
                    FindObjectOfType<PlayerTileBasedMovement>().moveRight();
                }
                if (g.transform.position.x < playerPos.x)
                {
                    FindObjectOfType<PlayerTileBasedMovement>().moveLeft();
                }
                yield return new WaitForSeconds(0.5f); // wait one second before moving the player
            }
            // player no longer moving
            GameManager.IsPlayerMoving = false;

        }
    }

}
