using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamageable
{
    [HideInInspector] public NavMeshAgent navMeshAgent; // NavMeshAgent 컴포넌트
    public Transform canvas; // UI (캔버스)
    private Slider healthBar; // 체력바 (슬라이더)
    private GameObject target; // 추적할 플레이어 오브젝트
    private Camera cam; // 메인 카메라

    private Animator animator;

    public GameObject money;
    public GameObject healpack;

    public enum State {Idle, Track, Attack}; // 캐릭터의 상태를 enum 타입으로 선언
    State currentState;

    public float health = 50;
    private float maxHealth;

    private float attackRange = 0.75f;
    private float attackCooldown = 2.0f;
    private float attackSpeed = 2.0f;
    private float nextAttackTime;

    public float damage = 10;

    private bool isDead = false;
    private bool isAttack = false;
    private bool isPlayerDead = false;

    private float collistionDistance;

    public event System.Action EnemyDead; // Enemy 가 죽었음을 알리기 위한 이벤트

    void Start()
    {
        animator = transform.Find("Zombie").gameObject.GetComponent<Animator>();

        maxHealth = health;
        navMeshAgent = GetComponent<NavMeshAgent>();

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        healthBar = canvas.GetComponentInChildren<Slider>();

        target = GameObject.FindGameObjectWithTag("Player");
        target.GetComponent<Player>().PlayerDead += OnPlayerDead;
        collistionDistance = GetComponent<CapsuleCollider>().radius + target.GetComponent<CapsuleCollider>().radius;

        currentState = State.Track;

        // 플레이어 추적 - 매 프레임마다 갱신하지 않고 일정 주기마다 갱신.
        StartCoroutine(UpdateTarget());
    }

    void Update()
    {
        if (currentState == State.Track)
        {
            animator.SetBool("IsWalking", true);
        }
        else if(currentState == State.Attack)
        {
            animator.SetBool("IsWalking", false);
            animator.SetTrigger("Attack");
        }

        // 체력바가 항상 카메라를 향하게
        canvas.transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);


        if (!isDead)
        {
            // 공격 쿨다운이 끝났는지, 공격 사거리 안에 들어왔는지 체크 (제곱근 연산이 필요한 Distance 함수 대신 제곱한 값을 사용)
            if (!isPlayerDead && Time.time > nextAttackTime)
            {
                if ((target.transform.position - transform.position).sqrMagnitude < Mathf.Pow(attackRange + collistionDistance, 2))
                {
                    nextAttackTime = Time.time + attackCooldown;
                    StartCoroutine(Attack());
                }
            }
        }
    }

    IEnumerator Attack()
    {
        currentState = State.Attack;
        navMeshAgent.enabled = false; // 공격 시 경로 업데이트 해제

        Vector3 originalPosition = transform.position;
        Vector3 targetDirection = (target.transform.position - transform.position).normalized;
        Vector3 targetPosition = target.transform.position - targetDirection * target.GetComponent<CapsuleCollider>().radius;

        float percent = 0;
        isAttack = false;

        while (!isPlayerDead && percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;

            if (percent >= 0.5f && !isAttack)
            {
                isAttack = true;
                target.GetComponent<Player>().TakeHit(damage);
            }

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;

            transform.position = Vector3.Lerp(originalPosition, new Vector3(targetPosition.x, originalPosition.y, targetPosition.z), interpolation);

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
                Vector3 targetDirection = (target.transform.position - transform.position).normalized;
                Vector3 targetPosition = target.transform.position - targetDirection;

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

        healthBar.value = health / maxHealth;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        navMeshAgent.ResetPath();

        Instantiate(money, new Vector3(transform.position.x, 3, transform.position.z), Quaternion.identity); // Money 생성

        target.GetComponent<Player>().score += 100;

        if (GetRandom(10))
        {
            Instantiate(healpack, new Vector3(transform.position.x, 3, transform.position.z), Quaternion.identity);
        }

        animator.SetTrigger("Dead");

        GameObject.Destroy(gameObject, 1f);

        EnemyDead?.Invoke(); // 대리자 호출 간소화
    }

    bool GetRandom(int percent)
    {
        if (Random.Range(0, 100)+1 <= percent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnPlayerDead()
    {
        // 플레이어가 죽었을 때
        isPlayerDead = true;
    }
}
