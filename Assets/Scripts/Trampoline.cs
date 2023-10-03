using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] Transform jumpPoint;
    [SerializeField] float jumpForce = 50f;
    [SerializeField] AudioClip jumpClip;

    GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        var jumpPointHeight = jumpPoint.position.y;
        var collisionPointHeight = collision.GetContact(0).point.y;

        if (collisionPointHeight > jumpPointHeight)
        {
            bool isHoldingJumpButton = Input.GetButton("Jump");
            collision.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(0f, isHoldingJumpButton ? jumpForce * 1.5f : jumpForce);
            gameManager.PlaySFXClip(jumpClip);
        }
    }
}
