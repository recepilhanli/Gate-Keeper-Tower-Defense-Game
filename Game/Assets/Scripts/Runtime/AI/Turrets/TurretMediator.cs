using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMediator : MediatorInstance<TurretMediator, TurretPayload>
{

    public TurretMediator()
    {

        EnemyManager.enemies.OnRemove += OnEnemyRemovedFromScene;
    }

    private List<FiringTurret> _firingTurets = new List<FiringTurret>();

    private void OnEnemyRemovedFromScene(AEnemy enemy)
    {
        for (int i = 0; i < _firingTurets.Count; i++)
        {
            if (_firingTurets[i].enemyFiringAt == enemy)
            {
                _firingTurets.RemoveAt(i);
                i--;
            }
        }
    }

    private void OnEnemyAddedToScene(AEnemy enemy)
    {

    }

    protected override void OnReceivePayload(IMediatorReceiver<TurretPayload> sender, TurretPayload payload)
    {
        if (payload.enemyToFireAt != null && IsEnemyOnFire(payload.enemyToFireAt))
        {
            if (_firingTurets.Count >= EnemyManager.enemies.Count)
            {
                ApproveFiring(sender, payload);
                return;
            }
            else
            {
                var rand = Random.Range(0, EnemyManager.enemies.Count);
                payload.enemyToFireAt = EnemyManager.enemies[rand];
                ApproveFiring(sender, payload);
            }

        }
        else if (payload.enemyToFireAt != null)
        {
            ApproveFiring(sender, payload);
            return;
        }

    }




    private void ApproveFiring(IMediatorReceiver<TurretPayload> requester, TurretPayload payload)
    {
        _firingTurets.Add(new FiringTurret() { turret = requester, enemyFiringAt = payload.enemyToFireAt });
        SendPayloadToReceiver(requester, payload);
    }

    public bool IsEnemyOnFire(AEnemy enemy)
    {
        for (int i = 0; i < _firingTurets.Count; i++)
        {
            if (_firingTurets[i].enemyFiringAt == enemy)
            {
                return true;
            }
        }
        return false;
    }


    public struct FiringTurret
    {
        public IMediatorReceiver<TurretPayload> turret;
        public AEnemy enemyFiringAt;
    }
}

public struct TurretPayload : IMediatorPayload
{

    public AEnemy enemyToFireAt;


}