using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int hitPoints = 2;

    Rigidbody2D rb;
    FlashEffect flashEffect;
    bool isTakingDamage = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flashEffect = GetComponent<FlashEffect>();
    }

    void Update()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        transform.localScale = new Vector2(Mathf.Sign(moveSpeed) * Math.Abs(transform.localScale.x), transform.localScale.y);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Bullet")
        {
            moveSpeed = -moveSpeed;
            Flip();
        }
    }

    public void TakeDamage(int bulletDamage)
    {
        if (!isTakingDamage)
        {
            isTakingDamage = true;

            flashEffect.Flash();
            hitPoints -= bulletDamage;
            Invoke(nameof(ResetIsTakingDamage), 0.1f);

            if (hitPoints <= 0)
            {
                Die();
            }
        }
    }

    public float MoveSpeed => moveSpeed;

    public void SetMoveSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void ResetIsTakingDamage()
    {
        isTakingDamage = false;
    }
}
