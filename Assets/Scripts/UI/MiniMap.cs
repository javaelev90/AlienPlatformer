using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public GameObject wallTilePrefab;
    public GameObject playerTilePrefab;
    public GameObject friendTilePrefab;
    public GameObject enemyTilePrefab;
    public GameObject hazardTilePrefab;
    private float enemyVisibleTime = 1f;

    private Dictionary<int, TimeKeeper> enemyVisibleCountDown;
    // Start is called before the first frame update
    void Start()
    {
        enemyVisibleCountDown = new Dictionary<int, TimeKeeper>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMap();
    }

    private void UpdateMap()
    {
        UpdateTiles();
        UpdateEnemies();
    }

    private void UpdateTiles()
    {
        foreach (KeyValuePair<Vector3, TileMapTile> kvp in WorldTiles.Instance.worldTiles)
        {
            TileMapTile tile = kvp.Value;
            if (tile.GetMiniMapObject().visibleOnMinimap && !tile.GetMiniMapObject().paintedOnMinimap)
            {
                tile.GetMiniMapObject().paintedOnMinimap = true;
                Instantiate(getGameObjectFromName(tile.GetMiniMapObject().objectName), tile.localPlace, Quaternion.identity);
            }
        }
    }

    private void UpdateEnemies()
    {
        foreach (KeyValuePair<int, GameObject> kvp in WorldTiles.Instance.enemies)
        {
            EnemyStats enemy = kvp.Value.GetComponent<EnemyStats>();
            int enemyID = kvp.Value.GetInstanceID();

            // Add object to minimap
            if (enemy.GetMiniMapObject().visibleOnMinimap
                && !enemy.GetMiniMapObject().paintedOnMinimap
                && !enemyVisibleCountDown.ContainsKey(enemyID))
            {
                enemy.GetMiniMapObject().paintedOnMinimap = true;
                enemyVisibleCountDown.Add(enemyID, new TimeKeeper
                {
                    mapObject = kvp.Value,
                    miniMapObject = Instantiate(getGameObjectFromName(enemy.GetMiniMapObject().objectName), kvp.Value.transform.position, Quaternion.identity),
                    timeToLive = enemyVisibleTime
                });
            }
        }
        List<int> finishedTimeKeepers = new List<int>();
        foreach (KeyValuePair<int, TimeKeeper> kvp in enemyVisibleCountDown)
        {
            TimeKeeper visibleEnemy = kvp.Value;
            visibleEnemy.timeToLive -= Time.deltaTime;
            if (visibleEnemy.timeToLive < 0)
            {
                Destroy(visibleEnemy.miniMapObject);
                if (visibleEnemy.mapObject != null)
                {
                    visibleEnemy.mapObject.GetComponent<EnemyStats>().GetMiniMapObject().visibleOnMinimap = false;
                    visibleEnemy.mapObject.GetComponent<EnemyStats>().GetMiniMapObject().paintedOnMinimap = false;
                    finishedTimeKeepers.Add(visibleEnemy.mapObject.GetInstanceID());
                }
            }
        }
        foreach(int removableTimeKeeper in finishedTimeKeepers)
        {
            enemyVisibleCountDown.Remove(removableTimeKeeper);
        }
    }

    private GameObject getGameObjectFromName(string name)
    {
        if (name == "tile")
        {
            return wallTilePrefab;
        } 
        else if (name == "player")
        {
            return playerTilePrefab;
        } 
        else if (name == "enemy")
        {
            return enemyTilePrefab;
        } 
        else if (name == "hazard")
        {
            return hazardTilePrefab;
        } 
        else
        {
            return friendTilePrefab;
        }
    }

    class TimeKeeper
    {
        public GameObject mapObject { get; set; }
        public GameObject miniMapObject { get; set; }
        public float timeToLive { get; set; }
    }
}
