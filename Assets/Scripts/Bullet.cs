using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] int bulletDamage = 1;
    [SerializeField] AudioClip bulletClip;

    PlayerMovement player;

    Rigidbody2D rb;
    float xSpeed;

    void Start()
    {
        player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;
        rb = GetComponent<Rigidbody2D>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        rb.velocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            FindAnyObjectByType<GameManager>().PlaySFXClip(bulletClip);
            collision.gameObject.GetComponent<EnemyMovement>().TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            FindAnyObjectByType<GameManager>().PlaySFXClip(bulletClip);
            collision.gameObject.GetComponent<EnemyMovement>().TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }
}
