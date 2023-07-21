using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyGameObjects : MonoBehaviour
{
    void Start()
    {
        GameObject tempObject = new GameObject("Sacrificial Lamb");
        DontDestroyOnLoad(tempObject);

        foreach (GameObject dontdestroyGameObject in tempObject.scene.GetRootGameObjects())
        {
            Destroy(dontdestroyGameObject);
        }
    }

}
