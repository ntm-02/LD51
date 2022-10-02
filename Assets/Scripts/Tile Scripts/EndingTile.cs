using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.EndingTile = gameObject.GetComponent<Tile>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
