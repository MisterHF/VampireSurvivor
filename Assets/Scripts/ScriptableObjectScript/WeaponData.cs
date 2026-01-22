using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObject/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Attack Settings")]
    public float baseDamageValue;
    public float cooldown;
    public float range;
    public int maxTargets;

    [Header("Identity")]
    public string weaponName;
    public Sprite icon;
    public string description;

    [Header("Prefab Projectile")]
    public GameObject prefabProjectile;
}