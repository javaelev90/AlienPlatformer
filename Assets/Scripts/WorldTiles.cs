using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTiles : MonoBehaviour
{
    public static WorldTiles _instance;
    public Tilemap tileMapGround;
    public Tilemap tileMapHazards;
    
    public Dictionary<Vector3, TileMapTile> worldTiles;
    public TileMapTile[,] map;

    public ConcurrentDictionary<int, GameObject> enemies;

    public static WorldTiles Instance
    {
        get {
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        } else
        {
            _instance = this;
        }

        CreateWorldMap();
    }

    private void OnDestroy()
    {
        if(this == _instance)
        {
            _instance = null;
        }
    }

    private void CreateWorldMap()
    {
        tileMapGround.CompressBounds();
        worldTiles = new Dictionary<Vector3, TileMapTile>();
        map = new TileMapTile[tileMapGround.cellBounds.size.x, tileMapGround.cellBounds.size.y];

        loadTiles(tileMapGround, "tile");
        loadTiles(tileMapHazards, "hazard");

        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        enemies = new ConcurrentDictionary<int, GameObject>();
        foreach(GameObject enemy in enemyArray)
        {
            enemies.TryAdd(enemy.GetInstanceID(), enemy);
        }
    }

    private void loadTiles(Tilemap tileMap, string tileName)
    {
        foreach (Vector3Int tilePosistion in tileMap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(tilePosistion.x, tilePosistion.y, tilePosistion.z);

            //If this position is empty we skip it
            if (!tileMap.HasTile(localPlace)) continue;

            TileMapTile worldTile = new TileMapTile
            {
                localPlace = localPlace,
                worldLocation = tileMap.CellToWorld(localPlace),
                tileBase = tileMap.GetTile(localPlace),
                parentTileMap = tileMap,
                miniMapObject = new MiniMapObject
                {
                    visibleOnMinimap = false,
                    paintedOnMinimap = false,
                    objectName = tileName
                }
            };
            worldTiles.Add(worldTile.worldLocation, worldTile);
        }
    }
}
