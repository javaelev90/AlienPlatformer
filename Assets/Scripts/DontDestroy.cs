using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{

    public static GameObject Instance;
    private void Awake()
    {
        if(Instance != null && Instance != gameObject){
            Destroy(gameObject);
            return;
        } else {
            Instance = gameObject;
        }
        DontDestroyOnLoad(gameObject);
    }
}
