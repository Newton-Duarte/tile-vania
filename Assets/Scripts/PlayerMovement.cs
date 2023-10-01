using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float climbForce = 5f;
    [SerializeField] string[] hazardTags = new string[] { "Enemy", "Hazard" };

    CinemachineImpulseSource cameraImpulseSource;
    Rigidbody2D rb;
    Animator animator;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    Vector2 moveInput;
    float baseGravity;
    bool isAlive = true;

    void Start()
    {
        cameraImpulseSource = GetComponent<CinemachineImpulseSource>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        baseGravity = rb.gravityScale;
    }

    void FixedUpdate()
    {
        if (!isAlive) return;

        Run();
        FlipSprite();
        ClimbLadder();
    }

    bool isPlayerMoving => Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
    bool isPlayerClimbing => Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;

    void Run()
    {
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        animator.SetBool("isRunning", isPlayerMoving);
    }

    void FlipSprite()
    {
        if (isPlayerMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), transform.localScale.y);
        }
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) return;

        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            rb.velocity += new Vector2(0f, jumpForce);
        }
    }

    void ClimbLadder()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            animator.SetBool("isClimbing", false);
            rb.gravityScale = baseGravity;
            return;
        }

        rb.velocity = new Vector2(rb.velocity.x, moveInput.y * climbForce);
        rb.gravityScale = 0f;

        animator.SetBool("isClimbing", isPlayerClimbing);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hazardTags.Contains(collision.gameObject.tag) && isAlive)
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        cameraImpulseSource.GenerateImpulse(0.25f);
        rb.velocity = new Vector2(10f, 15f);
        rb.velocity = new Vector2(0f, rb.velocity.y);
        animator.SetTrigger("Die");
    }
}
