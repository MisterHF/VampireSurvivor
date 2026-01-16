using UnityEngine;

public class XPDrop : MonoBehaviour
{
    int xpAmount;

    public void Init(int amount)
    {
        xpAmount = amount;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerXp playerXpReceiver= other.GetComponent<PlayerXp>();
        if (playerXpReceiver != null)
        {
            playerXpReceiver.AddXP(xpAmount);
        }
        Destroy(gameObject);
    }
}
