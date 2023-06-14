using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTextSinglePlayer : MonoBehaviour
{
    [SerializeField] private Text myself;
    [SerializeField] private GameObject gameManager; // это сам обьект GameManager, который на сцене
    [SerializeField] private GameManagerSinglePlayer gameManagerScript; // а это его скрипт

    // Update is called once per frame
    void Update()
    {
        if (myself.name.Contains("1"))
        {
            myself.text = (gameManagerScript.playersScores[0].ToString());
        }
        if (myself.name.Contains("2"))
        {
            myself.text = (gameManagerScript.playersScores[1].ToString());
        }
        if (myself.name.Contains("3"))
        {
            myself.text = (gameManagerScript.playersScores[2].ToString());
        }
        if (myself.name.Contains("4"))
        {
            myself.text = (gameManagerScript.playersScores[3].ToString());
        }
        if (myself.name.Contains("Winner"))
        {
            myself.text = (gameManagerScript.winnerMessage);
        }

    }
}
