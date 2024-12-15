using System.Collections;
using System.Collections.Generic;
using Game.PlayerOperations.Skills;
using UnityEngine;

namespace Game.PlayerOperations
{
    //Main class between Player partial classes
    public partial class Player : AEntity
    {
        public static Player localPlayerInstance;
        public GameObject playerMesh;

        public AudioSource audioSource;
        public AudioClip punch;

        private void Awake()
        {
            localPlayerInstance = this;
            InitMovement();
            InitCombat();
            InitSkills();
            InitCamera();
        }


        private void Update()
        {
            UpdateMovement();
        }

        private void OnDestroy()
        {
            SkillDatabase.ReleaseAllSkils();
        }




    }

}
