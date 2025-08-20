using UnityEngine;

public class Giver : MonoBehaviour
{
    [SerializeField] int givingCoin = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"+{givingCoin}");
            Destroy(gameObject);
        }
    }
}
