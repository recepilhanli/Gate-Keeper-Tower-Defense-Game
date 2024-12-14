using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

namespace Game.AI
{
    using Debug = Utils.Logger.Debug;
    public class BasicEnemy : AEnemy
    {
        [Header("Basic Enemy")]
        [SerializeField] float _attackDamage = 5f;
        [SerializeField, Tooltip("in seconds")] float _attacRate = 1f;

        private DamageType _lastDamageType = DamageType.Standart;


        public override bool isAvailable => !isDead && !isPhysical && target != null;

        private void Start()
        {
            onDeath += () =>
            {
                Sequence.Create().OnComplete(() => Destroy(gameObject))
                .Chain(Tween.Scale(transform, transform.lossyScale.x * 1.5f, 0.5f, Ease.OutElastic))
                .Chain(Tween.Scale(transform, 0, 0.2f, Ease.InElastic));
            };

            onTakeDamage += (damageData) =>
            {
                _lastDamageType = damageData.damageType;
            };
        }


        protected override async UniTaskVoid LifeCycle()
        {
            while (!isDead)
            {
                await UnusualStateBlock();
                await UniTask.WhenAll(GetNearToTarget(), Attack());
            }
        }

        protected override bool CheckUnusualState()
        {
            return !isPhysical && target != null;
        }

        private async UniTask GetNearToTarget()
        {
            while (isAvailable && Vector3.Distance(transform.position, target.transform.position) > navMeshAgent.stoppingDistance + .2f)
            {
                navMeshAgent.SetDestination(target.transform.position);
                await UniTask.Delay(100);
                Debug.Log("[Test] GetNearToTarget", "orange");
            }
        }

        private async UniTask Attack()
        {
            while (isAvailable && Vector3.Distance(transform.position, target.transform.position) <= navMeshAgent.stoppingDistance + .2f)
            {
                if (navMeshAgent.hasPath) navMeshAgent.ResetPath();
                target.Damage(new DamageData(_attackDamage));
                await UniTask.WaitForSeconds(_attacRate);
                Debug.Log("[Test] Attack", "orange");
            }
        }
    }

}