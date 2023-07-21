using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>().ChangeRespawnPosition(gameObject);
            GameMaster.Instance.RespawnPointCaptured();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
