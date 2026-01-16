using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObject/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float angularSpeed = 120f;
    public float acceleration = 8f;

    [Header("Attack Settings")]
    public float attackRange = 2f;
    public int damage = 10;
    public float attackCooldown = 1f;

    [Header("Health Settings")]
    public float maxHealth = 100f;

    [Header("Xp Settings")]
    public int xpAmount = 100;

    [Header("Drops")]
    public GameObject xpDropPrefab;

}
