using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SonarBullet : MonoBehaviour
{
    // travel distance config
    public float maxTravelDistance = 10f;
    private Vector3 instantiationPosition;

    public float speed = 20f;
    public Rigidbody2D rigidBody2D;
    public BoxCollider2D boxCollider2D;
    public LayerMask groundLayer;

    void Start()
    {
        rigidBody2D.velocity = transform.right * speed;
        instantiationPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(instantiationPosition.x - transform.position.x) > maxTravelDistance)
        {
            Destroy(gameObject);
        }
        RaycastHit2D downCast = Physics2D.BoxCast(transform.position, boxCollider2D.size, 0f, Vector2.down, boxCollider2D.size.y*6f, groundLayer);
        RaycastHit2D upCast = Physics2D.BoxCast(transform.position, boxCollider2D.size, 0f, Vector2.up, boxCollider2D.size.y*6f, groundLayer);
        
        // RaycastHit2D frontCast = Physics2D.Raycast(transform.position,Vector2.right, 0.2f, groundLayer);
        RaycastHit2D frontCast = Physics2D.BoxCast(transform.position, new Vector2(0.1f, 0.3f), 0f, new Vector2(Mathf.Sign(rigidBody2D.velocity.x),0f), 0.2f, groundLayer);

        Dictionary<Vector3, TileMapTile> tiles = WorldTiles.Instance.worldTiles;

        if (downCast.collider != null)
        {

            Vector3Int worldPoint = new Vector3Int(Mathf.FloorToInt(downCast.point.x), Mathf.FloorToInt(downCast.point.y - 0.1f), 0);
            TileMapTile tile;
            if (tiles.TryGetValue(worldPoint, out tile))
            {
                tile.GetMiniMapObject().visibleOnMinimap = true;
            }
        }
        if (upCast.collider != null)
        {
            Vector3Int worldPoint = new Vector3Int(Mathf.FloorToInt(upCast.point.x), Mathf.FloorToInt(upCast.point.y + 0.1f), 0);
            TileMapTile tile;
            if (tiles.TryGetValue(worldPoint, out tile))
            {
                tile.GetMiniMapObject().visibleOnMinimap = true;
            }
        }
        if (frontCast.collider != null)
        {
            Vector3Int worldPoint = new Vector3Int(Mathf.FloorToInt(frontCast.point.x + (Mathf.Sign(rigidBody2D.velocity.x) * 0.4f)), Mathf.FloorToInt(frontCast.point.y), 0);
            TileMapTile tile;
            if (tiles.TryGetValue(worldPoint, out tile))
            {
                tile.GetMiniMapObject().visibleOnMinimap = true;
            }
        }
    }
}
