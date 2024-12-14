using System.Collections;
using System.Collections.Generic;
using Game.AI;
using UnityEngine;

namespace Game.AI
{
    public class TurretLevelHandler : MonoBehaviour
    {
        [Tooltip("Current index of the list.")] public int currentLevel = 0;
        public int upgradeCost = 100;
        public List<Turret> turretLevels = new List<Turret>();

        void Awake() => NavMeshManager.instance.Build();

        [ContextMenu("Upgrade Turret")]
        public void UpgradeTurret()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (currentLevel < turretLevels.Count - 1)
            {
                upgradeCost *= 2;
                currentLevel++;
                turretLevels[currentLevel].gameObject.SetActive(true);
                turretLevels[currentLevel - 1].gameObject.SetActive(false);
            }

        }
    }
}