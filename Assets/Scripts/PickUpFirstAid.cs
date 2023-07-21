using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFirstAid : MonoBehaviour, IStateful
{

    [SerializeField] private int healthPoints;
    [SerializeField] private bool shouldRollback = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>().AddHealth(healthPoints);
            
            if(shouldRollback){
                gameObject.SetActive(false);
                // FIX THIS, it adds same object multiple times possibly
                GameMaster.Instance.AddStatefulObject(this); 
            } else {
                Destroy(gameObject);
            }
        }
    }

    public void SaveState()
    {
    }
    public void LoadSavedState()
    {
        if(shouldRollback){
            gameObject.SetActive(true);
        }
    }
}
