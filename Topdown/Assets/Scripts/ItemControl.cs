using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemControl : MonoBehaviour
{
    public event System.Action SelectFinished; // 플레이 버튼이 눌렸을 때 이벤트
    public event System.Action Reinforced; // 강화 버튼이 눌렸을 때 이벤트

    private GunControl gc;

    [HideInInspector] public int damageMoney = 100;
    [HideInInspector] public int rpmMoney = 100;

    // Start is called before the first frame update
    void Start()
    {
        gc = GetComponent<GunControl>();
    }

    public void StartStage()
    {
        SelectFinished?.Invoke(); // 대리자 호출 단순화
    }

    public void DamageReinforce() // 데미지 증가
    {
        if (gameObject.GetComponent<Player>().money >= damageMoney)
        {
            gc.IncreaseDamage();

            gameObject.GetComponent<Player>().money -= damageMoney;
            damageMoney *= 2;

            Reinforced?.Invoke(); // 대리자 호출 간소화
        }
    }

    public void RPMReinforce() // 연사력 증가
    {
        if (gameObject.GetComponent<Player>().money >= rpmMoney)
        {
            gc.DecreaseShootCooldown();

            gameObject.GetComponent<Player>().money -= rpmMoney;
            rpmMoney *= 2;

            Reinforced?.Invoke(); // 대리자 호출 간소화
        }
    }

    public void PlayerSpeed() // 플레이어 이동속도 증가
    {
        if (gameObject.GetComponent<Player>().money >= 500)
        {
            gameObject.GetComponent<Player>().money -= 500;
            gameObject.GetComponent<Player>().playerSpeed += 1;
        }
    }

    public void PlayerHealth() // 플레이어 체력 증가
    {
        if (gameObject.GetComponent<Player>().money >= 500)
        {
            gameObject.GetComponent<Player>().money -= 500;
            gameObject.GetComponent<Player>().health += 10;
            gameObject.GetComponent<Player>().maxHealth += 10;
        }
    }
}
