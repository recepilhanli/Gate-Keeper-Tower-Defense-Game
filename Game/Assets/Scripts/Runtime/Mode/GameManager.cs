using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;

namespace Game.Modes
{
    public class GameManager : Singleton<GameManager>
    {
        public AGameMode gameMode = null;
        public List<Transform> enemySpawnPoints = new List<Transform>();
        private bool isGameStarted = false;
        [SerializeField] private int _currency = 0;
        public int score = 0;
        public TextMeshProUGUI timerTMP;
        [SerializeField] private TextMeshProUGUI _currencyTMP;
        [SerializeField] private TextMeshProUGUI _TitleTMP;

        public int currency
        {
            get => _currency;
            set
            {
                _currency = value;
                _currencyTMP.text = _currency.ToString();
            }
        }


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

        private void Start()
        {
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
    }
}