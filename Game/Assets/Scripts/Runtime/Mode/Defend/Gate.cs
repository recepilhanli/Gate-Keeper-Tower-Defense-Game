using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

namespace Game.Modes
{
    public class DefendingObject : EntityInstance<DefendingObject>
    {
        void Start()
        {
            onTakeDamage += OnGateTakeDamage;
            onDeath += OnGateDestroyed;
        }

        private void OnGateTakeDamage(DamageData data)
        {
            Vector3 strength = new Vector3(.2f, .2f, .2f);
            Tween.ShakeScale(transform, strength, .2f, 5, easeBetweenShakes: Ease.OutQuart);
        }

        private void OnGateDestroyed()
        {
            GameManager.instance.Fail();
        }
    }


}
