using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GunControl))]
public class Player : MonoBehaviour
{

    public float playerSpeed = 5; // 플레이어 이동 속도
    private Rigidbody rb; // 플레이어 리지드바디
    private GunControl gunControl;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gunControl = GetComponent<GunControl>();
    }

    void FixedUpdate()
    {
        // 플레이어 이동
        Vector3 playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        rb.MovePosition(rb.position + playerInput.normalized * playerSpeed * Time.fixedDeltaTime);

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
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        float distance; // out 매개변수
        if (plane.Raycast(ray, out distance))
        {
            // Ray 충돌 지점의 좌표 반환
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }
}
