using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    private GameObject player;
    private GameObject stageController;

    [Header("Panel")]
    public GameObject shopPanel;
    public GameObject startPanel;
    public GameObject endPanel;

    [Header("Weapon Image")]
    public Image weaponImage;

    [Header ("Game State Text")]
    public TMP_Text stageText_1; // 상단 UI
    public TMP_Text stageText_2; // 중앙 UI
    public TMP_Text currentHealthText;
    public TMP_Text moneyText;
    public TMP_Text scoreText;

    [Header("Player Stat Text")]
    public TMP_Text maxHealthText;
    public TMP_Text playerSpeedText;

    [Header("Gun State Text")]
    public TMP_Text damageText;
    public TMP_Text rpmText;

    [Header("Reinforce Text")]
    public TMP_Text damageMoneyText;
    public TMP_Text rpmMoneyText;

    [Header("Weapon Sprite")]
    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<ItemControl>().SelectFinished += StartStage;
        player.GetComponent<ItemControl>().Reinforced += UpdateGunStat;
        player.GetComponent<Player>().PlayerDead += GameEnd;

        stageController = GameObject.FindGameObjectWithTag("StageController");
        stageController.GetComponent<StageControl>().StageEnd += EndStage;

        startPanel.SetActive(false);
        endPanel.SetActive(false);

        FirstUpdate();
        TurnOffUI();
        StartCoroutine(InformStage());
    }

    private void Update()
    {
        if (player != null)
        {
            moneyText.text = player.GetComponent<Player>().money.ToString();
            currentHealthText.text = player.GetComponent<Player>().health.ToString();
            maxHealthText.text = player.GetComponent<Player>().maxHealth.ToString();
            playerSpeedText.text = player.GetComponent<Player>().playerSpeed.ToString();
        }
        else
        {
            currentHealthText.text = string.Format("0");
        }
    }

    void FirstUpdate() // 텍스트 초기화
    {
        UpdateGunStat();
        stageText_1.text = string.Format($"스테이지 {stageController.GetComponent<StageControl>().currentStage}");
    }

    void StartStage()
    {
        TurnOffUI();
        stageText_1.text = string.Format($"스테이지 {stageController.GetComponent<StageControl>().currentStage}");

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

    void UpdateGunStat()
    {
        damageText.text = player.GetComponent<GunControl>().CurrentGun.bulletDamage.ToString();
        rpmText.text = Mathf.Round((60 / player.GetComponent<GunControl>().CurrentGun.shootCooldown)).ToString();

        damageMoneyText.text = string.Format($"${player.GetComponent<ItemControl>().damageMoney}");
        rpmMoneyText.text = string.Format($"${player.GetComponent<ItemControl>().rpmMoney}");
    }

    void GameEnd()
    {
        scoreText.text = string.Format($"점수 : {player.GetComponent<Player>().score}");

        if (player.GetComponent<Player>().score > GameManager.Instance.playerData.highScore)
        {
            GameManager.Instance.playerData.highScore = player.GetComponent<Player>().score;
        }
        GameManager.Instance.SaveData();

        endPanel.SetActive(true);
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Play");
    }
}
