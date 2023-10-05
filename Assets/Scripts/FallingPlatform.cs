using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] Transform jumpArea;
    [SerializeField] float fallingTime = 1.5f;
    [SerializeField] AudioClip fallingClip;

    BoxCollider2D boxCollider;
    TargetJoint2D targetJoint;
    Rigidbody2D rb;
    Shake shakeScript;

    GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        boxCollider = GetComponent<BoxCollider2D>();
        targetJoint = GetComponent<TargetJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        shakeScript = GetComponent<Shake>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        float jumpAreaY = jumpArea.position.y;
        float collisionPointY = collision.GetContact(0).point.y;
        bool collidesFromAbove = collisionPointY > jumpAreaY;

        if (collidesFromAbove)
        {
            gameManager.PlaySFXClip(fallingClip);
            shakeScript.StartShaking();
            Invoke(nameof(MakePlatformFall), fallingTime);
        }
    }

    void MakePlatformFall()
    {
        rb.gravityScale = 8.0f;
        targetJoint.enabled = false;
        boxCollider.isTrigger = true;
        shakeScript.StopShaking();
        Invoke(nameof(DestroyPlatform), 1f);
    }

    void DestroyPlatform()
    {
        Destroy(gameObject);
    }

}
