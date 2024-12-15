using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Modes
{
    public class DefendingObject : EntityInstance<DefendingObject>
    {
        [SerializeField] private Image healthFill;
        [SerializeField] private TextMeshProUGUI _healthTMP;
        void Start()
        {
            onTakeDamage += OnGateTakeDamage;
            onDeath += (reason) => { OnGateDestroyed(); };
        }

        private void OnGateTakeDamage(DamageData data)
        {
            Vector3 strength = new Vector3(.5f, .3f, .2f);
            Tween.ShakeScale(transform, strength, .3f, 5, easeBetweenShakes: Ease.OutQuart);
            healthFill.fillAmount = health / 100;

            Sequence.Create()
            .Group(Tween.ShakeLocalPosition(healthFill.transform, new Vector3(1, 1, 1), .1f, 5, easeBetweenShakes: Ease.OutElastic))
            .Chain(Tween.Color(healthFill, Color.red, .3f))
            .Chain(Tween.Color(healthFill, Color.white, .3f));

            _healthTMP.text = $"{health} / 100";
        }

        private void OnGateDestroyed()
        {
            GameManager.instance.Fail();
        }
    }


}
