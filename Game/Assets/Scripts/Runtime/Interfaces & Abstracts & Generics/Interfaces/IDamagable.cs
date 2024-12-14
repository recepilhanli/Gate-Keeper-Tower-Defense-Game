
using UnityEngine;

/// <summary>
/// Interface for objects that can take damage.
/// </summary>
public interface IDamageable
{
    public void Damage(DamageData data);
    public void Kill(DeathReason reason);
}

public struct DamageData
{
    public AEntity attacker;
    public float damage;
    public Vector3 hitPoint;
    public DamageType damageType;

    public DamageData(float damage, Vector3 hitPoint = default, DamageType damageType = DamageType.Standart)
    {
        this.damage = damage;
        this.hitPoint = hitPoint;
        this.damageType = damageType;
        attacker = null;
    }

    public DamageData(AEntity attacker, float damage, Vector3 hitPoint = default, DamageType damageType = DamageType.Standart)
    {
        this.damage = damage;
        this.hitPoint = hitPoint;
        this.damageType = damageType;
        this.attacker = attacker;
    }

}



public enum DamageType
{
    Standart,
    Wind,
    Bowling,
    Stun
}

public enum DeathReason
{
    Standart,
    Suicide,
}

