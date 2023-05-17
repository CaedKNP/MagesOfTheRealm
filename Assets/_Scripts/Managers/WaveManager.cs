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

    public class WaveManager : MonoBehaviour
    {
        public Animator animator;
        public Text waveName;


        bool _canSpawn = true;
        bool _bossWave = false;

        Wave currentWave;
        int currentWaveNumber = 1;
        float nextSpawnTime = 0;
        float spawnInterval = 5;
        int allEnemiesToSpawn;
        int spawnCountNow;
        int totalEnemies;

        private void Update()
        {
            TrySpawn();
            totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

            Debug.Log(currentWave.waveName);
            //animator.SetTrigger("WaveComplete");
        }

        void TrySpawn()
        {

            if (_canSpawn && Time.time > nextSpawnTime)
            {
                currentWave = new Wave("Wave: " + currentWaveNumber, currentWaveNumber + 4, currentWaveNumber + 4);
                if (currentWaveNumber % 10 == 0)
                {
                    allEnemiesToSpawn = 1;
                    _bossWave = true;
                }
                else
                    allEnemiesToSpawn = currentWave.noOfEnemies;
                _ = StartCoroutine(SpawnWave());
                waveName.text = currentWave.waveName;//set UI Text to waveName
            }
        }

        IEnumerator SpawnWave()
        {
            _canSpawn = false;

            while (allEnemiesToSpawn > 0)
            {
                if (allEnemiesToSpawn != 1 && !_bossWave)
                {
                    EnemiesToSpawnSetter(allEnemiesToSpawn);

                    for (int i = 0; i < spawnCountNow; i++)
                    {
                        UnitManager.Instance.SpawnEnemy((ExampleEnemyType)Random.Range(0, 3));
                        allEnemiesToSpawn--;
                    }
                }
                else
                {
                    //UnitManager.Instance.SpawnEnemy(ExampleEnemyType.Boss);
                    if (totalEnemies == 0)
                        allEnemiesToSpawn--;
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