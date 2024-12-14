using System;
using System.Collections;
using System.Collections.Generic;
using Game.PlayerOperations.Skills;
using UnityEngine;

using static UnityEngine.InputSystem.InputAction;

namespace Game.PlayerOperations
{
    using Debug = Utils.Logger.Debug;
    //Player.Input
    public partial class Player
    {
        [NonSerialized] public Vector2 movementInput;

        public void OnMove(CallbackContext value) => movementInput = value.ReadValue<Vector2>();
        public void OnBowling(CallbackContext value)
        {
            if (value.performed)
            {
                 SkillDatabase.GetSkill(Skill.Bowling).UseSkill();
                 Debug.Log("Bowling","cyan");
            }
        }
        public void OnPunch(CallbackContext value) => Punch();
    }
}