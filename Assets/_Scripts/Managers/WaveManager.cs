using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets._Scripts.Managers
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public int noOfEnemies;
        public float nextSpawnTime;

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

    public class WaveManager : StaticInstance<WaveManager>
    {
        public Animator animator;
        public Text waveName;
        public Text enemyCounter;


        bool _canSpawn = true;
        bool _bossWave = false;
        public bool gameOver = false;

        int scaleMultiplier = 1;
        Wave currentWave;
        int currentWaveNumber = 1;
        float nextSpawnTime = 0;
        readonly float spawnInterval = 0.1f;
        int allEnemiesToSpawn;
        int spawnCountNow;
        int totalEnemies;

        private void Update()
        {
            if (!gameOver)
            {
                TrySpawn();
                //totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
                totalEnemies = GameManager.enemies.Count;

                if (currentWaveNumber % 10 == 0)
                    enemyCounter.text = "Enemies Left: \n" + totalEnemies;
                else
                    enemyCounter.text = "";

                Debug.Log(currentWave.waveName);
                //animator.SetTrigger("WaveComplete");
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
                    //UnitManager.Instance.SpawnEnemy(ExampleEnemyType.Boss);
                    if (totalEnemies == 0)
                    {
                        allEnemiesToSpawn--;
                        _bossWave = false;
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