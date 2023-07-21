using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{

    [SerializeField] private GameObject target;
    [SerializeField] private float cameraMovementSpeed = 6f;
    [SerializeField] private float zoomFactor = -10f;
    [SerializeField] private float maxTravelOnAxisY = 2f;
    private Vector2 startPosition;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0f, 0f, zoomFactor);
        startPosition = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = target.transform.position;
        
        if (target.transform.position.y >= (startPosition.y + maxTravelOnAxisY)){
            targetPosition = new Vector3(targetPosition.x, startPosition.y + maxTravelOnAxisY, targetPosition.z);
        } else if(target.transform.position.y <= (startPosition.y - maxTravelOnAxisY)){
            targetPosition = new Vector3(targetPosition.x, startPosition.y - maxTravelOnAxisY, targetPosition.z);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition + offset, cameraMovementSpeed * Time.deltaTime);

    }
}
