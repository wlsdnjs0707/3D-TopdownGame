using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamageable
{
    private NavMeshAgent navMeshAgent; // NavMeshAgent 컴포넌트
    private Transform target; // 추적할 플레이어
    
    public enum State {Idle, Track, Attack}; // 캐릭터의 상태를 enum형으로 선언
    State currentState;

    public float health = 50;
    private float maxHealth;

    public float attackRange = 0.5f;
    public float attackCooldown = 1;
    public float attackSpeed = 1;
    private float nextAttackTime;

    private bool isDead = false;

    private float collistionDistance;

    public event System.Action enemyDead; // Spawner에 Enemy가 죽었음을 알리기 위한 이벤트

    void Start()
    {
        maxHealth = health;
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        collistionDistance = GetComponent<CapsuleCollider>().radius + target.GetComponent<CapsuleCollider>().radius;

        currentState = State.Track;

        // 플레이어 추적 - 매 프레임마다 갱신하지 않고 일정 주기마다 갱신.
        StartCoroutine(UpdateTarget());
    }

    void Update()
    {
        // 공격 쿨다운이 끝났는지, 공격 사거리 안에 들어왔는지 체크 (제곱근 연산이 필요한 Distance 함수 대신 제곱한 값을 사용)
        if (Time.time > nextAttackTime)
        {
            if ((target.position - transform.position).sqrMagnitude < Mathf.Pow(attackRange + collistionDistance, 2))
            {
                nextAttackTime = Time.time + attackCooldown;
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        currentState = State.Attack;
        navMeshAgent.enabled = false; // 공격 시 경로 업데이트 해제

        Vector3 originalPosition = transform.position;
        Vector3 targetDirection = (target.position - transform.position).normalized;
        Vector3 targetPosition = target.position - targetDirection * target.GetComponent<CapsuleCollider>().radius;

        float percent = 0;

        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;

            transform.position = Vector3.Lerp(originalPosition, targetPosition, interpolation);

            yield return null;
        }

        currentState = State.Track;
        navMeshAgent.enabled = true;

    }

    IEnumerator UpdateTarget()
    {
        float updateCooldown = 0.25f;

        while (target != null)
        {
            if (currentState == State.Track)
            {
                Vector3 targetDirection = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - targetDirection * (collistionDistance + attackRange / 2);

                if (!isDead)
                {
                    navMeshAgent.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(updateCooldown);
        }
    }
    public void TakeHit(float damage)
    {
        health -= damage;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (enemyDead != null)
        {
            enemyDead();
        }

        GameObject.Destroy(gameObject);
    }
}
