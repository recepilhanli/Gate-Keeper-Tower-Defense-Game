using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;


namespace Game.PlayerOperations
{
    //Player.Combat 
    public partial class Player
    {

        private bool hasAttackedWithRightHand = false;

        private void InitCombat()
        {
            onTakeDamage += OnPlayerTakeDamage;
            onDeath += OnPlayerDeath;

        }


        private void Punch()
        {

        }

        private void OnPlayerTakeDamage(DamageData data)
        {
            Tween.ShakeScale(transform, new Vector3(.2f, .2f, .2f), .2f, 5, easeBetweenShakes: Ease.OutQuart);
        }

        private void OnPlayerDeath(DeathReason reason = DeathReason.Standart)
        {

        }


    }

}