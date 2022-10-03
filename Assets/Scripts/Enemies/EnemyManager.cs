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
        yield return new WaitUntil(() => tilePathFinding.getGrid() != null);
        endTileGameObject = GameManager.EndingTile.transform.gameObject;
        GameObject[,] grid = tilePathFinding.getGrid();
        //print(grid);
        //print(GameManager.PlayerGridPos);
        //print(endTileGameObject);
        optimalPlayerPath = TilePathFinding.FindShortestPath(grid, GameManager.PlayerGridPos, endTileGameObject.GetComponent<Tile>().gridPos);
        
        // loops through the grid and randomly spawns an enemy in a tile's location
        /*foreach (GameObject tile in grid)
        {
            Debug.Log("entered gameobject loop for non-path tiles");
            if (!optimalPlayerPath.Contains(tile))
            {
                InstantiateRandomEnemy(tile);
                
                //InstantiateRandomEnemy(tile).TryGetComponent<EnemyTileBasedMovement>().SetEnemy;
                Debug.Log("Instantiated non-path enemy");
            }
        }*/

        for (int i = 0; i < grid.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < grid.GetLength(1) - 1; j++)
            {
                if (!optimalPlayerPath.Contains(grid[i, j]))
                {
                    InstantiateRandomEnemy(grid[i, j], new Vector2(i, j));
                   
                }
            }
        }


        /*foreach (GameObject tile in optimalPlayerPath)
        {
            GameObject[] playerPathAdjacentTiles = TilePathFinding.adjacentToPoint(grid, tile.gameObject.transform.position);
            InstantiateRandomEnemy(playerPathAdjacentTiles);
            //Debug.Log("instantiated path enemy");
        }*/

        for (int i = 0; i < grid.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < grid.GetLength(1) - 1; j++)
            {
                if (optimalPlayerPath.Contains(grid[i, j]))
                {
                    GameObject[] playerPathAdjacentTiles = TilePathFinding.adjacentToPoint(grid, new Vector2(i, j));
                    InstantiateRandomEnemy(playerPathAdjacentTiles, new Vector2(i,j));
                    
                }
            }
        }

        // calculate the shortest path from the player to the exit
        // along that path, randomly spawn enemies
        // get where the player spawns
        // get where the exit is
        // set an offset bias to populate enemies around the path
    }

    private void InstantiateRandomEnemy(GameObject tile, Vector2 gridPos)
    {
        int randomVal = UnityEngine.Random.Range(0, 20);
        if (randomVal == 1)
        {
            GameObject newEnemy = Instantiate(enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)], tile.transform.position, Quaternion.identity, transform);
            newEnemy.GetComponent<EnemyTileBasedMovement>().SetEnemyGridPos(gridPos);
        }
        
    }

    private void InstantiateRandomEnemy(GameObject[] playerPathAdjacentTiles, Vector2 gridPos)
    {
        int randomAdjacentTileIndex = UnityEngine.Random.Range(0, 4);
        GameObject selectedPlayerPathAdjacentTile = null;

        selectedPlayerPathAdjacentTile = playerPathAdjacentTiles[randomAdjacentTileIndex];

        if (selectedPlayerPathAdjacentTile is null) InstantiateRandomEnemy(playerPathAdjacentTiles, gridPos); // if the tile is null run the method again to get a new random tile index

        int randomVal = UnityEngine.Random.Range(0, 40); // was 0, 2
        if (randomVal == 1)
        {
            GameObject newEnemy = Instantiate(enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)], selectedPlayerPathAdjacentTile.transform.position, Quaternion.identity, transform);
            newEnemy.GetComponent<EnemyTileBasedMovement>().SetEnemyGridPos(gridPos);
            //newEnemy.GetComponent<SpriteRenderer>().color = Color.grey;
        }
    }

    private List<GameObject> FetchPrefabs()
    {
        return new List<GameObject>()
        {
            // implement other enemies that have a load prefab method
            // could make an interface like ISpawnable or something for spawning stuff other than enemies
            gameObject.AddComponent<SlimeEnemy>().LoadPrefab()
            /*gameObject.AddComponent<SlimeEnemySpawner>().LoadPrefab(),
            gameObject.AddComponent<SlimeEnemySpawner>().LoadPrefab(),
            gameObject.AddComponent<SlimeEnemySpawner>().LoadPrefab(),
            gameObject.AddComponent<SlimeEnemySpawner>().LoadPrefab(),
            gameObject.AddComponent<SlimeEnemySpawner>().LoadPrefab(),*/
        };
    }

    private void Update()
    {

    }
}