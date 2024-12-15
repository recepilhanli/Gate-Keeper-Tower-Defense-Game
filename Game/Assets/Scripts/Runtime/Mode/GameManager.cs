using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;

namespace Game.Modes
{
    using Game.PlayerOperations;
    using UnityEngine.InputSystem;
    using UnityEngine.SceneManagement;
    using Debug = Utils.Logger.Debug;
    public class GameManager : Singleton<GameManager>
    {
        public AGameMode gameMode = null;
        public List<Transform> enemySpawnPoints = new List<Transform>();
        private bool isGameStarted = false;
        [SerializeField] private int _currency = 0;
        public int score = 0;
        public TextMeshProUGUI timerTMP;
        public GameObject deathPanel;
        public GameObject pausePanel;
        [SerializeField] private TextMeshProUGUI _currencyTMP;
        [SerializeField] private TextMeshProUGUI _TitleTMP;
        [SerializeField] private TextMeshProUGUI _tutorialTMP;

        public int currency
        {
            get => _currency;
            set
            {
                if(gameMode == null)
                {
                    Debug.LogError("GameMode is not set!!");
                    return;
                }

                _currency = value;
                _currencyTMP.text = _currency.ToString();
            }
        }


        #region  Temporary UI Operations
        public void ShowTitle(string title, Color color)
        {
            _TitleTMP.text = title;
            _TitleTMP.color = color;
            Color colorZeroAlpha = color;
            colorZeroAlpha.a = 0;

            Sequence.Create()
            .Group(Tween.Color(_TitleTMP, colorZeroAlpha, 2f))
            .Group(Tween.ShakeScale(_TitleTMP.transform, new Vector3(.1f, .1f, .1f), 1.25f, 10, easeBetweenShakes: Ease.OutQuart));
        }



        public void ReturnMainMenu() => SceneManager.LoadScene(0);

        public void RestartGame()
        {
            Destroy(Player.localPlayerInstance.gameObject);
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            Time.timeScale = 1;
        }


        public void Pause()
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }

        public void Resume()
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }
        #endregion

        #region GameMode
        private void Start()
        {
            var colorAlpha = _tutorialTMP.color;
            colorAlpha.a = 0;
            Tween.Color(_tutorialTMP, colorAlpha, 2f,Ease.Linear, startDelay: 5f);

            if(gameMode == null)
            {
                Debug.LogError("GameMode is not set!!");
                return;
            }

            _currencyTMP.text = _currency.ToString();
            ShowTitle("Defend the Gate!", Color.yellow);
            Invoke(nameof(StartGame), 5f);
            timerTMP.text = "Ready?";
        }

        [ContextMenu("Start Game")]
        public void StartGame()
        {
            ShowTitle("Ready?", Color.green);

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
            if (Keyboard.current.escapeKey.wasPressedThisFrame) Pause();
        }




        public Vector3 GetRandomSpawnPoint()
        {
            return enemySpawnPoints[Random.Range(0, enemySpawnPoints.Count)].position;
        }

        public void Fail()
        {
            isGameStarted = false;
            gameMode.Fail();
        }
        public void Success() => gameMode.Success();

        public void GuideEnemy(AEnemy enemy) => gameMode.GuideEnemy(enemy);
        #endregion

    }
}