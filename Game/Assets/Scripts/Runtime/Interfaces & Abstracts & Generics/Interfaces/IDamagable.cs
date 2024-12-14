
using UnityEngine;

/// <summary>
/// Interface for objects that can take damage.
/// </summary>
public interface IDamagable
{
    public void Damage(DamageData data);
    public void Kill();
}

public struct DamageData
{
    public float damage;
    public Vector3 hitPoint;
    public DamageType damageType;

    public DamageData(float damage, Vector3 hitPoint = default, DamageType damageType = DamageType.Standart)
    {
        this.damage = damage;
        this.hitPoint = hitPoint;
        this.damageType = damageType;
    }

}



public enum DamageType
{
    Standart,
    Wind,
    Bowling,
    Stun
}

