using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnTile : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    GameObject playerTile;
    List<GameObject> path;
    Vector2 playerPos = Vector2.zero;
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
    }

   
    // on mouse hover, this will generate a path to the destination
    public void OnPointerEnter(PointerEventData eventData)
    {
        // clears the previous path trail
        FindObjectOfType<TilePathFinding>().clearPathTrail();

        // finding the tile with player on it
        //GameObject[,] tileGrid = FindObjectOfType<TilePathFinding>().getGrid();

        playerPos = FindObjectOfType<PlayerTileBasedMovement>().getGridPos();
        //print(playerPos);
        // generating the new path
        path = FindObjectOfType<TilePathFinding>().FindShortestPath(playerPos, transform.parent.parent.GetComponent<Tile>().gridPos);
        foreach (GameObject g in path)
        {
            
            g.GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }



    public IEnumerator MouseClickCoroutine()
    {
        if (!GameManager.IsPlayerMoving)
        {
            foreach (GameObject g in path)
            {
                // player is currently moving
                GameManager.IsPlayerMoving = true;
                Vector2 playerPos = FindObjectOfType<PlayerTileBasedMovement>().getWorldPos();
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
                yield return new WaitForSeconds(1f);  // wait one second before moving the player again
            }
            GameManager.IsPlayerMoving = false;
        }
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(MouseClickCoroutine());
    }

   

}
