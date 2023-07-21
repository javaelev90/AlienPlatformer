using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBullet : MonoBehaviour
{
    public float maxTravelDistance = 5f;
    public float maxSpeed = 25f;
    public int damage = 1;
    private Rigidbody2D bullet;
    private Vector3 instantiationPosition;
    [SerializeField] private ParticleSystem destructionParticles;

    // Start is called before the first frame update
    void Start()
    {
        bullet = gameObject.GetComponent<Rigidbody2D>();
        bullet.velocity = transform.right * maxSpeed;
        instantiationPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(instantiationPosition.x - transform.position.x) > maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyStats>().takeDamage(damage);
            Destroy(Instantiate(destructionParticles.gameObject, transform.position, transform.rotation), destructionParticles.main.duration - 0.01f);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(Instantiate(destructionParticles.gameObject, transform.position, transform.rotation), destructionParticles.main.duration - 0.01f);
            Destroy(gameObject);
        }
    }

}
