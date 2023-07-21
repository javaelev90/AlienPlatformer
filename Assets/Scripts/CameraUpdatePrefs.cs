using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraUpdatePrefs : MonoBehaviour
{
    void Start()
    {
        if(GetComponent<CinemachineVirtualCamera>().Follow == null){
            GetComponent<CinemachineVirtualCamera>().Follow =
                GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
