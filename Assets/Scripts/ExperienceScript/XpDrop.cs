using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Collider))]
public class XpDrop : MonoBehaviour
{
    int xpValue;

    IObjectPool<GameObject> pool;

    public void Init(int amount)
    {
        xpValue = amount;
    }
    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerXp playerXp = other.GetComponent<PlayerXp>();
        if (playerXp == null) return;

        playerXp.AddXP(xpValue);
        pool.Release(gameObject);
    }
}