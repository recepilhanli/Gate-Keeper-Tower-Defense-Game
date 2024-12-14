using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.PlayerOperations.Skills
{
    using Debug = Utils.Logger.Debug;
    public sealed class BowlingSkill : SkillInstance<BowlingSkill>
    {

        public override Skill skillType { get; } = Skill.Bowling;

        protected override void Perform()
        {
            Debug.Log("Bowling skill is being used");
            player.onBowlingAttackStart?.Invoke();
        }



    }
}
