using System.Collections.Generic;
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
        endTileGameObject = GameManager.EndingTile.transform.gameObject;

        optimalPlayerPath = tilePathFinding.FindShortestPath(GameManager.PlayerGridPos, endTileGameObject.transform.position);
        GameObject[,] grid = FindObjectOfType<TilePathFinding>().getGrid();

        foreach (GameObject tile in grid)
        {
            Debug.Log("entered gameobject loop for non-path tiles");
            if (!optimalPlayerPath.Contains(tile))
            {
                InstantiateRandomEnemy(tile);
                Debug.Log("Instantiated non-path enemy");
            }
        }

        foreach (GameObject tile in optimalPlayerPath)
        {
            GameObject[] playerPathAdjacentTiles = TilePathFinding.adjacentToPoint(grid, tile.gameObject.transform.position);
            InstantiateRandomEnemy(playerPathAdjacentTiles);
            Debug.Log("instantiated path enemy");
        }


        // calculate the shortest path from the player to the exit
        // along that path, randomly spawn enemies
        // get where the player spawns
        // get where the exit is
        // set an offset bias to populate enemies around the path
    }

    private void InstantiateRandomEnemy(GameObject tile)
    {
        int randomVal = UnityEngine.Random.Range(0, 20);
        if (randomVal == 1)
        {
            Instantiate(enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)], tile.transform.position, Quaternion.identity);
        }
    }

    private void InstantiateRandomEnemy(GameObject[] playerPathAdjacentTiles)
    {
        int randomAdjacentTileIndex = UnityEngine.Random.Range(0, 4);
        GameObject selectedPlayerPathAdjacentTile = null;

        selectedPlayerPathAdjacentTile = playerPathAdjacentTiles[randomAdjacentTileIndex];

        if (selectedPlayerPathAdjacentTile is null) InstantiateRandomEnemy(playerPathAdjacentTiles); // if the tile is null run the method again to get a new random tile index

        int randomVal = UnityEngine.Random.Range(0, 2);
        if (randomVal > 0)
        {
            Instantiate(enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)], selectedPlayerPathAdjacentTile.transform.position, Quaternion.identity);
        }
    }

    private List<GameObject> FetchPrefabs()
    {
        return new List<GameObject>()
        {
            // implement other enemies that have a load prefab method
            // could make an interface like ISpawnable or something for spawning stuff other than enemies
            gameObject.AddComponent<SlimeEnemySpawner>().LoadPrefab(),
            gameObject.AddComponent<SlimeEnemySpawner>().LoadPrefab(),
            gameObject.AddComponent<SlimeEnemySpawner>().LoadPrefab(),
            gameObject.AddComponent<SlimeEnemySpawner>().LoadPrefab(),
            gameObject.AddComponent<SlimeEnemySpawner>().LoadPrefab(),
            gameObject.AddComponent<SlimeEnemySpawner>().LoadPrefab(),
        };
    }

    private void Update()
    {

    }
}