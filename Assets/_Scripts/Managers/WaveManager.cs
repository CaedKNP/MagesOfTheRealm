using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets._Scripts.Managers
{
    /// <summary>
    /// Class to store information about waves (current game situation)
    /// </summary>
    [System.Serializable]
    public class Wave
    {
        /// <summary>
        /// name of the wave
        /// </summary>
        public string waveName;

        /// <summary>
        /// number of enemies that will spawn at this wave
        /// </summary>
        public int noOfEnemies;

        /// <summary>
        /// the time of pause after wave
        /// </summary>
        public float nextSpawnTime;

        /// <summary>
        /// constructor to initiate waves
        /// </summary>
        /// <param name="waveName">name of the wave</param>
        /// <param name="noOfEnemies">number of enemies that will spawn at this wave</param>
        /// <param name="nextSpawnTime">the time of pause after wave</param>
        public Wave(string waveName, int noOfEnemies, float nextSpawnTime)
        {
            this.waveName = waveName;
            this.noOfEnemies = noOfEnemies;

            if (nextSpawnTime < 30)
                this.nextSpawnTime = nextSpawnTime;
            else
                this.nextSpawnTime = 30;
        }
    }

    /// <summary>
    /// Class that handle waves
    /// </summary>
    public class WaveManager : StaticInstance<WaveManager>
    {
        /// <summary>
        /// name of the current wave
        /// </summary>
        public Text waveName;

        /// <summary>
        /// text that contains number of enemies alive
        /// </summary>
        public Text enemyCounter;

        bool _canSpawn = true;
        bool _bossWave = false;
        bool _bossSpawned = false;
        public bool gameOver = false;

        int scaleMultiplier = 1;
        Wave currentWave;
        int currentWaveNumber = 1;
        float nextSpawnTime = 0;
        readonly float spawnInterval = 0.1f;
        int allEnemiesToSpawn;
        int spawnCountNow;
        int totalEnemies;

        private void Start()
        {
            waveName = GameManager.Instance.waveName;
            enemyCounter = GameManager.Instance.enemyCounter;
        }

        private void Update()
        {
            if (!gameOver && GameManager.map != null)
            {
                TrySpawn();
                totalEnemies = GameManager.enemies.Count;

                if (currentWaveNumber % 10 == 0)
                    enemyCounter.text = "Enemies Left: \n" + totalEnemies;
                else
                    enemyCounter.text = "";

                Debug.Log(currentWave.waveName);
            }
        }

        void TrySpawn()
        {
            if (_canSpawn && Time.time > nextSpawnTime)
            {
                currentWave = new Wave("Wave: " + currentWaveNumber, currentWaveNumber + 4, currentWaveNumber + 4);

                if (currentWaveNumber % 10 == 0)
                {
                    enemyCounter.text = "Enemies Left: \n" + totalEnemies;
                    allEnemiesToSpawn = 1;
                    _bossWave = true;
                }
                else
                    allEnemiesToSpawn = currentWave.noOfEnemies;

                _ = StartCoroutine(SpawnWave());
                scaleMultiplier++;
                waveName.text = currentWave.waveName;//set UI Text to waveName
            }
        }

        IEnumerator SpawnWave()
        {
            _canSpawn = false;

            while (allEnemiesToSpawn > 0)
            {
                if (!_bossWave)
                {
                    EnemiesToSpawnSetter(allEnemiesToSpawn);

                    for (int i = 0; i < spawnCountNow; i++)
                    {
                        UnitManager.Instance.SpawnEnemy((ExampleEnemyType)Random.Range(0, 3), scaleMultiplier);
                        allEnemiesToSpawn--;
                    }
                }
                else
                {
                    if (!_bossSpawned)
                    {
                        UnitManager.Instance.SpawnEnemy(ExampleEnemyType.Boss, scaleMultiplier);
                        _bossSpawned = true;
                    }

                    if (totalEnemies == 0)
                    {
                        allEnemiesToSpawn--;
                        _bossWave = false;
                        _bossSpawned = false;
                    }
                }

                yield return new WaitForSeconds(spawnInterval);
            }

            nextSpawnTime = Time.time + currentWave.nextSpawnTime;
            currentWaveNumber++;
            _canSpawn = true;
        }

        void EnemiesToSpawnSetter(int enemyCount)
        {
            if (enemyCount < 5)
                spawnCountNow = enemyCount;
            else
                spawnCountNow = 5;
        }
    }
}