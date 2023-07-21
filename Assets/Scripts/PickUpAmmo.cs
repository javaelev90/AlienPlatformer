using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAmmo : MonoBehaviour, IStateful
{

    [SerializeField] private Shooter gunForThisAmmo;
    [SerializeField] private int ammoInPickUp;
    [SerializeField] private bool shouldRollback = false;
    void Start()
    {
        ammoInPickUp = 5;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<Inventory>().ContainsItem(gunForThisAmmo.GetItemName()))
            {
                gunForThisAmmo = (Shooter) collision.GetComponent<Inventory>().GetItem(gunForThisAmmo.GetItemName());
                gunForThisAmmo.increaseAmmo(ammoInPickUp);
    
                if(shouldRollback){
                    gameObject.SetActive(false);
                    GameMaster.Instance.AddStatefulObject(this); 
                } else {
                    Destroy(gameObject);
                }
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
