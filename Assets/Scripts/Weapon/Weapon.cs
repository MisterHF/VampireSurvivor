using UnityEngine;

public abstract class Weapon : IWeapon
{
    protected WeaponData data;
    protected float timer;
    protected Transform player;

    public Weapon(WeaponData data)
    {
        this.data = data;
    }

    protected Weapon(WeaponData data, Transform owner)
    {
        this.data = data;
        this.player = owner;
        timer = 0f;
    }

    public void Update(float deltaTime)
    {
        timer += deltaTime;

        if (timer >= data.cooldown)
        {
            timer = 0f;
            Attack();
        }
    }

    protected abstract void Attack();
}