using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float climbForce = 5f;

    Rigidbody2D rb;
    Animator animator;
    CapsuleCollider2D capsuleCollider;
    Vector2 moveInput;
    float baseGravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        baseGravity = rb.gravityScale;
    }

    void FixedUpdate()
    {
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
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            rb.velocity += new Vector2(0f, jumpForce);
        }
    }

    void ClimbLadder()
    {
        if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            animator.SetBool("isClimbing", false);
            rb.gravityScale = baseGravity;
            return;
        }

        rb.velocity = new Vector2(rb.velocity.x, moveInput.y * climbForce);
        rb.gravityScale = 0f;

        animator.SetBool("isClimbing", isPlayerClimbing);
    }
}
