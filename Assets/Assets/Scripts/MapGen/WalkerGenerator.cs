using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[System.Serializable]
public class PrefabWithRate
{
    public GameObject prefab;
    public float spawnRate;
    public int maxAppearing;
}

public class WalkerGenerator : MonoBehaviour
{
    
    public enum Grid
    {
        FLOOR,
        WALL,
        EMPTY,
        ENVIRONMENT,
        SPAWNED
    }

    //Variables
    public Grid[,] gridHandler;
    public List<WalkerObject> Walkers;
    [Header("Map and enviroment")]
    public Tilemap tileMapFloor;
    public Tilemap tileMapWall;
    public Tilemap tileMapBackground;
    public Tile Floor;
    public List<PrefabWithRate> TilePrefabsWithRates;
    public Tile Wall;
    public Tile Background;
    
    [Header("Map Properties")]
    public int MapWidth = 30;
    public int MapHeight = 30;
    public int MaximumWalkers = 10;
    public int TileCount = default;
    public float FillPercentage = 0.4f;
    public float WaitTime = 0.05f;

    private List<Vector3Int> validPos = new List<Vector3Int>();
    [SerializeField] GameObject deathSlime;

    void Awake()
    {
        
        InitializeGrid();
        PlaceTilePrefabs();
        GetValidPos();
        // SpawnGameObject(deathSlime, 5);
    }
    public void InitializeGrid()
    {
        gridHandler = new Grid[MapWidth, MapHeight];
        
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        float cameraHeight = 2f * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Screen.width / Screen.height;
        cameraFollow.SetBounds(MapWidth, MapHeight, cameraWidth, cameraHeight);
        
        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                gridHandler[x, y] = Grid.EMPTY;
                tileMapBackground.SetTile(new Vector3Int(x, y, 0), Background);
            }
        }

        Walkers = new List<WalkerObject>();

        Vector3Int TileCenter = new Vector3Int(gridHandler.GetLength(0) / 2, gridHandler.GetLength(1) / 2, 0);

        WalkerObject curWalker = new WalkerObject(new Vector2(TileCenter.x, TileCenter.y), GetDirection(), 0.5f);
        gridHandler[TileCenter.x, TileCenter.y] = Grid.FLOOR;
        tileMapFloor.SetTile(TileCenter, Floor);
        Walkers.Add(curWalker);

        TileCount++;

        CreateFloors();
    }

    Vector2 GetDirection()
    {
        int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);

        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            case 3:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    void CreateFloors()
    {
        while ((float)TileCount / (float)gridHandler.Length < FillPercentage)
        {
            bool hasCreatedFloor = false;
            foreach (WalkerObject curWalker in Walkers)
            {
                Vector3Int curPos = new Vector3Int((int)curWalker.Position.x, (int)curWalker.Position.y, 0);

                if (gridHandler[curPos.x, curPos.y] != Grid.FLOOR)
                {
                    tileMapFloor.SetTile(curPos, Floor);
                    TileCount++;
                    gridHandler[curPos.x, curPos.y] = Grid.FLOOR;
                }
            }

            //Walker Methods
            ChanceToRemove();
            ChanceToRedirect();
            ChanceToCreate();
            UpdatePosition();
            
        }

        CreateWalls();
    }

    void ChanceToRemove()
    {
        int updatedCount = Walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count > 1)
            {
                Walkers.RemoveAt(i);
                break;
            }
        }
    }

    void ChanceToRedirect()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange)
            {
                WalkerObject curWalker = Walkers[i];
                curWalker.Direction = GetDirection();
                Walkers[i] = curWalker;
            }
        }
    }

    void ChanceToCreate()
    {
        int updatedCount = Walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count < MaximumWalkers)
            {
                Vector2 newDirection = GetDirection();
                Vector2 newPosition = Walkers[i].Position;

                WalkerObject newWalker = new WalkerObject(newPosition, newDirection, 0.5f);
                Walkers.Add(newWalker);
            }
        }
    }

    void UpdatePosition()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            WalkerObject FoundWalker = Walkers[i];
            FoundWalker.Position += FoundWalker.Direction;
            FoundWalker.Position.x = Mathf.Clamp(FoundWalker.Position.x, 1, gridHandler.GetLength(0) - 2);
            FoundWalker.Position.y = Mathf.Clamp(FoundWalker.Position.y, 1, gridHandler.GetLength(1) - 2);
            Walkers[i] = FoundWalker;
        }
    }

    void CreateWalls()
    {
        
        for (int x = 0; x < gridHandler.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1) - 1; y++)
            {
                if (gridHandler[x, y] == Grid.FLOOR)
                {

                    if (gridHandler[x + 1, y] == Grid.EMPTY)
                    {
                        tileMapWall.SetTile(new Vector3Int(x + 1, y, 0), Wall);
                        gridHandler[x + 1, y] = Grid.WALL;
                    }
                    if (gridHandler[x - 1, y] == Grid.EMPTY)
                    {
                        tileMapWall.SetTile(new Vector3Int(x - 1, y, 0), Wall);
                        gridHandler[x - 1, y] = Grid.WALL;

                    }
                    if (gridHandler[x, y + 1] == Grid.EMPTY)
                    {
                        tileMapWall.SetTile(new Vector3Int(x, y + 1, 0), Wall);
                        gridHandler[x, y + 1] = Grid.WALL;

                    }
                    if (gridHandler[x, y - 1] == Grid.EMPTY)
                    {
                        tileMapWall.SetTile(new Vector3Int(x, y - 1, 0), Wall);
                        gridHandler[x, y - 1] = Grid.WALL;

                    }
                    
                }
            }
        }
        
    }

    void PlaceTilePrefabs()
    {
        foreach (PrefabWithRate prefabWithRate in TilePrefabsWithRates)
        {
            int numberOfPrefabs = prefabWithRate.maxAppearing;
            for (int i = 0; i < numberOfPrefabs; i++)
            {
                float spawnChance = prefabWithRate.spawnRate;
                
                if (Random.value <= spawnChance)
                {
                    Vector3Int randomPosition = GetRandomPositionForPrefab(prefabWithRate.prefab);
                
                    if (randomPosition != Vector3Int.zero)
                    {
                        Instantiate(prefabWithRate.prefab, randomPosition, Quaternion.identity);
                        MarkGridAsOccupied(prefabWithRate.prefab, randomPosition);
                    }
                }
            }
        }
    }

    Vector3Int GetRandomPositionForPrefab(GameObject prefab)
    {
        Tilemap tilemap = prefab.GetComponentInChildren<Tilemap>();
        
        BoundsInt bounds = tilemap.cellBounds;
        int prefabWidth = bounds.size.x;
        int prefabHeight = bounds.size.y;
        
        for (int attempt = 0; attempt < 10; attempt++)
        {
            int randomX = Random.Range(0, MapWidth - prefabWidth);
            int randomY = Random.Range(0, MapHeight - prefabHeight);
            Vector3Int position = new Vector3Int(randomX, randomY, 0);

            if (IsAreaEmpty(position, prefabWidth, prefabHeight))
            {
                return position;
            }
        }

        return Vector3Int.zero;
    }

    bool IsAreaEmpty(Vector3Int position, int width, int height)
    {
        for (int x = position.x; x < position.x + width; x++)
        {
            for (int y = position.y; y < position.y + height; y++)
            {
                if (gridHandler[x, y] != Grid.FLOOR)
                    return false;
            }
        }
        return true;
    }

    void MarkGridAsOccupied(GameObject prefab, Vector3Int position)
    {
        Tilemap tilemap = prefab.GetComponentInChildren<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        int prefabWidth = bounds.size.x;
        int prefabHeight = bounds.size.y;

        for (int x = position.x; x < position.x + prefabWidth; x++)
        {
            for (int y = position.y; y < position.y + prefabHeight; y++)
            {
                gridHandler[x, y] = Grid.ENVIRONMENT;
            }
        }
    }

    void GetValidPos()
    {
        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                
                if (gridHandler[i, j] == Grid.FLOOR)
                {
                    validPos.Add(new Vector3Int(i, j, 0));
                }
            }
        }
    }

    public void SpawnGameObject(GameObject gameObject, int numberOfEnemies)
    {
        Debug.Log("Size of validPos" + validPos.Count);
        numberOfEnemies = Math.Min(numberOfEnemies, validPos.Count - 10);
        if (gameObject == null)
        {
            Debug.LogError("Cannot spawn: GameObject is null!");
            return;
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            int randomIndex = Random.Range(0, validPos.Count);
            Debug.Log(randomIndex);
            Vector3Int spawnPosition = validPos[randomIndex];
            Instantiate(gameObject, spawnPosition, Quaternion.identity);
            gridHandler[spawnPosition.x, spawnPosition.y] = Grid.SPAWNED;
            validPos.RemoveAt(randomIndex);
        }
    }
    
    
}
