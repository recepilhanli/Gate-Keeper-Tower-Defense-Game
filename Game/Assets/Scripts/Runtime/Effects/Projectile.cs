using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Effects
{
    public class Projectile : MonoBehaviour
    {
        public bool onlyHitEnemies = true;
        public float speed = 10f;
        public Rigidbody rb;


        private void OnEnable()
        {

        }

        private void Update()
        {
            rb.velocity = transform.forward * speed;
        }
    }
}