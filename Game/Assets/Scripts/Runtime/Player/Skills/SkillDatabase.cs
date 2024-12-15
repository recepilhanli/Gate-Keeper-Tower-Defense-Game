using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.PlayerOperations.Skills
{
    /// <summary>
    /// SkillDatabase class is a static class that holds all the skills in the game.
    /// </summary>
    public static class SkillDatabase
    {
        private static Dictionary<Skill, PlayerSkill> _skills = new Dictionary<Skill, PlayerSkill>();
        public static void LoadSkills()
        {
            AddSkill(BowlingSkill.Register());
        }

        public static PlayerSkill GetSkill(Skill skill) => _skills[skill];

        private static void AddSkill(PlayerSkill skill)
        {
            if (skill.skillType == Skill.Passive)
            {
                Debug.LogError("Passive skill cannot be added to the database");
                return;
            }
            _skills.Add(skill.skillType, skill);
        }

        public static void  ReleaseAllSkils() => _skills.Clear();

    }
}

