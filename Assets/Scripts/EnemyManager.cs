﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    List<GameObject> optimalPlayerPath;
    List<GameObject> enemyTypes;
    TilePathFinding tilePathFinding;

    GameObject endTileGameObject;

    IEnumerator Start()
    {
        tilePathFinding = FindObjectOfType<TilePathFinding>();
        enemyTypes = FetchPrefabs();
        // wait an arbitrary amount of time so that the map exists when we try to access it
        yield return new WaitForSeconds(0.5f);
        endTileGameObject = tilePathFinding.getGrid()[10, 10];

        optimalPlayerPath = tilePathFinding.FindShortestPath(GameManager.PlayerGridPos, endTileGameObject.transform.position);

        foreach (GameObject tile in optimalPlayerPath)
        {
            GameObject[] playerPathAdjacentTiles = TilePathFinding.adjacentToPoint(FindObjectOfType<TilePathFinding>().getGrid(), tile.gameObject.transform.position);
            InstantiateRandomEnemy(playerPathAdjacentTiles);
        }

        // calculate the shortest path from the player to the exit
        // along that path, randomly spawn enemies
        // get where the player spawns
        // get where the exit is
        // set an offset bias to populate enemies around the path
    }

    private void InstantiateRandomEnemy(GameObject[] playerPathAdjacentTiles)
    {
        int randomAdjacentTileIndex = UnityEngine.Random.Range(0, 4);
        GameObject selectedPlayerPathAdjacentTile = null;
        
        Debug.Log($"playerPathAdjacentTiles Length: {playerPathAdjacentTiles.Length}");
        Debug.Log($"Selected random adjacent tile index: {randomAdjacentTileIndex}");
        
        try
        {
            selectedPlayerPathAdjacentTile = playerPathAdjacentTiles[randomAdjacentTileIndex];

        }
        catch (IndexOutOfRangeException ex)
        {
            // if the tile doesn't exist, just run the method again to get a new random index
            Debug.Log($"Index was out of bounds: {randomAdjacentTileIndex}");
            InstantiateRandomEnemy(playerPathAdjacentTiles);
        }



        int randomVal = UnityEngine.Random.Range(0, 2);
        Debug.Log($"Random value: {randomVal}");
        if (randomVal > 0)
        {
            Debug.Log("instantiating enemy");
            // do a better job of randomizing this

            Instantiate(enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)], selectedPlayerPathAdjacentTile.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("not instantiating enemy");
        }
    }

    private List<GameObject> FetchPrefabs()
    {
        return new List<GameObject>()
        {
            // implement other enemies that have a load prefab method
            // could make an interface like ISpawnable or something for spawning stuff 
            gameObject.AddComponent<SlimeEnemy>().LoadPrefab(),
            gameObject.AddComponent<SlimeEnemy>().LoadPrefab(),
            gameObject.AddComponent<SlimeEnemy>().LoadPrefab(),
            gameObject.AddComponent<SlimeEnemy>().LoadPrefab(),
            gameObject.AddComponent<SlimeEnemy>().LoadPrefab(),
            gameObject.AddComponent<SlimeEnemy>().LoadPrefab(),
        };
    }

    private void Update()
    {

    }
}