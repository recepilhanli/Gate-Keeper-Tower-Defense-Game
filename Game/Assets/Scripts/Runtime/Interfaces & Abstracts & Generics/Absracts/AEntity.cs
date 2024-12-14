using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ACharacter is an abstract class that represents a character in the game.
/// </summary>
public abstract class AEntity : MonoBehaviour, IDamageable
{
    public UnityAction<DamageData> onTakeDamage;
    public UnityAction<DeathReason> onDeath;


    public float health = 100;
    public virtual void Damage(DamageData data)
    {
        health -= data.damage;
        onTakeDamage?.Invoke(data);
        if (health <= 0)
        {
            Kill();
        }
    }

    public virtual void Kill(DeathReason reason = DeathReason.Standart)
    {
        onDeath?.Invoke(reason);
    }
}
