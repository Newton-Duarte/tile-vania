using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int coinPoints = 1;
    [SerializeField] AudioClip coinSfx;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindAnyObjectByType<GameManager>().SetCoin(coinPoints);
            FindAnyObjectByType<GameManager>().PlaySFXClip(coinSfx);
            Destroy(gameObject);
        }
    }
}
