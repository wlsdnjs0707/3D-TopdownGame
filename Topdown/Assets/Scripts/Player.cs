using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GunControl))]
[RequireComponent(typeof(ItemControl))]
public class Player : MonoBehaviour, IDamageable
{

    public float playerSpeed = 5; // 플레이어 이동 속도
    private Rigidbody rb; // 플레이어 리지드바디
    private GunControl gunControl;

    public float health = 50;
    private float maxHealth;

    public event System.Action PlayerDead; // Enemy에게 Player가 죽었음을 알리기 위한 이벤트


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gunControl = GetComponent<GunControl>();
        maxHealth = health;
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

    private Vector3 GetLookAtPoint() // transform이 LookAt할 좌표를 구하는 함수
    {
        // 카메라에서 마우스 위치로 Ray를 쏴 바닥과 만나는 곳을 구한다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Ray와 충돌할 Plane 생성
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
        if (PlayerDead != null)
        {
            PlayerDead();
        }

        GameObject.Destroy(gameObject);
    }
}
