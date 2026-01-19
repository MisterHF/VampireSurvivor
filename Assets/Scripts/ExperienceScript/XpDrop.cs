using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Collider))]
public class XpDrop : MonoBehaviour
{
    [SerializeField] int xpValue = 1;

    IObjectPool<GameObject> pool;

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