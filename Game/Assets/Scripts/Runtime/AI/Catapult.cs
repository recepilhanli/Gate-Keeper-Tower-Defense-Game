using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Effects;
using Game.Modes;
using Game.PlayerOperations;
using PrimeTween;
using UnityEngine;

namespace Game.AI
{
    using Debug = Utils.Logger.Debug;
    public class Catapult : AEnemy
    {
        [Header("Catapult")]
        [SerializeField] float _attackDamage = 25f;
        [SerializeField, Tooltip("in seconds")] float _attacRate = 10f;
        [SerializeField] private float _attackRange = 30f;
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private Projectile _projectile;

        public override bool isAvailable => !isDead && target != null;

        private void Start()
        {
            onDeath += (reason) =>
            {
                EffectManager.instance.CreatePuffEffect(transform.position, 3);
                Sequence.Create().OnComplete(() => Destroy(gameObject))
                .Group(Tween.Scale(transform, transform.lossyScale.x * 1.5f, 0.5f, Ease.OutElastic))
                .Group(Tween.Scale(transform, 0, 0.2f, Ease.InElastic));

                GameManager.instance.currency += 15;
            };

            onTakeDamage += (damageData) =>
            {
                Tween.ShakeScale(transform, new Vector3(.4f, .4f, .4f), .35f, 5, easeBetweenShakes: Ease.OutQuart);
                if (damageData.attacker != null)
                {
                    if (target != null) return;
                    if (damageData.attacker.CompareTag("Enemy")) return;
                    target = damageData.attacker;

                    if (target.CompareTag("Player")) navMeshAgent.stoppingDistance = 1.5f;
                    else navMeshAgent.stoppingDistance = 4.25f;
                }

                GameManager.instance.currency += 1;
            };
        }


        protected override async UniTaskVoid LifeCycle()
        {
            while (!isDead)
            {
                if (target == null)
                {
                    GameManager.instance.GuideEnemy(this);
                    await UniTask.Delay(500);
                }
                await UniTask.WhenAll(GetNearToTarget(), Attack());
            }
        }



        private async UniTask GetNearToTarget()
        {
            while (isAvailable && Vector3.Distance(transform.position, target.transform.position) > _attackRange)
            {
                navMeshAgent.SetDestination(target.transform.position);
                await UniTask.Delay(100);
                Debug.Log("[Test] GetNearToTarget", "orange");
            }
        }




        private async UniTask Attack()
        {

            if (isAvailable && Vector3.Distance(transform.position, target.transform.position) <= _attackRange + 5) LookAtTarget().Forget();

            while (isAvailable && Vector3.Distance(transform.position, target.transform.position) <= _attackRange + 5)
            {
                if (navMeshAgent.hasPath) navMeshAgent.ResetPath();


                _ = Sequence.Create()
                .Chain(Tween.LocalEulerAngles(_head, new Vector3(0, 0, 10), new Vector3(0, 0, -90), .5f, Ease.OutQuart).OnComplete(Fire))
                .Chain(Tween.LocalEulerAngles(_head, new Vector3(0, 0, -90), new Vector3(0, 0, 10), 1f, Ease.InQuart));


                await UniTask.WaitForSeconds(_attacRate);
                Debug.Log("[Test] Attack", "orange");
            }
        }

        private async UniTaskVoid LookAtTarget()
        {
            while (isAvailable && target != null)
            {
                var rot = Quaternion.LookRotation(target.transform.position - transform.position);
                rot.x = 0;
                rot.z = 0;
                _ = Tween.Rotation(transform, rot, 0.1f);
                await UniTask.Delay(100);
            }
        }


        private void Fire()
        {
            var rot = Quaternion.LookRotation(target.transform.position - _firePoint.position);
            Projectile projectile = Instantiate(_projectile, _firePoint.position, rot);
            projectile.sender = this;
            if (Vector3.Distance(transform.position, target.transform.position) > 15) projectile.damage = _attackDamage;
            else target.Damage(new DamageData(this, _attackDamage));
        }
    }


}