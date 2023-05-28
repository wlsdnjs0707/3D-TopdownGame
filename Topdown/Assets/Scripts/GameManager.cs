using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData
{
    public string name = "AAA";
    public int highScore = 0;
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance; // 게임 매니저 객체를 전역 선언

    public PlayerData playerData = new PlayerData();

    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }

            return _instance; 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject); // 인스턴스가 이미 존재하면 새로 생긴 인스턴스 제거
        }

        if (File.Exists(Application.persistentDataPath + "/PlayerData"))
        {
            LoadData();
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(playerData);
        File.WriteAllText(Application.persistentDataPath + "/PlayerData", data);
    }

    void LoadData()
    {
        string data = File.ReadAllText(Application.persistentDataPath + "/PlayerData");
        playerData = JsonUtility.FromJson<PlayerData>(data);
    }
}
