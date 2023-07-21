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
        foreach (Vector3Int tilePosistion in tileMapGround.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(tilePosistion.x, tilePosistion.y, tilePosistion.z);

            //If this position is empty we skip it
            if (!tileMapGround.HasTile(localPlace)) continue;

            TileMapTile worldTile = new TileMapTile
            {
                localPlace = localPlace,
                worldLocation = tileMapGround.CellToWorld(localPlace),
                tileBase = tileMapGround.GetTile(localPlace),
                parentTileMap = tileMapGround,
                miniMapObject = new MiniMapObject {
                    visibleOnMinimap = false,
                    paintedOnMinimap = false,
                    objectName = "tile"
                }
            };
            worldTiles.Add(worldTile.worldLocation, worldTile);

        }

        foreach (Vector3Int tilePosistion in tileMapHazards.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(tilePosistion.x, tilePosistion.y, tilePosistion.z);

            //If this position is empty we skip it
            if (!tileMapHazards.HasTile(localPlace)) continue;

            TileMapTile worldTile = new TileMapTile
            {
                localPlace = localPlace,
                worldLocation = tileMapHazards.CellToWorld(localPlace),
                tileBase = tileMapHazards.GetTile(localPlace),
                parentTileMap = tileMapHazards,
                miniMapObject = new MiniMapObject {
                    visibleOnMinimap = false,
                    paintedOnMinimap = false,
                    objectName = "hazard"
                }
            };
            worldTiles.Add(worldTile.worldLocation, worldTile);

        }

        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        enemies = new ConcurrentDictionary<int, GameObject>();
        foreach(GameObject enemy in enemyArray)
        {
            enemies.TryAdd(enemy.GetInstanceID(), enemy);
        }
        //for (int x = tileMap.origin.x, i = 0; i < (tileMap.size.x); x++, i++)
        //{
        //    for (int y = tileMap.origin.y, j = 0; j < (tileMap.size.y); y++, j++)
        //    {
        //        Vector3Int tilePosition = new Vector3Int(x, y, 0);

        //        if (!tileMap.HasTile(tilePosition)) continue;

        //        var worldTile = new TileMapTile
        //        {
        //            localPlace = tilePosition,
        //            worldLocation = tileMap.CellToWorld(tilePosition),
        //            tileBase = tileMap.GetTile(tilePosition),
        //            parentTileMap = tileMap,
        //            visibleOnMinimap = false,
        //            paintedOnMinimap = false,
        //            name = "tile"
        //        };

        //        map[i, j] = worldTile;

        //    }

        //}

    }
}
