using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// credit : https://www.youtube.com/watch?v=iD1_JczQcFY&t=1004s&ab_channel=CodeMonkey 

public class MoveIndicator : MonoBehaviour
{
    [SerializeField] private Transform pfMoveIndicator;
    private Transform indicator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Spawns the Popup
    public static MoveIndicator Create(Vector3 position)
    {
        Transform moveIndicatorTransform = Instantiate(GameAssetsContainer.i.pfMoveIndicator, position, Quaternion.identity);
        MoveIndicator moveIndicator = moveIndicatorTransform.GetComponent<MoveIndicator>();
        moveIndicator.Setup();
        return moveIndicator;
    }

    // Decides if to do white or red
    public void Setup()
    {
        // if enemy is in tile swap animation to red
    }

    public void SpawnIndicator()
    {
        //Debug.Log("Is null? : " + transform.position);
        Vector3 createPos = new Vector3 (transform.position.x, (transform.position.y + 0.75f), transform.position.z);
        indicator = Instantiate(pfMoveIndicator, createPos, Quaternion.identity);
    }

    public void DestroyIndicator()
    {
        Destroy(indicator.gameObject);
    }
}
