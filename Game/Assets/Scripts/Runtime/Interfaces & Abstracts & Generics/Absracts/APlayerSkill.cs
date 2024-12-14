using System;
using System.Collections;
using System.Collections.Generic;
using Game.PlayerOperations.Skills;
using UnityEngine;

public abstract class PlayerSkill
{
    protected virtual void Perform()
    {
        Debug.Log("PlayerSkill Triggered");

    }
    public void UseSkill() => Perform();
    public virtual Skill skillType { get; } = Skill.Passive;
}


