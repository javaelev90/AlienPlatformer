using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarBulletDestory : MonoBehaviour
{
    [SerializeField] private string nameOfLayerToCollideWith;
    [SerializeField] public GameObject sonarBulletParent;

    private BoxCollider2D boxCollider2D;
    private void Start() {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        RaycastHit2D downCast = Physics2D.BoxCast(transform.position, boxCollider2D.size, 0f, Vector2.down, boxCollider2D.size.y*6f, LayerMask.GetMask("Enemy"));
        Dictionary<Vector3, TileMapTile> tiles = WorldTiles.Instance.worldTiles;

        if (downCast.collider != null)
        {
            foreach (KeyValuePair<int, GameObject> kvp in WorldTiles.Instance.enemies)
            {
                if (kvp.Value.GetInstanceID() == downCast.collider.gameObject.GetInstanceID())
                {
                    EnemyStats mapObject = downCast.collider.gameObject.GetComponent<EnemyStats>();
                    mapObject.GetComponent<EnemyStats>().GetMiniMapObject().visibleOnMinimap = true;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(nameOfLayerToCollideWith))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            // foreach (KeyValuePair<int, GameObject> kvp in WorldTiles.Instance.enemies)
            // {
            //     if (kvp.Value.GetInstanceID() == collision.gameObject.GetInstanceID())
            //     {
            //         EnemyStats mapObject = collision.gameObject.GetComponent<EnemyStats>();
            //         mapObject.GetComponent<EnemyStats>().GetMiniMapObject().visibleOnMinimap = true;
            //     }
            // }
        }
    }

    private void OnDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
