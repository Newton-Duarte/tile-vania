using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] Transform jumpArea;
    [SerializeField] float jumpForce = 50f;
    [SerializeField] float forceMultiplier = 1.25f;
    [SerializeField] AudioClip jumpClip;

    GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        float jumpAreaY = jumpArea.position.y;
        float collisionPointY = collision.GetContact(0).point.y;
        bool collidesFromAbove = collisionPointY > jumpAreaY;

        if (collidesFromAbove)
        {
            bool isHoldingJumpButton = Input.GetButton("Jump");
            collision.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(0f, isHoldingJumpButton ? jumpForce * forceMultiplier : jumpForce);
            gameManager.PlaySFXClip(jumpClip);
        }
    }
}
