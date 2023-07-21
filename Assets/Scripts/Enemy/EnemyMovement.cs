using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Attack params
    private EnemyStats stats;
    private float attackTimer;
    private bool isAttacking;

    // Target params
    [SerializeField] private GameObject player;
    [SerializeField] private int attackProximity = 5;
    [SerializeField] private List<GameObject> patrolPoints;
    private GameObject nextTarget;
    private int currentTargetPointIndex;
    [SerializeField] private bool chasePlayerOn = false;

    // movement params
    float movementSpeed;
    [SerializeField] float originalMovementSpeed = 3f;
    [SerializeField] float playerChasingMovementSpeed = 5f;
    private bool facingRight = false;
    private float horizontalAxisValue;
    private bool playerHasHorizontalSpeed;
    // jump params
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpForceMultiplier = 100f;
    private bool isJumpPressed = false;

  
    // collider and rigidbody 2D
    private BoxCollider2D bodyCollider;
    private Rigidbody2D rigidBody2D;

    // ground check configs
    [SerializeField] private float rayCastDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    // animation
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip attackAnimation;

    void Start()
    {
        bodyCollider = GetComponent<BoxCollider2D>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        // rayCastDistance = 0.1f;//(bodyCollider.size.y / 2) + 0.1f;
        movementSpeed = originalMovementSpeed;
        stats = GetComponent<EnemyStats>();
        attackTimer = stats.attackDelay;

        if (patrolPoints.Count > 0)
        {
            nextTarget = patrolPoints[0];
            currentTargetPointIndex = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (nextTarget != null)
        { 
            Move();
        }
        attackTimer += Time.fixedDeltaTime;
        if(attackTimer > attackAnimation.length){
            animator.SetBool("isAttacking", false);
        }
            
    }

    private void Move()
    {
        playerHasHorizontalSpeed = Mathf.Abs(horizontalAxisValue) > Mathf.Epsilon;

        if (PlayerInRange())
        {
            ChasePlayer();
        } else
        {
            Patrol();
        }

        if (playerHasHorizontalSpeed)
        {
            if (Mathf.Sign(horizontalAxisValue) > 0 && !facingRight)
            {
                FlipDirection();
            }
            else if (Mathf.Sign(horizontalAxisValue) < 0 && facingRight)
            {
                FlipDirection();
            }
        }
        MoveToPosition(nextTarget);
        Jump();
    }

    private bool PlayerInRange()
    {
        return chasePlayerOn &&
            Mathf.Abs(player.transform.position.x - transform.position.x) < attackProximity;
    }

    private void ChasePlayer()
    {
        nextTarget = player;
        movementSpeed = playerChasingMovementSpeed;
    }

    private void Patrol()
    {
        nextTarget = patrolPoints[currentTargetPointIndex];
        movementSpeed = originalMovementSpeed;
    }

    private void Jump()
    {
        if (!GroundCheck()) {
            isJumpPressed = false;
            return;
        }
        Vector2 jumpDistance = new Vector2(0f, jumpForce * jumpForceMultiplier) * Time.fixedDeltaTime;

        var velocity = rigidBody2D.velocity;

        velocity.y += jumpDistance.y;

        if (isJumpPressed)
        {
            rigidBody2D.velocity += velocity;
            //animator.SetTrigger("Jump");
            //audioSource.PlayOneShot(jumpSFX);
            isJumpPressed = false;
        }
    }

    private bool GroundCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, bodyCollider.size, 0f, Vector2.down, rayCastDistance, groundLayer);

        return hit.collider != null;
    }

    private void FlipDirection()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void MoveToPosition(GameObject moveToTarget)
    {
        Vector3 target = new Vector3(moveToTarget.transform.position.x, transform.position.y);

        // Stands on target, mainly used for player
        // The distance should be smaller than the "switch target distance"
        if(PlayerInRange() && Mathf.Abs(transform.position.x - target.x) < bodyCollider.size.x / 1.1f)
        {         
            StopMovement();
        }
        else if(moveToTarget.transform.position.x > transform.position.x)
        {
            MoveRight();
        } 
        else
        {
            MoveLeft();
        }
        // Switch target when close to current target
        if (Mathf.Abs(transform.position.x - target.x) < bodyCollider.size.x/2)
        {
            if (!PlayerInRange())
            {
                ChangeTarget();
            }
        }
    }

    private void StopMovement()
    {

        rigidBody2D.velocity = new Vector2(0f, rigidBody2D.velocity.y);
        horizontalAxisValue = 0f;
    }

    private void MoveLeft()
    {
        rigidBody2D.velocity = new Vector2(-movementSpeed, rigidBody2D.velocity.y);
        horizontalAxisValue = -movementSpeed;
    }

    private void MoveRight()
    {
        rigidBody2D.velocity = new Vector2(movementSpeed, rigidBody2D.velocity.y);
        horizontalAxisValue = movementSpeed;
    }

    private void ChangeTarget()
    {
        currentTargetPointIndex++;
        if (currentTargetPointIndex >= patrolPoints.Count)
        {
            currentTargetPointIndex = 0;
        }

        while (patrolPoints[currentTargetPointIndex] == null)
        {
            currentTargetPointIndex++;
            if (currentTargetPointIndex >= patrolPoints.Count)
            {
                currentTargetPointIndex = 0;
            }
        }
        nextTarget = patrolPoints[currentTargetPointIndex];
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isJumpPressed = true;
        }
        if (collision.gameObject.CompareTag("Player"))
        {

            if(attackTimer > stats.attackDelay)
            {
                animator.SetBool("isAttacking", true);
                collision.gameObject.GetComponent<PlayerStats>().takeDamage(stats.attackDamage);
                attackTimer = 0f;

            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if(attackTimer > stats.attackDelay)
            {
                animator.SetBool("isAttacking", true);
                collision.gameObject.GetComponent<PlayerStats>().takeDamage(stats.attackDamage);
                attackTimer = 0f;

            }
        }
    }

}
