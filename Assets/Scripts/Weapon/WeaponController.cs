using UnityEngine;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    private readonly List<Weapon> weapons = new();

    public void RegisterWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
    }

    void Update()
    {
        float dt = Time.deltaTime;

        foreach (var weapon in weapons)
            weapon.Update(dt);
    }
}