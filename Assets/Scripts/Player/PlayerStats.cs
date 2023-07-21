using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IStateful
{
    private PlayerState playerState;
    private GameObject respawnPosition;
    private Inventory inventory;
    [SerializeField] private GameObject startPosition;
    [SerializeField] private AudioClip hurtSFX;
    [HideInInspector] public AudioSource audioSource;
    public int playerHealth;
    public int playerInitialHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerInitialHealth = playerHealth;
        respawnPosition = startPosition;
        audioSource = GameMaster.Instance.GetComponent<AudioSource>();
        inventory = GetComponent<Inventory>();
        SaveState();
    }

    public void takeDamage(int healthLost)
    {
        playerHealth -= healthLost;
        audioSource.PlayOneShot(hurtSFX);
        if (!isAlive())
        {
            Respawn();
        }
    }

    public void AddHealth(int health)
    {
        playerHealth += health;
    }

    public bool isAlive()
    {
        return playerHealth > 0;
    }

    public void Respawn()
    {
        playerHealth = playerInitialHealth;
        gameObject.transform.position = respawnPosition.transform.position;
        GameMaster.Instance.RollbackToLastRespawnPoint();
        StartCoroutine(FreezePosition());
    }

    private IEnumerator FreezePosition(){
        TogglePlayerLock();
        yield return new WaitForSeconds(0.7f);
        TogglePlayerLock();
        yield return null;
    }

    public void TogglePlayerLock(){

        gameObject.GetComponent<PlayerMovement>().toggleCanMove();
        inventory.ToggleInventoryActive();
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    public void ChangeRespawnPosition(GameObject newRespawnPosition)
    {
        respawnPosition = newRespawnPosition;
    }

    public void MoveToStartPosition(GameObject startPosition){
        gameObject.transform.position = startPosition.transform.position;
    }

    public void SaveState()
    {
        playerState = new PlayerState()
        {
            playerHealth = this.playerHealth
        };
        if(inventory != null)
        {
            inventory.SaveState();
        }
    }

    public void LoadSavedState()
    {
        playerHealth = playerState.playerHealth;
        if (inventory != null)
        {
            inventory.LoadSavedState();
        }
    }

    private class PlayerState
    {
        public int playerHealth;
    }
}

