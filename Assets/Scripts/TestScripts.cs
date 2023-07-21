using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestScripts : MonoBehaviour
{

	private Camera currentCamera;
	private TileMapTile _tile;

    private void Start()
    {
		currentCamera = GameObject.FindWithTag("SubCamera").GetComponent<Camera>();

	}

    // Update is called once per frame
    private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 point = currentCamera.ScreenToWorldPoint(Input.mousePosition);
			var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);

			var tilesx = WorldTiles.Instance.worldTiles; // This is our Dictionary of tiles
			var map = WorldTiles.Instance.map;

			if (tilesx.TryGetValue(worldPoint, out _tile))
			{
				_tile.parentTileMap.SetTileFlags(_tile.localPlace, TileFlags.None);
				_tile.parentTileMap.SetColor(_tile.localPlace, HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 1, 1), 1, 1)));
                //Debug.Log("Clicked tile: " + _tile.localPlace + " " + _tile.worldLocation);
                //Debug.Log("map x: " + (Mathf.Abs(_tile.parentTileMap.origin.x) - Mathf.Abs(_tile.localPlace.x)) + " map y: " + (Mathf.Abs(_tile.parentTileMap.origin.y) - Mathf.Abs(_tile.localPlace.y)));
                //Debug.Log("hehex "+ (map[Mathf.Abs(_tile.parentTileMap.origin.x) - Mathf.Abs(_tile.localPlace.x), Mathf.Abs(_tile.parentTileMap.origin.y) - Mathf.Abs(_tile.localPlace.y)].localPlace.x == _tile.localPlace.x));
                //Debug.Log("hehey " + (map[Mathf.Abs(_tile.parentTileMap.origin.x) - Mathf.Abs(_tile.localPlace.x), Mathf.Abs(_tile.parentTileMap.origin.y) - Mathf.Abs(_tile.localPlace.y)].localPlace.y == _tile.localPlace.y));

                //Debug.Log("Clicked tile is same as map tile: " + map[Mathf.Abs(_tile.parentTileMap.origin.x) - Mathf.Abs(_tile.localPlace.x), Mathf.Abs(_tile.parentTileMap.origin.y) - Mathf.Abs(_tile.localPlace.y)].localPlace.x == _tile.localPlace.x
                //    + " " + (map[Mathf.Abs(_tile.parentTileMap.origin.x) - Mathf.Abs(_tile.localPlace.x), Mathf.Abs(_tile.parentTileMap.origin.y) - Mathf.Abs(_tile.localPlace.y)].localPlace.y == _tile.localPlace.y));

            }
        }
    }

}
