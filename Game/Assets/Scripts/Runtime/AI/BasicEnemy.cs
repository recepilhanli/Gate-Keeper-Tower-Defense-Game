using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Modes;
using Game.PlayerOperations;
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
        [SerializeField, Range(1, 5)] private int _level = 1;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private List<AEnemy> _enemiesToSpawnAfterDeath = new List<AEnemy>();

        public override bool isAvailable => !isDead && !isPhysical && target != null;

        private void Start()
        {
            onDeath += (reason) =>
            {
                if (reason == DeathReason.Suicide) EffectManager.instance.CreateMagicExplosion(transform.position, 20, _attackDamage * 10);
                else
                {

                    if (_enemiesToSpawnAfterDeath.Count > 0)
                    {
                        foreach (var enemy in _enemiesToSpawnAfterDeath)
                        {
                            var created = Instantiate(enemy, transform.position, transform.rotation);
                            GameManager.instance.GuideEnemy(created);
                        }
                    }
                    EffectManager.instance.CreatePuffEffect(transform.position);

                    GameManager.instance.currency += 5;
                }
                Sequence.Create().OnComplete(() => Destroy(gameObject))
                .Chain(Tween.Scale(transform, transform.lossyScale.x * 1.5f, 0.5f, Ease.OutElastic))
                .Chain(Tween.Scale(transform, 0, 0.2f, Ease.InElastic));


            };

            onTakeDamage += (damageData) =>
            {
                Tween.ShakeScale(transform, new Vector3(.4f, .4f, .4f), .35f, 5, easeBetweenShakes: Ease.OutQuart);
                if (damageData.attacker != null)
                {
                    if (target != null)
                    {
                        if (target == Player.localPlayerInstance) return;
                        else if (target.CompareTag("Turret")) return;
                    }
                    if (damageData.attacker.CompareTag("Enemy")) return;
                    target = damageData.attacker;

                    if (target.CompareTag("Player")) navMeshAgent.stoppingDistance = 2f;
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
                await UnusualStateBlock();
                await UniTask.WhenAll(GetNearToTarget(), Attack());
            }
        }

        protected override bool CheckUnusualState()
        {
            return !isPhysical;
        }

        private async UniTask GetNearToTarget()
        {
            while (isAvailable && Vector3.Distance(transform.position, target.transform.position) > navMeshAgent.stoppingDistance + .3f)
            {
                navMeshAgent.SetDestination(target.transform.position);
                await UniTask.Delay(100);
                Debug.Log("[Test] GetNearToTarget", "orange");
            }
        }

        private async UniTask Attack()
        {
            while (isAvailable && Vector3.Distance(transform.position, target.transform.position) <= navMeshAgent.stoppingDistance + .5f)
            {
                if (navMeshAgent.hasPath) navMeshAgent.ResetPath();
                target.Damage(new DamageData(_attackDamage));
                await UniTask.WaitForSeconds(_attacRate);
                Debug.Log("[Test] Attack", "orange");
            }
        }
    }

}