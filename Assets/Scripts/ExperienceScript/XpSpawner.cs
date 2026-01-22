using UnityEngine;
using UnityEngine.Pool;

public class XpSpawner : MonoBehaviour
{
    public static XpSpawner Instance;

    [SerializeField] GameObject xpPrefab;

    IObjectPool<GameObject> xpPool;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        xpPool = new ObjectPool<GameObject>(CreateXp, OnGet, OnRelease, null, true, 50, 300);
    }

    GameObject CreateXp()
    {
        GameObject xp = Instantiate(xpPrefab);
        xp.GetComponent<XpDrop>().SetPool(xpPool);
        xp.SetActive(false);
        return xp;
    }

    void OnGet(GameObject xp)
    {
        xp.SetActive(true);
    }

    void OnRelease(GameObject xp)
    {
        xp.SetActive(false);
    }

    public void SpawnXp(Vector3 position, int xpAmount)
    {
        GameObject xp = xpPool.Get();
        xp.transform.position = position;
        xp.GetComponent<XpDrop>().Init(xpAmount);
    }
}