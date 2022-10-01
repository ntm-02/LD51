using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOnTile : MonoBehaviour
{
    private int mark = 0;
    public void GotToTile()
    {
        StartCoroutine(FindObjectOfType<TileBasedMovement>().MovePlayer(Vector3.up));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void incrementMark()
    {
        mark++;
    }
}
