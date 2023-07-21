using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMovement : MonoBehaviour
{
    [SerializeField] private GameObject endPositionObject;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 currentTarget;

    [SerializeField] float speed = 1f;
    [SerializeField] float speedDown = 0.5f;
    private float currentSpeed;

    void Start()
    {
        endPosition = endPositionObject.transform.position;
        startPosition = transform.position;
        currentSpeed = speed;
        currentTarget = startPosition;
    }

    void FixedUpdate()
    {
        if (currentTarget != null)
        {
            MoveToPosition(currentTarget);
        }
    }

    private void MoveToPosition(Vector3 moveToTarget)
    {
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, moveToTarget, currentSpeed * Time.fixedDeltaTime);
        
        //if (gameObject.transform.position == moveToTarget)
        //{
        //    ChangeTarget();
        //}
    }

    private void ChangeTarget()
    {
       if(currentTarget == startPosition)
       {
            currentTarget = endPosition;
       } else
        {
            currentTarget = startPosition;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(gameObject.transform);
            currentTarget = endPosition;
            currentSpeed = speed;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
            DontDestroyOnLoad(collision.gameObject);
            currentTarget = startPosition;
            currentSpeed = speedDown;
        }
    }
}