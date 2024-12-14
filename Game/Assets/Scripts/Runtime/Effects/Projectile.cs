using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Effects
{
    //Object Pooling Design Pattern Will be implemented here.....
    public class Projectile : MonoBehaviour
    {
        private const float MAXIMUM_LIFE_TIME = 10f;

        public bool onlyHitEnemies = true;
        public float speed = 10f;
        public float damage = 10f;
        public Rigidbody rb;
        public GameObject hitEffect;
        private float _destroyTime = 0f;


        private void OnEnable()
        {
            if (onlyHitEnemies) rb.excludeLayers = LayerMasks.LAYER_PLAYER;
        }

        private void Update()
        {
            _destroyTime += Time.deltaTime;
            rb.velocity = transform.forward * speed;
            if (_destroyTime >= MAXIMUM_LIFE_TIME) Destroy(gameObject);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(new DamageData(null, 10, transform.position, DamageType.Bowling));
            }

            if (hitEffect != null)
            {
                var contact = collision.contacts[0];
                var rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                var contactPos = contact.point;
                var effect = Instantiate(hitEffect, contactPos, transform.rotation);
                Destroy(effect, 1.5f);
            }

            Destroy(gameObject);
        }
    }
}