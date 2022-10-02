using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemySpawner : MonoBehaviour, IKillable 
{
    public void Die()
    {
        Destroy(gameObject);
    }

    public GameObject LoadPrefab()
    {
        return Resources.Load("Slime Enemy") as GameObject;
    }

    public void NotifyDamage()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Instantiate(Resources.Load("Slime Enemy") as GameObject, Vector3.zero, Quaternion.identity);
    }

    // calculate the shortest path from the player to the exit
    // along that path, randomly spawn enemies
    // get where the player spawns
    // get where the exit is
    // set an offset bias to populate enemies around the path
}
