using System.Collections;
using System.Collections.Generic;
using Game.PlayerOperations.Skills;
using UnityEngine;
using UnityEngine.Events;

namespace Game.PlayerOperations
{
    //Player.Skills
    public partial class Player
    {
        [Header("Skill Related")]
        public GameObject bowlingGameObject;
        public GameObject speedLines;

        private void InitSkills()
        {
            SkillDatabase.LoadSkills();
            speedLines.SetActive(false);
        }
    }
}