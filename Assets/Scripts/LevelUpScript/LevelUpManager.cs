using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    [SerializeField] private WeaponInventory weaponInventory;
    [SerializeField] private GameObject levelUpWindow;
    [SerializeField] private Button[] choiceButtons = new Button[3];

    public event Action OnWeaponChoice;
    public event Action OnUpgradeChoice;

    private void Start()
    {
        levelUpWindow.SetActive(false);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int index = i;
            choiceButtons[i].onClick.AddListener(() => HandleChoice(index));
        }
    }

    public void OpenLevelUp()
    {
        levelUpWindow.SetActive(true);
        UpdateChoices();
    }

    private void UpdateChoices()
    {
        if (weaponInventory.IsFull())
        {
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                choiceButtons[i].interactable = true;
                choiceButtons[i].GetComponentInChildren<Text>().text = "Upgrade";
            }
        }
        else
        {
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                choiceButtons[i].interactable = true;
                choiceButtons[i].GetComponentInChildren<Text>().text = "Weapon / Upgrade";
            }
        }
    }

    private void HandleChoice(int index)
    {
        levelUpWindow.SetActive(false);

        if (weaponInventory.IsFull())
        {
            OnUpgradeChoice?.Invoke();
        }
        else
        {
            OnWeaponChoice?.Invoke();
        }
    }
}
