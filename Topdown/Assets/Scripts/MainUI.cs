using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject statePanel;
    public GameObject buttonPanel;
    public GameObject namePanel;

    [Header("Text")]
    public TMP_Text nameText;

    void Start()
    {
        if (!File.Exists(Application.persistentDataPath + "/PlayerData"))
        {
            statePanel.SetActive(false);
            buttonPanel.SetActive(false);
        }
        else
        {
            namePanel.SetActive(false);
        }
    }

    void Update()
    {

    }

    public void PressPlay()
    {
        SceneManager.LoadScene("Play");
    }

    public void PressSetName()
    {
        GameManager.Instance.playerData.name = nameText.text;

        GameManager.Instance.SaveData();

        namePanel.SetActive(false);
        statePanel.SetActive(true);
        buttonPanel.SetActive(true);
    }
}
