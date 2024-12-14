using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PlayerOperations
{
    //Main class between Player partial classes
    public partial class Player : AEntity
    {
        public static Player localPlayerInstance;
        public GameObject playerMesh;

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
            if (Input.GetKeyDown(KeyCode.F))
            {
               EffectManager.instance.CreateMagicExplosion(transform.position,5,10,false);
            }
        }




    }

}
