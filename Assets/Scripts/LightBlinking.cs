using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightBlinking : MonoBehaviour
{

    [SerializeField] private Light2D pointLight;
    [SerializeField] private float onOffInterval = 1f;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = onOffInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= onOffInterval)
        {
            pointLight.enabled = !pointLight.enabled;
            timer = 0;
        }
        timer += Time.deltaTime;
    }
}
