using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Config
    [SerializeField] float resetLevelDelay = 2f;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Collider2D playerCollider;
    [SerializeField] Collider2D feetCollider;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 15f);

    // State
    bool isAlive = true;
    bool isRunning = false;
    bool isFalling = false;
    bool isClimbing = false;
    bool isJumping = false;

    // Cached component references
    Rigidbody2D rb;
    Animator animator;
    float initalGravityScale;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        initalGravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }

        Run();
        FlipSprite();
        Jump();
        Fall();
        ClimbLadder();
        Die();
    }

    private void Run()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float horizontalMove = Mathf.Abs(h * runSpeed);

        Vector2 velocity = new Vector2(h * runSpeed, rb.velocity.y);
        rb.velocity = velocity;

        animator.SetFloat("speed", horizontalMove);
    }

    private void FlipSprite()
    {
        isRunning = Mathf.Abs(rb.velocity.x) > 0;

        if (isRunning)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), transform.localScale.y);
        }
    }

    private void Jump()
    {
        animator.SetBool("isJumping", isJumping);

        var isTouchingLayer = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (!isTouchingLayer) { return; }

        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
            rb.velocity += jumpVelocity;
            isJumping = true;
        }
    }

    private void Fall()
    {
        if (rb.velocity.y < -0.1)
        {
            isJumping = false;
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }
        animator.SetBool("isFalling", isFalling);
    }

    private void ClimbLadder()
    {
        var isTouchingLadder = playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));

        if (!isTouchingLadder)
        {
            isClimbing = false;
            rb.gravityScale = initalGravityScale;
            animator.SetBool("isClimbing", isClimbing);
            return;
        }

        isClimbing = true;

        var v = Input.GetAxisRaw("Vertical");

        Vector2 verticalVelocity = new Vector2(rb.velocity.x, v * climbSpeed);

        rb.gravityScale = 0f;

        rb.velocity = verticalVelocity;

        animator.SetBool("isClimbing", isClimbing);
    }

    private void Die()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) || feetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("die");
            rb.gravityScale = initalGravityScale;
            rb.velocity = deathKick;
            StartCoroutine(RemovePlayerPhysics());
            StartCoroutine(ProcessDeath());
        }
    }

    IEnumerator RemovePlayerPhysics()
    {
        yield return new WaitForSeconds(1f);
        rb.simulated = false;
    }

    IEnumerator ProcessDeath()
    {
        yield return new WaitForSeconds(resetLevelDelay);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
