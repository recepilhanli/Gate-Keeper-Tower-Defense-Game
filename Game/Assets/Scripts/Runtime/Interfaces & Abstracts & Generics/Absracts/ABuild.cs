using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ACharacter is an abstract class that represents a character in the game.
/// </summary>
public abstract class ABuild : AEntity
{
    public UnityAction onBuildComplete;

    public override void Damage(DamageData data)
    {
        if (data.attacker != null && !data.attacker.CompareTag("Enemy")) return;

        health -= data.damage;
        onTakeDamage?.Invoke(data);
        if (health <= 0)
        {
            Kill();
        }
    }

    

}
