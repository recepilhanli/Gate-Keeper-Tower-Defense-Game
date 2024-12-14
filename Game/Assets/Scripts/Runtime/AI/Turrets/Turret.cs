using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.PlayerOperations;
using PrimeTween;
using UnityEngine;

namespace Game.AI
{
    using Debug = Utils.Logger.Debug;
    public class Turret : ABuild, IMediatorReceiver<TurretPayload>
    {
        public static MediatorInstance<TurretMediator, TurretPayload> mediator => MediatorInstance<TurretMediator, TurretPayload>.instance;
        public Transform head;
        public List<ParticleSystem> muzzles = new List<ParticleSystem>();
        public GameObject bulletPrefab;
        [Tooltip("In Seconds")] public float fireRate = 1f;
        public float fireDistance = 20f;
        private AEnemy _enemyFiringAt = null;
        private int _lastMuzzleFired = 0;


        private void Awake()
        {
            LifeCycle().Forget();
        }

        private async UniTaskVoid LifeCycle()
        {
            var token = this.GetCancellationTokenOnDestroy();
            while (!token.IsCancellationRequested)
            {
                await UniTask.WhenAll(LookForEnemy(), Firing());
            }
        }

        private async UniTask LookForEnemy()
        {

            if (_enemyFiringAt == null) //only once
            {
                _ = Tween.LocalEulerAngles(head, Vector3.zero, new Vector3(0, 180, 0), 5f, Ease.Linear, -1, CycleMode.Yoyo);
            }

            while (_enemyFiringAt == null)
            {
                foreach (var enemy in EnemyManager.enemies)
                {
                    if (enemy == null) continue;
                    if (Vector3.Distance(enemy.transform.position, transform.position) < fireDistance)
                    {
                        SendMessageToMeadiator(new TurretPayload() { enemyToFireAt = enemy }); //request fire
                        break;
                    }
                }
                await UniTask.Delay(500);
            }
        }

        private async UniTask Firing()
        {
            Tween.StopAll(head);
            LookAtEnemy().Forget();

            while (_enemyFiringAt != null)
            {
                Fire();
                await UniTask.WaitForSeconds(fireRate);
            }

        }

        private async UniTaskVoid LookAtEnemy()
        {
            while (_enemyFiringAt != null)
            {
                head.LookAt(_enemyFiringAt.transform);
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            }
        }




        public void Fire()
        {
            if (muzzles.Count == 0)
            {
                Debug.Log("No muzzles found", "red");
                return;
            }

            if (_enemyFiringAt == null)
            {
                Debug.Log("No enemy to fire at", "red");
                return;
            }

            _lastMuzzleFired++;
            if (_lastMuzzleFired >= muzzles.Count) _lastMuzzleFired = 0;

            var muzzle = muzzles[_lastMuzzleFired];
            Quaternion toEnemy = Quaternion.LookRotation(_enemyFiringAt.transform.position - muzzle.transform.position);
            var bullet = Instantiate(bulletPrefab, muzzle.transform.position, toEnemy);
        }

        public void OnReceiveMessage(TurretPayload payload)
        {
            if (payload.enemyToFireAt != null)
            {
                _enemyFiringAt = payload.enemyToFireAt;
            }
        }

        public void SendMessageToMeadiator(TurretPayload payload) => mediator.SendPayload(this, payload);

    }

}