using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public static GameObject Instance { get; private set; }

    // movement params
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float movementMultiplier = 100f;
    private bool facingRight = true;
    private float horizontalAxisValue;
    private bool playerHasHorizontalSpeed;
    private bool canMove = true;

    // jump params
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpForceMultiplier = 100f;
    private bool isJumpPressed = false;

    // collider and rigidbody 2D
    private BoxCollider2D bodyCollider;
    private Rigidbody2D rigidBody2D;

    // ground check configs
    private float rayCastDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    // Animator
    private Animator animator;


    // sounds
    private AudioSource audioSource;
    [SerializeField] private AudioClip runSoundFX;
    private bool soundIsPlaying = false;

    void Start()
    {
        bodyCollider = GetComponent<BoxCollider2D>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        //rayCastDistance = (bodyCollider.size.y / 4);
    }

    private void Awake()
    {
        // DontDestroyOnLoad(gameObject);
        // if(Instance == null){
        //     Instance = gameObject;
        // }

        if(Instance != null && Instance != gameObject){
            Destroy(gameObject);
            return;
        } else {
            Instance = gameObject;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isJumpPressed = true;
        }
        horizontalAxisValue = Input.GetAxisRaw("Horizontal");

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if(canMove){
            playerHasHorizontalSpeed = Mathf.Abs(horizontalAxisValue) > Mathf.Epsilon;

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

            Run();
            Jump();
        }
    }

    private void Run()
    {
        Vector2 inputVector = new Vector2(horizontalAxisValue, 0f);
        var velocity = rigidBody2D.velocity;

        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = (inputVector * movementSpeed * movementMultiplier) * Time.fixedDeltaTime;

        velocity.x = movement.x;
        rigidBody2D.velocity = velocity;

        animator.SetBool("isRunning", playerHasHorizontalSpeed);
        if (playerHasHorizontalSpeed && !soundIsPlaying && GroundCheck())
        {
            StartCoroutine(WaitForSound());
        } 
        if((!playerHasHorizontalSpeed && soundIsPlaying) || !GroundCheck())
        {
            audioSource.Stop();
        }
        
    }

    private IEnumerator WaitForSound()
    {
        audioSource.pitch = 3f;
        soundIsPlaying = true;
        audioSource.PlayOneShot(runSoundFX);
        yield return new WaitForSeconds(runSoundFX.length/audioSource.pitch);
        soundIsPlaying = false;
        audioSource.pitch = 1f;
    }

    private void Jump()
    {
        if (!GroundCheck()) {
            isJumpPressed = false;
            return;
        }
        // rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 0f);


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
        RaycastHit2D hitGround = Physics2D.BoxCast(transform.position, bodyCollider.size, 0f, Vector2.down, rayCastDistance, groundLayer);
        RaycastHit2D hitElevator = Physics2D.BoxCast(transform.position, bodyCollider.size, 0f, Vector2.down, rayCastDistance, LayerMask.GetMask("Elevator"));
        RaycastHit2D hitEnemy = Physics2D.BoxCast(transform.position, bodyCollider.size, 0f, Vector2.down, rayCastDistance, LayerMask.GetMask("Enemy"));

        return (hitGround.collider != null || hitElevator.collider != null || hitEnemy.collider != null);
    }

    private void FlipDirection()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public bool toggleCanMove(){
        canMove = !canMove;
        animator.SetBool("isRunning", canMove);
        return canMove;
    }
}
