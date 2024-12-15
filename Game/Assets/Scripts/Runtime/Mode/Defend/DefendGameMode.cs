using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.PlayerOperations;
using UnityEngine;

namespace Game.Modes
{
    [CreateAssetMenu(fileName = "DefendGameMode", menuName = "Game/Modes/Defend Game Mode")]
    public class DefendGameMode : AGameMode
    {
        [NonSerialized] public int wave = 0;


        [SerializeField] private List<EnemySpawn> _enemySpawns = new List<EnemySpawn>();
        [SerializeField, Tooltip("if some enemies failed to spawn, this will be spawned instead")] private AEnemy _defaultEnemy;
        [SerializeField, Tooltip("Time between waves (seconds)")] private float _pauseTime = 10;
        [SerializeField, Tooltip("Wave's Duration (seconds)")] private float _waveDuration = 60;
        [SerializeField, Tooltip("min time between enemies (seconds)")] private float _minTimeBetweenEnemies = .25f;
        [SerializeField, Tooltip("max time between enemies (seconds)")] private float _maxTimeBetweenEnemies = 1f;
        [SerializeField] private int enemyCount = 10;
        [SerializeField, Tooltip("Changes number of enemies for per wave. (Wave * enemySpawnMultiplier)")] private float _enemyMultiplier = 1.1f;

        private int _enemyCount = 0;
        private bool _pause = true;
        private List<EnemySpawn> _enemySpawnsForCurrentWave = new List<EnemySpawn>();


        private float _currentWaveDuration = 0;

        #region Abstract Methods
        public override void GameModeUpdate()
        {
            if (_pause) return;

            _currentWaveDuration -= Time.deltaTime;
            if (_currentWaveDuration <= 0 && _enemyCount == 0)
            {
                Success();
            }
            if (_currentWaveDuration > 1) GameManager.instance.timerTMP.text = _currentWaveDuration.ToString("F1");
            else if (_enemyCount != 0) GameManager.instance.timerTMP.text = $"<color=yellow>Eliminate The Enemies: {_enemyCount}</color>";
        }

        public override void InitGameMode()
        {
            GameManager.instance.score = 1;
            _pause = true;
            wave = 0;
            Wave().Forget();
            GameManager.instance.timerTMP.text = "Waiting...";
        }

        public override void Fail()
        {
            _pause = true;
            wave = 0;
            GameManager.instance.timerTMP.text = "<color=red>Failed!</color>";
            EffectManager.instance.DeathEffect();
        }

        public override void Success()
        {
            GameManager.instance.ShowTitle("Wave Completed!", Color.green);
            GameManager.instance.currency += 40 * wave;
            GameManager.instance.score++;
            _pause = true;
            GameManager.instance.timerTMP.text = "Waiting..";
            wave++;
            Wave().Forget();
        }
        #endregion

        private async UniTaskVoid Wave()
        {
            _currentWaveDuration = _waveDuration + (wave * 1.15f * _waveDuration);
            _enemyCount = 0;
            await UniTask.WaitForSeconds(_pauseTime);
            GameManager.instance.ShowTitle($"Wave {wave + 1} !", Color.red);

            _enemySpawnsForCurrentWave.Clear();

            foreach (var spawn in _enemySpawns)
            {
                if (spawn.minWave <= wave)
                {
                    _enemySpawnsForCurrentWave.Add(spawn);
                }
            }

            _pause = false;

            int enemyCount = (int)(this.enemyCount * Mathf.Pow(_enemyMultiplier, wave));
            for (int i = 0; i < this.enemyCount; i++)
            {
                await SpawnRandomEnemy();
            }
        }

        private async UniTask SpawnRandomEnemy()
        {
            await UniTask.WaitForSeconds(UnityEngine.Random.Range(_minTimeBetweenEnemies, _maxTimeBetweenEnemies));
            var prefab = _defaultEnemy;

            if (_enemySpawnsForCurrentWave.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, _enemySpawnsForCurrentWave.Count);
                if (UnityEngine.Random.value <= _enemySpawnsForCurrentWave[index].spawnRatio)
                {
                    prefab = _enemySpawnsForCurrentWave[index].enemyPrefab;
                }
            }

            var enemy = Instantiate(prefab, GameManager.instance.GetRandomSpawnPoint(), Quaternion.identity);
            enemy.onDeath += DecreaseEnemyCount;
            enemy.target = DefendingObject.instance;
            _enemyCount++;
        }

        private void DecreaseEnemyCount(DeathReason reason) => _enemyCount--;

        public override void GuideEnemy(AEnemy enemy)
        {
            if (_pause)
            {
                enemy.target = Player.localPlayerInstance;
                return;
            }
            enemy.target = DefendingObject.instance;
        }
    }

    [Serializable]
    struct EnemySpawn
    {
#if UNITY_EDITOR
        public string name; //only for editor
#endif
        public AEnemy enemyPrefab;
        public int minWave;
        [Range(0, 1)] public float spawnRatio;
    }
}