using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;



//EffectManager.SpeedLines
//Object Pool Design Pattern will be implemented in the future.........
public partial class EffectManager
{   
    [Header("Explosion")]
    public Explosion MagicExplosion;
    public GameObject puffEffect;

    public void CreateMagicExplosion(Vector3 position,float range, float damage,bool canDamagePlayer = true)
    {
        var explosion = Instantiate(MagicExplosion, position, Quaternion.identity);
        explosion.range = range;
        explosion.damage = damage;  
        explosion.canDamagePlayer = canDamagePlayer;
        explosion.gameObject.SetActive(true);
        Destroy(explosion.gameObject, 3);
    }

    public void CreatePuffEffect(Vector3 position)
    {
        var puff = Instantiate(puffEffect, position, Quaternion.identity);
        puff.SetActive(true);
        Destroy(puff, 3);
    }
}
