using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl : MonoBehaviour
{
    [System.Serializable]
    public class Stage
    {
        public int totalEnemyCount;
        public float spawnCooldown;
    }

    public float rotSpeed;

    public enum State { Play, Stop }; // 현재 스테이지 상태
    State currentState;

    public Transform spawnPoint; // 스폰 위치

    public Stage[] stages;
    public Enemy enemy;

    private int currentStage = 0;
    private int enemyCountLeftToSpawn = 0;
    private int currentEnemyCount = 0;

    private float nextSpawnTime = 0;

    private GameObject player; // 플레이어
    public GameObject directionalLight; // 라이트

    public event System.Action StageEnd; // UI Controller 에게 스테이지가 끝났음을 알림

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<ItemControl>().SelectFinished += StartStage;

        StopStage();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.Play)
        {
            if (enemyCountLeftToSpawn > 0 && Time.time > nextSpawnTime) // 적 생성
            {
                enemyCountLeftToSpawn--;

                nextSpawnTime = Time.time + stages[currentStage - 1].spawnCooldown;

                // Enemy 생성
                Enemy currentEnemy = Instantiate(enemy, spawnPoint.position, Quaternion.identity) as Enemy;
                currentEnemy.EnemyDead += OnEnemyDead; // Enemy Dead 이벤트 등록
            }
            else if (currentEnemyCount == 0) // 모든 적을 죽였을 때
            {
                if (currentStage == stages.Length)
                {
                    // 모든 스테이지 클리어

                    UnityEditor.EditorApplication.isPlaying = false; // 에디터에서
                    //Application.Quit(); // 어플리케이션에서
                }
                else
                {
                    StopStage();
                }
            }
        }
        
    }

    void SetNextStage()
    {
        currentStage++;

        if (currentStage - 1 < stages.Length )
        {
            enemyCountLeftToSpawn = stages[currentStage - 1].totalEnemyCount;
            currentEnemyCount = enemyCountLeftToSpawn;
        }

    }

    void StartStage()
    {
        SetNextStage();
        currentState = State.Play;
        Time.timeScale = 1.0f;
    }

    void StopStage()
    {
        currentState = State.Stop;
        Time.timeScale = 0;

        if (StageEnd != null)
        {
            StageEnd();
        }
    }

    void OnEnemyDead() // Enemy 하나가 죽었을 때 
    {
        currentEnemyCount--;
    }

}
