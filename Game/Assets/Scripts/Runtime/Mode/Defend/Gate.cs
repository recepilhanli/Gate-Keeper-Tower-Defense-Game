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
            onDeath += (reason) => { OnGateDestroyed(); };
        }

        private void OnGateTakeDamage(DamageData data)
        {
            Vector3 strength = new Vector3(.5f, .3f, .2f);
            Tween.ShakeScale(transform, strength, .3f, 5, easeBetweenShakes: Ease.OutQuart);
        }

        private void OnGateDestroyed()
        {
            GameManager.instance.Fail();
        }
    }


}
