using System;
using System.Collections;
using System.Collections.Generic;
using Game.PlayerOperations.Skills;
using UnityEngine;

using static UnityEngine.InputSystem.InputAction;

namespace Game.PlayerOperations
{
    //Player.Input
    public partial class Player
    {
        [NonSerialized] public Vector2 movementInput;

        public void OnMove(CallbackContext value) => movementInput = value.ReadValue<Vector2>();
        public void OnBowling(CallbackContext value) => SkillDatabase.GetSkill(Skill.Bowling).UseSkill();
        public void OnPunch(CallbackContext value) => Punch();
    }
}