using System;
using UnityEngine;

public sealed class WeaponInventory : MonoBehaviour
{
    public const int MaxSlots = 4;

    public event Action InventoryChanged;
    public event Action InventoryBecameFull;

    private readonly Weapon[] _slots = new Weapon[MaxSlots];
    private bool _isFullCached;

    public bool TryAddWeapon(Weapon weapon)
    {
        if (weapon == null)
        {
            return false;
        }

        for (int i = 0; i < MaxSlots; i++)
        {
            if (_slots[i] == null)
            {
                _slots[i] = weapon;
                InventoryChanged?.Invoke();

                CheckFullState();
                return true;
            }
        }

        return false;
    }

    public Weapon GetWeaponInSlot(int index)
    {
        if (index < 0 || index >= MaxSlots)
        {
            return null;
        }

        return _slots[index];
    }

    public int GetWeaponCount()
    {
        int count = 0;

        for (int i = 0; i < MaxSlots; i++)
        {
            if (_slots[i] != null)
            {
                count++;
            }
        }

        return count;
    }

    public bool IsFull()
    {
        for (int i = 0; i < MaxSlots; i++)
        {
            if (_slots[i] == null)
            {
                return false;
            }
        }

        return true;
    }

    private void CheckFullState()
    {
        bool isNowFull = IsFull();

        if (isNowFull && !_isFullCached)
        {
            _isFullCached = true;
            InventoryBecameFull?.Invoke();
        }
    }
}
