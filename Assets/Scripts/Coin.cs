using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int coinPoints = 1;
    [SerializeField] AudioClip coinSfx;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            FindObjectOfType<GameManager>().SetCoin(coinPoints);
            FindObjectOfType<GameManager>().PlaySFXClip(coinSfx);
            Destroy(gameObject);
        }
    }
}
