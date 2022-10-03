using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveIndicator : MonoBehaviour
{
    [SerializeField] private Transform pfMoveIndicator;
    private Transform indicator;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Decides if to do white or red
    public void Setup(Transform indicator)
    {
        // if enemy is in tile swap animation to red\
        Animator anim = indicator.gameObject.GetComponent<Animator>();
        // if condition
        Tile tileScript = GetComponent<Tile>(); 
        if (tileScript == null)
        {
            Debug.Log("tile.cs not found in parent of parent of the tile button");
        }
        else
        {
            if (tileScript.hasEnemy)
            {
                anim.SetTrigger("AttackIndicator");
            }
            else
            {
                anim.SetTrigger("MoveIndicator");
            }
        }
    }

    public void SpawnIndicator()
    {
        //Debug.Log("Is null? : " + transform.position);
        Vector3 createPos = new Vector3 (transform.position.x, (transform.position.y + 0.8f), transform.position.z);
        indicator = Instantiate(pfMoveIndicator, createPos, Quaternion.identity);
        Setup(indicator);
    }

    public void DestroyIndicator()
    {
        Destroy(indicator.gameObject);
    }
}
