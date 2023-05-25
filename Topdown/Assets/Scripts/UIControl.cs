using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    private GameObject player;
    private GameObject stageController;

    [Header("Panel")]
    public GameObject shopPanel;
    public GameObject startPanel;

    [Header("Weapon Image")]
    public Image weaponImage;

    [Header ("Game State Text")]
    public TMP_Text stageText_1; // 상단 UI
    public TMP_Text stageText_2; // 중앙 UI
    public TMP_Text currentHealthText;
    public TMP_Text moneyText;

    [Header("Gun State Text")]
    public TMP_Text damageText;
    public TMP_Text rpmText;

    [Header("Weapon Sprite")]
    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<ItemControl>().SelectFinished += StartStage;
        player.GetComponent<ItemControl>().Reinforced += UpdateGunStat;
        player.GetComponent<Player>().PlayerHit += OnPlayerHit;

        stageController = GameObject.FindGameObjectWithTag("StageController");
        stageController.GetComponent<StageControl>().StageEnd += EndStage;

        startPanel.GetComponent<CanvasGroup>().alpha = 0.0f;
        startPanel.SetActive(false);

        FirstUpdate();
        TurnOffUI();
        StartCoroutine(InformStage());

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            OnPlayerGetMoney(); // 지금은 매 프레임 수행, 나중에 돈 주웠을때, 썼을때만 호출
        }
        
    }

    void FirstUpdate() // 텍스트 초기화
    {
        OnPlayerHit();
        OnPlayerGetMoney();
        UpdateGunStat();
    }

    void StartStage()
    {
        TurnOffUI();
        stageText_1.text = stageController.GetComponent<StageControl>().currentStage.ToString();

        StartCoroutine(InformStage());

    }

    void EndStage()
    {
        TurnOnUI();
    }

    IEnumerator InformStage()
    {
        stageText_2.text = string.Format($"~ 스테이지 {stageController.GetComponent<StageControl>().currentStage} ~");
        
        startPanel.SetActive(true);

        for (float f = 0.0f; f <= 1.0f; f += 0.05f) // 페이드 인
        {
            startPanel.GetComponent<CanvasGroup>().alpha = f;

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(2f);

        for (float f = 1.0f; f >= 0.0f; f -= 0.05f) // 페이드 아웃
        {
            startPanel.GetComponent<CanvasGroup>().alpha = f;

            yield return new WaitForSeconds(0.025f);
        }

        startPanel.GetComponent<CanvasGroup>().alpha = 0.0f;
        startPanel.SetActive(false);

    }

    void TurnOnUI()
    {
        shopPanel.SetActive(true);
    }

    void TurnOffUI()
    {
        shopPanel.SetActive(false);
    }

    void OnPlayerHit()
    {
        currentHealthText.text = player.GetComponent<Player>().health.ToString();
    }

    void OnPlayerGetMoney()
    {
        moneyText.text = player.GetComponent<Player>().money.ToString();
    }

    void UpdateGunStat()
    {
        damageText.text = player.GetComponent<GunControl>().CurrentGun.bulletDamage.ToString();
        rpmText.text = Mathf.Round((60 / player.GetComponent<GunControl>().CurrentGun.shootCooldown)).ToString();
    }
}
