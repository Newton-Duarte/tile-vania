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
    [SerializeField] GameObject bullet;
    [SerializeField] Transform weapon;
    [SerializeField] AudioClip bowClip;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip spikeClip;
    [SerializeField] AudioClip bouncyClip;
    [SerializeField] AudioClip dieClip;

    CinemachineImpulseSource cameraImpulseSource;
    Rigidbody2D rb;
    Animator animator;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    Vector2 moveInput;
    float baseGravity;
    bool isAlive = true;

    GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
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

    void OnFire(InputValue value)
    {
        if (!isAlive) return;
        animator.SetTrigger("Atack");
    }

    public void Shoot()
    {
        gameManager.PlaySFXClip(bowClip);
        Instantiate(bullet, weapon.transform.position, bullet.transform.rotation);
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
            gameManager.PlaySFXClip(jumpClip);
        }
    }

    void ClimbLadder()
    {
        if (!bodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
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

        if (collision.gameObject.tag == "Hazard")
        {
            gameManager.PlaySFXClip(spikeClip);
        }

        if (collision.gameObject.tag == "Bouncy")
        {
            gameManager.PlaySFXClip(bouncyClip);
        }
    }

    private void Die()
    {
        isAlive = false;
        gameManager.PlaySFXClip(dieClip);
        cameraImpulseSource.GenerateImpulse(0.25f);
        rb.velocity = new Vector2(10f, 15f);
        rb.velocity = new Vector2(0f, rb.velocity.y);
        animator.SetTrigger("Die");
        FindAnyObjectByType<GameManager>().ProcessPlayerDeath();
    }
}
