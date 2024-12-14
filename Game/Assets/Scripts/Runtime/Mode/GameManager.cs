using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Modes
{
    public class GameManager : Singleton<GameManager>
    {
        public AGameMode gameMode = null;
        private bool isGameStarted = false;

        public int score = 0;

        [ContextMenu("Start Game")]
        public void StartGame()
        {
            if (gameMode == null)
            {
                Debug.LogError("GameMode is not set!!");
                return;
            }
            gameMode.InitGameMode();
            isGameStarted = true;
        }

        private void Update()
        {
            if (!isGameStarted) return;
            gameMode.GameModeUpdate();
        }

        public void Fail()
        {
            isGameStarted = false;
            gameMode.Fail();
        }
        public void Success() => gameMode.Success();

        public void GuideEnemy(AEnemy enemy) => gameMode.GuideEnemy(enemy);
    }
}