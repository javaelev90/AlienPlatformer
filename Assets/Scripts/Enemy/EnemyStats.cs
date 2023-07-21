using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour, IMiniMapObject, IStateful
{
    private EnemyState enemyState;
    public int attackDamage = 1;
    public float attackDelay = 0.5f;
    public int health = 5;
    public List<GameObject> itemsToDrop;

    private GameObject player;
    MiniMapObject miniMapObject;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        miniMapObject = new MiniMapObject
        {
            visibleOnMinimap = false,
            paintedOnMinimap = false,
            objectName = "enemy"
        };
        SaveState();
    }

    private void Update()
    {
        if (!isAlive())
        {
            DeleteSelf();
        }
    }

    private void DeleteSelf()
    {
        //GameObject self;
        //WorldTiles.Instance.enemies.TryRemove(gameObject.GetInstanceID(), out self);
        if (itemsToDrop != null && itemsToDrop.Count > 0)
        {
            foreach(GameObject prefab in itemsToDrop){
                GameMaster.Instance.AddRemoveableObject(Instantiate(prefab, transform.position, transform.rotation));
            }
        }
        if(player.GetComponent<MonsterHuntQuest>() != null){
            player.GetComponent<MonsterHuntQuest>().KilledMonster();
        }
        //Destroy(gameObject);
        gameObject.SetActive(false);
        GameMaster.Instance.AddStatefulObject(this);
    }

    private void OnDestroy()
    {
        Destroy(transform.parent.gameObject);
    }

    public MiniMapObject GetMiniMapObject()
    {
        return miniMapObject;
    }

    public void takeDamage(int damage)
    {
        health -= damage;
    }

    private bool isAlive()
    {
        return health > 0;
    }

    public void SaveState()
    {
        enemyState = new EnemyState()
        {
            health = this.health,
            enabled = gameObject.activeSelf
        };
    }

    public void LoadSavedState()
    {
        this.health = enemyState.health;
        gameObject.SetActive(enemyState.enabled);
    }

    private class EnemyState
    {
        public int health;
        public bool enabled;
    }
}
