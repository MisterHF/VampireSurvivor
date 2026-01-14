using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceManager : MonoBehaviour
{
    [Header("Experience")]
    [SerializeField] AnimationCurve experienceCurve;

    int currentLevel, totalExperience;
    int previousLevelsExperience, nextLevelsExperience;

    [Header("Interface")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Slider experienceSlider;

    void Start()
    {
        UpdateLevel();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddExperience(10);
        }
    }

    public void AddExperience(int amount)
    {
        totalExperience += amount;
        CheckForLevelUp();
        UpdateInterface();
    }

    void CheckForLevelUp()
    {
        if (totalExperience >= nextLevelsExperience)
        {
            currentLevel++;
            UpdateLevel();
        }
    }

    void UpdateLevel()
    {
        previousLevelsExperience = (int)experienceCurve.Evaluate(currentLevel);
        nextLevelsExperience = (int)experienceCurve.Evaluate(currentLevel + 1);
        UpdateInterface();
    }

    void UpdateInterface()
    {
        int currentXP = totalExperience - previousLevelsExperience;
        int requiredXP = nextLevelsExperience - previousLevelsExperience;

        levelText.text = currentLevel.ToString();
        experienceSlider.minValue = 0;
        experienceSlider.maxValue = requiredXP;
        experienceSlider.value = currentXP;
    }
}