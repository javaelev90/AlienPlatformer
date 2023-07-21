using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapTile : IMiniMapObject
{
    public Vector3Int localPlace;
    public Vector3 worldLocation;
    public TileBase tileBase;
    public Tilemap parentTileMap;
    public MiniMapObject miniMapObject { get; set; }

    public MiniMapObject GetMiniMapObject()
    {
        return miniMapObject;
    }
    // Start is called before the first frame update

}
