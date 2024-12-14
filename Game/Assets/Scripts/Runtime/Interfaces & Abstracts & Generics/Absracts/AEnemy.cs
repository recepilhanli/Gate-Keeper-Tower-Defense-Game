using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Debug = Game.Utils.Logger.Debug;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(NavMeshAgent))]
public abstract class AEnemy : AEntity
{
    private const float MAXIMUM_PHYSICAL_TIME = 5f;

    public NavMeshAgent navMeshAgent;
    public Rigidbody rb;
    public AEntity target = null;

    public bool isDead { get; protected set; } = false;
    public virtual bool isAvailable => !isDead && !isPhysical;
    protected bool _isPhysical = false;
    private float _physicalTime = 0;

    //Signals
    public UnityAction onBecomePhysical;
    public UnityAction onBecomeAgent;

    protected virtual async UniTaskVoid LifeCycle()
    {
        await UniTask.Delay(1000);
    }

    /// <summary>
    /// Check is not physical
    /// </summary>
    /// <returns></returns>
    protected UniTask UnusualStateBlock()
    {
        return UniTask.WaitUntil(CheckUnusualState);
    }

    public bool isPhysical
    {
        get => _isPhysical;
        protected set
        {
            _physicalTime = Time.timeSinceLevelLoad;
            bool hasChanged = _isPhysical != value;
            if (!hasChanged) return;

            _isPhysical = value;
            rb.isKinematic = !value;
            navMeshAgent.enabled = !value;

            if (value)
            {
                GroundChecker().Forget();
                onBecomePhysical?.Invoke();
            }
            else onBecomeAgent?.Invoke();
        }
    }



    private void Awake()
    {
        rb.freezeRotation = true;
        LifeCycle().Forget();
    }


    protected virtual bool CheckUnusualState()
    {
        return !isPhysical;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && _physicalTime + .25f < Time.timeSinceLevelLoad && _isPhysical && !isDead)
        {
            isPhysical = false;
            Debug.Log("Grounded");
        }


    }



    public void AddForce(Vector3 force)
    {
        isPhysical = true;
        rb.AddForce(force, ForceMode.Impulse);
    }

    public void MovePosition(Vector3 position)
    {
        isPhysical = true;
        Tween.RigidbodyMovePosition(rb, position, .2f, ease: Ease.OutQuart);
    }


    private async UniTaskVoid GroundChecker()
    {
        await UniTask.Delay(1000);
        while (_isPhysical && !isDead)
        {
            if (Time.timeSinceLevelLoad - _physicalTime > MAXIMUM_PHYSICAL_TIME)
            {
                Kill();
                return;
            }

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    isPhysical = false;
                }
            }

            await UniTask.Delay(200);

            Debug.Log("GroundChecker");
        }
    }


    public override void Damage(DamageData data)
    {
        if (isDead) return;

        health -= data.damage;
        onTakeDamage?.Invoke(data);
        if (health <= 0)
        {
            Kill();
        }
    }

    public override void Kill()
    {
        if (isDead) return;
        isDead = true;
        onDeath?.Invoke();
    }
}
