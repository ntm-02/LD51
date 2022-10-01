using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnTile : MonoBehaviour, IPointerEnterHandler
{
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
        
    }

   
    // on mouse hover, this will generate a path to the destination
    public void OnPointerEnter(PointerEventData eventData)
    {
        // clears the previous path trail
        FindObjectOfType<TilePathFinding>().clearPathTrail();
        // generating the new path
        List<GameObject> path = FindObjectOfType<TilePathFinding>().FindShortestPath(Vector2.zero, transform.parent.parent.GetComponent<Tile>().gridPos);
        foreach (GameObject g in path)
        {
            g.GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }
}
