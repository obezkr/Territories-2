using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour
{
    [SerializeField] private Text myself;
    [SerializeField] private GameObject gameManager; // это сам обьект GameManager, который на сцене
    [SerializeField] private GameManager gameManagerScript; // а это его скрипт

    // Update is called once per frame
    void Update()
    {
        if (myself.name.Contains("Blue"))
        {
            myself.text = (gameManagerScript.playersScores[0].ToString());
        }
        if (myself.name.Contains("Red"))
        {
            myself.text = (gameManagerScript.playersScores[1].ToString());
        }
        if (myself.name.Contains("Green"))
        {
            myself.text = (gameManagerScript.playersScores[2].ToString());
        }
        if (myself.name.Contains("Yellow"))
        {
            myself.text = (gameManagerScript.playersScores[3].ToString());
        }
        if (myself.name.Contains("Winner"))
        {
            myself.text = (gameManagerScript.winnerMessage);
        }

    }
}
