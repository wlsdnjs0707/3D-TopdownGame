using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject buttonPanel;
    public GameObject namePanel;
    public GameObject rankPanel;

    [Header("Text")]
    public TMP_Text nameText;
    public TMP_Text highScoreText;

    void Start()
    {
        rankPanel.SetActive(false);

        if (!File.Exists(Application.persistentDataPath + "/PlayerData"))
        {
            buttonPanel.SetActive(false);
        }
        else
        {
            namePanel.SetActive(false);
        }
    }

    public void PressPlay()
    {
        SceneManager.LoadScene("Play");
    }

    public void PressRank()
    {
        highScoreText.text = string.Format($"{GameManager.Instance.playerData.name} : {GameManager.Instance.playerData.highScore}");

        buttonPanel.SetActive(false);
        rankPanel.SetActive(true);
    }

    public void PressSetName()
    {
        GameManager.Instance.playerData.name = nameText.text;

        GameManager.Instance.SaveData();

        namePanel.SetActive(false);
        buttonPanel.SetActive(true);
    }

    public void PressMain()
    {
        rankPanel.SetActive(false);
        buttonPanel.SetActive(true);
    }
}
