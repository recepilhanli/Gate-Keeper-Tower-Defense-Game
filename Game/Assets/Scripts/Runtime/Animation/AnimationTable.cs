using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Animations
{
    /// <summary>
    /// A common place to store all the animation hash values
    /// </summary>
    public static class AnimationTable
    {
        public static readonly int IsMoving = Animator.StringToHash("IsMoving");
        public static readonly int Attack1 = Animator.StringToHash("Attack1");
        public static readonly int Attack2 = Animator.StringToHash("Attack2");
        public static readonly int GetHit = Animator.StringToHash("GetHit");
        public static readonly int Death = Animator.StringToHash("Death");

        public static readonly int x = Animator.StringToHash("X");
        public static readonly int y = Animator.StringToHash("Y");
    }
}