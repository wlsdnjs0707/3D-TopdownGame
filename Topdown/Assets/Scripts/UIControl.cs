using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    private GameObject player;
    private GameObject stageController;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<ItemControl>().SelectFinished += TurnOffUI;

        stageController = GameObject.FindGameObjectWithTag("StageController");
        stageController.GetComponent<StageControl>().StageEnd += TurnOnUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TurnOnUI()
    {
        gameObject.SetActive(true);
    }

    void TurnOffUI()
    {
        gameObject.SetActive(false);
    }
}
