using UnityEngine;
using UnityEngine.Pool;

public class PlayerXp : MonoBehaviour
{
    ExperienceManager experienceManager;
    
    IObjectPool<GameObject>[] xpPools;

    void Awake()
    {
        experienceManager = FindFirstObjectByType<ExperienceManager>();

        if (experienceManager == null)
        {
            Debug.LogError("ExperienceManager not found in scene");
        }
    }

    public void AddXP(int amount)
    {
        experienceManager?.AddExperience(amount);
    }
}
