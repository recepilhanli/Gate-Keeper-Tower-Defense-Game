using System.Collections;
using System.Collections.Generic;
using Game.PlayerOperations;
using UnityEngine;

/// <summary>
/// SkillInstance class is a generic class that registers to the skill database.
/// </summary>
public class SkillInstance<T> : PlayerSkill where T : PlayerSkill, new()
{
    public static Player player { get => Player.localPlayerInstance; }
    public static T Register()
    {
        return new T();
    }
}
