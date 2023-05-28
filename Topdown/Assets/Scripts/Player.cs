using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GunControl))]
public class Player : MonoBehaviour, IDamageable
{
    private Rigidbody rb; // 플레이어 리지드바디
    private GunControl gunControl;

    [HideInInspector] public float playerSpeed = 4; // 플레이어 이동 속도
    [HideInInspector] public float health = 50;
    [HideInInspector] public float maxHealth;
    [HideInInspector] public int money = 0;
    [HideInInspector] public int score = 0;

    public Transform canvas; // UI (캔버스)
    private Slider healthBar; // 체력바 (슬라이더)
    private Camera cam; // 메인 카메라

    public event System.Action PlayerDead; // Player 가 죽었을 때 이벤트

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gunControl = GetComponent<GunControl>();
        healthBar = canvas.GetComponentInChildren<Slider>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        maxHealth = health;
        score = 0;
    }

    void FixedUpdate()
    {
        // 플레이어 이동 (45도 기울어진 이동)
        Vector3 playerInput = new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var newPlayerInput = matrix.MultiplyPoint3x4(playerInput);

        rb.MovePosition(rb.position + playerSpeed * Time.fixedDeltaTime * newPlayerInput.normalized);

        // 마우스 방향 바라보기
        Vector3 mousePoint = GetLookAtPoint();
        transform.LookAt(new Vector3(mousePoint.x, transform.position.y, mousePoint.z));

        // 총 장착
        if (Input.GetMouseButton(0))
        {
            gunControl.Shoot();
        }
    }

    void Update()
    {
        // 체력바가 항상 카메라를 향하게
        canvas.transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);

        // 체력바 업데이트
        healthBar.value = health / maxHealth;
    }

    private Vector3 GetLookAtPoint() // transform 이 LookAt 할 좌표를 구하는 함수
    {
        // 카메라에서 마우스 위치로 Ray 를 쏴 바닥과 만나는 곳을 구한다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Ray 와 충돌할 Plane 생성
        Plane plane = new(Vector3.up, Vector3.zero);

        float distance; // out 매개변수
        if (plane.Raycast(ray, out distance))
        {
            // Ray 충돌 지점의 좌표 반환
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    public void TakeHit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        PlayerDead?.Invoke(); // 대리자 호출 간소화

        Time.timeScale = 0.0f;

        GameObject.Destroy(gameObject);
    }
}
