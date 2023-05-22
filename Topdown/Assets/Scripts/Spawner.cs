using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public Enemy enemy;

    Wave currentWave;
    int currentWaveNumber = 0;

    int enemyLeft;
    float nextSpawnTime;

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float spawnCooldown;
    }

    private void Start()
    {
        NextWave();
    }

    private void Update()
    {
        if (enemyLeft > 0 && Time.time > nextSpawnTime)
        {
            enemyLeft--;

            nextSpawnTime = Time.time + currentWave.spawnCooldown;

            Enemy currentEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity) as Enemy;
            currentEnemy.enemyDead += OnEnemyDead; // Enemy Dead 이벤트 등록
        }
    }

    void NextWave()
    {
        currentWaveNumber++;

        if (currentWaveNumber-1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];
            enemyLeft = currentWave.enemyCount;
        }
    }

    void OnEnemyDead()
    {
        // 적 하나가 죽었을때 호출되는 이벤트 구현
    }
}
