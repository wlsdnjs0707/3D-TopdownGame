using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StageControl : MonoBehaviour
{
    [System.Serializable]
    public class Stage
    {
        public int enemyCount;
        public float spawnCooldown;
    }

    public enum State { Play, Stop }; // 현재 스테이지 상태
    State currentState;

    [Header("Spawn Point")]
    public Transform[] spawnPoints; // 스폰 위치

    private Stage[] stages;
    public Enemy enemy;

    public int maxStage;

    [HideInInspector] public int currentStage = 0;
    private int enemyCountLeftToSpawn = 0;
    private int currentEnemyCount = -1;
    private float nextSpawnTime = 0;

    private float enemySpeed = 1.0f;

    private GameObject player; // 플레이어

    public event System.Action StageEnd; // UI Controller 에게 스테이지가 끝났음을 알림

    // Start is called before the first frame update
    void Start()
    {
        stages = new Stage[maxStage];

        for (int i = 0; i < maxStage; i++)
        {
            int index = i;
            stages[index] = new Stage();
            stages[index].enemyCount = 1 + Mathf.FloorToInt(index/3);
            stages[index].spawnCooldown = 2;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<ItemControl>().SelectFinished += StartStage;

        currentState = State.Stop;
        StartStage();
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
                for (int i=0; i<spawnPoints.Length; i++)
                {
                    int index = i;
                    Enemy currentEnemy = Instantiate(enemy, spawnPoints[index].position, Quaternion.identity) as Enemy;
                    currentEnemy.EnemyDead += OnEnemyDead; // Enemy Dead 이벤트 등록
                    currentEnemy.GetComponent<NavMeshAgent>().speed = enemySpeed + (currentStage * 0.5f); // 스테이지마다 Enemy 속도 빨라짐
                }
            }
        }
        
    }

    void StartStage()
    {
        SetNextStage();
        Time.timeScale = 1.0f;

        Invoke(nameof(SetPlay), 2f); // 플레이 시작 후 2초 뒤부터 적 생성 시작
    }

    void SetNextStage()
    {
        currentStage++;

        if (currentStage - 1 < stages.Length)
        {
            enemyCountLeftToSpawn = stages[currentStage - 1].enemyCount;
            currentEnemyCount = enemyCountLeftToSpawn * 3;
        }
    }

    void SetPlay()
    {
        currentState = State.Play;
    }

    void StopStage()
    {
        currentState = State.Stop;
        Time.timeScale = 0;

        StageEnd?.Invoke(); // 대리자 호출 간소화
    }

    void OnEnemyDead() // Enemy 하나가 죽었을 때 
    {
        currentEnemyCount--;

        if (currentEnemyCount == 0)
        {
            if (currentStage == maxStage) // 모든 스테이지 클리어
            {
                Time.timeScale = 0.0f;
                GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIControl>().GameEnd();
            }
            else
            {
                Invoke(nameof(StopStage), 5.0f);
            }
        }
    }

}
