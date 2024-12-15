using System;
using System.Collections;
using System.Collections.Generic;
using Game.Animations;
using Game.Modes;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Game.PlayerOperations
{
    using Debug = Utils.Logger.Debug;

    //Player.Combat 
    public partial class Player
    {
        [Header("Combat")]
        [SerializeField] private Image healthFill;
        [SerializeField] private TextMeshProUGUI _healthTMP;

        private bool hasAttackedWithRightHand = false;
        public bool isDead { get; private set; } = false;

        private void InitCombat()
        {
            onTakeDamage += OnPlayerTakeDamage;
            onDeath += OnPlayerDeath;

        }
        [Header("Combat")]
        public List<TrailRenderer> trails = new List<TrailRenderer>();


        private void Punch()
        {
            hasAttackedWithRightHand = !hasAttackedWithRightHand;
            animator.SetTrigger(hasAttackedWithRightHand ? AnimationTable.Attack1 : AnimationTable.Attack2);

            var hitColliders = Physics.OverlapSphere(transform.position + transform.forward, 1f);

            bool wasHit = false;
            foreach (var hitCollider in hitColliders)
            {
                if (!hitCollider.gameObject.CompareTag("Enemy")) continue;

                if (hitCollider.TryGetComponent(out AEnemy enemy))
                {
                    enemy.Damage(new DamageData(this, 20, transform.position, DamageType.Standart));
                    if (hitCollider.attachedRigidbody != null && hitCollider.attachedRigidbody.mass < 5) enemy.AddForce((transform.forward) + Vector3.up * 5);

                    wasHit = true;
                }

            }
            if (wasHit)
            {
                audioSource.clip = punchClip;
                audioSource.Play();
                CameraImpulse(new Vector3(0, 0, 2), .2f);
            }
        }

        private void OnPlayerTakeDamage(DamageData data)
        {
            if (isDead) return;
            Tween.ShakeScale(transform, new Vector3(.2f, .2f, .2f), .2f, 5, easeBetweenShakes: Ease.OutQuart);

            Sequence.Create()
            .Group(Tween.ShakeLocalPosition(_healthTMP.transform, new Vector3(3f, 3f, 3f), .3f, 15, easeBetweenShakes: Ease.OutElastic))
            .Group(Tween.ShakeLocalPosition(healthFill.transform, new Vector3(1, 1, 1), .1f, 5, easeBetweenShakes: Ease.OutElastic))
            .Chain(Tween.Color(healthFill, Color.red, .15f, ease: Ease.OutQuart))
            .Chain(Tween.Color(healthFill, Color.white, .15f, ease: Ease.InQuart));


            CameraImpulse(new Vector3(0, 0, -2), .15f);

            EffectManager.instance.SetVignetteIntensity(.4f, .5f);
            healthFill.fillAmount = health / 100;
            _healthTMP.text = $"%{health}";



        }

        public void RegenerateHealth()
        {
            health = 100;
            health = Mathf.Clamp(health, 0, 100);
            healthFill.fillAmount = health / 100;
            _healthTMP.text = $"%{health}";
        }

        private void OnPlayerDeath(DeathReason reason = DeathReason.Standart)
        {
            if (isDead) return;
            isDead = true;
            gameObject.SetActive(false);
            GameManager.instance.Fail();

        }


    }

}