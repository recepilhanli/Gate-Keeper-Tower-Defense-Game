using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float range = 5f;
    public float damage = 10f;
    public bool canDamagePlayer = true;
    public bool fadeDamage = false;

#if UNITY_EDITOR
    [SerializeField] private bool _visualize = false;
#endif

    private void OnEnable()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.layer == LayerMasks.LAYER_PLAYER && !canDamagePlayer) continue;
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                if (!fadeDamage) damageable.Damage(new DamageData(damage, transform.position));
                else
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    float damageAmount = damage - (damage * (distance / range));
                    damageable.Damage(new DamageData(damageAmount, transform.position));
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!_visualize) return;
        if (!fadeDamage)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, range / 2);
        }
    }
#endif
}
