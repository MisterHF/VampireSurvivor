using UnityEngine;

public class PlayerXp : MonoBehaviour
{
    ExperienceManager experienceManager;

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
