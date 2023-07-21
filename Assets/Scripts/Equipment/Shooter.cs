using Assets.Scripts.Equipment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shooter : MonoBehaviour, IEquipable
{
    public IMagazine magazine;
    [SerializeField] public int ammo;
    [SerializeField] public float damage;
    [SerializeField] public GameObject bullet;
    [SerializeField] public Transform playerEquipmentHolder;
    [SerializeField] public Transform firePoint;
    [SerializeField] public LayerMask groundLayer;

    public abstract void Activate();
    public abstract void InActivate();
    public abstract string GetItemName();

    public Sprite getItemSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }

    public IItem PickUp()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        GetComponent<SpriteRenderer>().enabled = false;
        transform.rotation = playerEquipmentHolder.rotation;
        transform.SetParent(playerEquipmentHolder);
        return this;
    }

    public void Drop()
    {
        transform.parent = null;
        GetComponent<SpriteRenderer>().enabled = true;
        transform.position = playerEquipmentHolder.position;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(DropItemToGround());
    }

    private IEnumerator DropItemToGround()
    {
        GetComponent<Collider2D>().isTrigger = false;
        GetComponent<Rigidbody2D>().gravityScale = 1;
        while (!GroundCheck())
        {
            yield return null;
        }
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Collider2D>().isTrigger = true;
    }

    private bool GroundCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(GetComponent<BoxCollider2D>().size.x*0.95f, 0.05f), 0f, Vector2.down, 0.05f, groundLayer);

        return hit.collider != null;
    }

    public void increaseAmmo(int ammo)
    {
        this.ammo += ammo;
    }

    public void decreaseAmmo(int ammo)
    {
        this.ammo -= ammo;
    }

    public void Equip()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        transform.position = playerEquipmentHolder.position;
    }

    public void UnEquip()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
