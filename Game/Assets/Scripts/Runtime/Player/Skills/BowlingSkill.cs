using System.Collections;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;


namespace Game.PlayerOperations.Skills
{
    using Debug = Utils.Logger.Debug;
    public sealed class BowlingSkill : SkillInstance<BowlingSkill>
    {
        public BowlingSkill()
        {
            player.onCollisionEnter += (collision) =>
             {
                 if (_isBowling)
                 {
                     if (collision.gameObject.CompareTag("Enemy"))
                     {
                         if (collision.gameObject.TryGetComponent(out AEnemy enemy))
                         {
                             enemy.Damage(new DamageData(player, 10, player.transform.position, DamageType.Bowling));
                             enemy.AddForce((player.transform.forward + Vector3.up) * 3);
                             player.CameraImpulse(new Vector3(0, 0, 2), .2f);
                             EffectManager.instance.SetChromaticAbernationIntensity(.5f, .25f);
                         }
                     }
                 }
             };


        }


        private bool _isBowling = false;
        public override Skill skillType { get; } = Skill.Bowling;
        private Sequence _sequence;

        protected override void Perform()
        {
            _isBowling = !_isBowling;


            SetBowlingState(_isBowling);

            if (_isBowling)
            {
                player.onBowlingAttackStart?.Invoke();
            }
            else
            {
                player.onBowlingAttackEnd?.Invoke();
            }



        }

        private void SetBowlingState(bool state)
        {
            EffectManager.instance.SetEnableSpeedLines(state);

            Tween.StopAll(player.bowlingGameObject.transform);
            Tween.StopAll(player.playerMesh.transform);

            if (_sequence.isAlive) _sequence.Stop();

            if (state)
            {
                player.blockMovement = true;
                _sequence = Sequence.Create()
                  .Group(Tween.Scale(player.bowlingGameObject.transform, endValue: 1.5f, duration: .25f, Ease.InQuad))
                  .Group(Tween.Scale(player.playerMesh.transform, endValue: 0f, duration: .25f, Ease.InQuad)).OnComplete(BowlingMovement().Forget);
                Tween.LocalRotation(player.bowlingGameObject.transform, new Vector3(0, 0, 0), new Vector3(270, 0, 0), .5f, Ease.Linear, -1, CycleMode.Yoyo);
            }
            else
            {
                player.rigidBody.velocity = player.rigidBody.velocity / 2;
                _sequence = Sequence.Create()
                  .Group(Tween.Scale(player.bowlingGameObject.transform, endValue: 0f, duration: .5f, Ease.OutQuad))
                  .Group(Tween.Scale(player.playerMesh.transform, endValue: 1f, duration: .5f, Ease.OutQuad)).OnComplete(EnablePlayerMovement);
            }
        }

        private void EnablePlayerMovement()
        {
            player.blockMovement = false;
        }


        private async UniTaskVoid BowlingMovement()
        {

            player.directionArrow.SetActive(true);
            _ = Tween.ScaleZ(player.directionArrow.transform, 0, 1, .5f, Ease.InQuad);

            var cToken = player.GetCancellationTokenOnDestroy();

            while (_isBowling && !cToken.IsCancellationRequested)
            {
                Vector3 movement = player.transform.forward * 25;
                movement += new Vector3(0, player.rigidBody.velocity.y, 0);

                //prevent shaking
                float mouseDistance = Vector3.Distance(player.lastMouseHitPoint, player.transform.position);
                mouseDistance = Mathf.Clamp(mouseDistance, 0.05f, 1);

                //set direction arrow material (_ = for ignoring warnings)
                _ = Tween.MaterialProperty(player.directionArrowMaterial, ShaderPorperties.alpha, mouseDistance, duration: .1f, ease: Ease.Linear);
                _ = Tween.MaterialProperty(player.directionArrowMaterial, ShaderPorperties.speed, mouseDistance * 2.5f, duration: .1f, ease: Ease.Linear);

                movement *= mouseDistance;
                player.rigidBody.velocity = movement;

                await UniTask.Yield(cToken);
            }
            player.directionArrow.SetActive(false);
        }
    }
}
