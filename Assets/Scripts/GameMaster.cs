using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    public static GameMaster _instance;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerStartPosition;
    private IStateful playerState;
    private List<IStateful> objectsToRollback;
    private List<IStateful> presistentObjectsToRollback;
    private List<GameObject> removeableObjects;

    public AudioSource audioSource;

    [SerializeField] private bool shouldRollbackOnDeath = true;

    public static GameMaster Instance
    {
        get
        {
            return _instance;
        }

    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            GameObject.Destroy(Instance);
            return;
        } else
        {
            _instance = this;
        }
        // Will maybe use this
        //DontDestroyOnLoad(this);
        if(player == null){
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        presistentObjectsToRollback = new List<IStateful>();
        objectsToRollback = new List<IStateful>();
        playerState = player.GetComponent<IStateful>();
        removeableObjects = new List<GameObject>();
        player.GetComponent<PlayerStats>().ChangeRespawnPosition(playerStartPosition);
        player.GetComponent<PlayerStats>().MoveToStartPosition(playerStartPosition);
        player.GetComponent<PlayerStats>().audioSource = audioSource;
    }


    public void AddRemoveableObject(GameObject gameObject)
    {
        removeableObjects.Add(gameObject);
    }

    private void ClearRemoveableObject()
    {
        foreach(GameObject gameObject in removeableObjects){
            Destroy(gameObject);
        }
        removeableObjects.Clear();
    }

    public void RespawnPointCaptured()
    {
        objectsToRollback.Clear();
        playerState.SaveState();
        foreach(IStateful stateful in presistentObjectsToRollback)
        {
            stateful.SaveState();
        }
    }

    public void RollbackToLastRespawnPoint()
    {
        if(shouldRollbackOnDeath){
            ClearRemoveableObject();
            RollbackStatefulObjects();
        }
        playerState.LoadSavedState();
    }

    public void AddStatefulObject(IStateful stateful)
    {
        objectsToRollback.Add(stateful);
    }

    public void AddPersistentStatefulObject(IStateful stateful)
    {
        presistentObjectsToRollback.Add(stateful);
    }

    private void RollbackStatefulObjects()
    {
        foreach(IStateful stateful in objectsToRollback)
        {
            stateful.LoadSavedState();
        }
        foreach(IStateful stateful in presistentObjectsToRollback)
        {
            stateful.LoadSavedState();
        }
    }


}
