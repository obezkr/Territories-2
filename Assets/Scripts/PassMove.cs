using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassMove : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private GameObject createRect;
    [SerializeField] private CreateRect createRectScript;
    public void Pass()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
        createRectScript = createRect.GetComponent<CreateRect>();
        string[] colorArray = gameManagerScript.colorArray;
        string nowToMove = gameManagerScript.nowToMove;
        int numberOfPlayers = gameManagerScript.numberOfPlayers;
        int i, numberOfPlayersInGame = 0;
        // ищем, сколько игроков ещё в игре
        for (i = 0; i < numberOfPlayers; i++)
        {
            if (gameManagerScript.playersScores[i] >= 0)
            {
                numberOfPlayersInGame++;
            }
        }
        if (numberOfPlayersInGame > 1) { // если играющих больше чем 1
            // передаем ход следующему в очереди, у кого очки не отрицательные
            do
            {
                gameManagerScript.nowToMove = colorArray[((Array.IndexOf(colorArray, gameManagerScript.nowToMove) + 1) % numberOfPlayers)];
                createRectScript.AssigncreateRect(true);
            }
            while (gameManagerScript.playersScores[((Array.IndexOf(colorArray, gameManagerScript.nowToMove)) % numberOfPlayers)] < 0);
        }
        else
        {
            gameManagerScript.FinishGame();
        }
    }
    public void DestroyRect()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
        int penalty = gameManagerScript.penalty;
        string[] colorArray = gameManagerScript.colorArray;
        string nowToMove = gameManagerScript.nowToMove;
        // отнимаем очки
        gameManagerScript.playersScores[Array.IndexOf(colorArray, nowToMove)] =
            gameManagerScript.playersScores[Array.IndexOf(colorArray, nowToMove)] - penalty;
        // уничтожаем прямоугольник и его обводку
        GameObject objectToDestroy = GameObject.Find(CreateRect.rectCounter + "Rect");
        Destroy(objectToDestroy);
        GameObject lRend1 = GameObject.Find("1Line" + CreateRect.rectCounter + "Rect");
        GameObject lRend2 = GameObject.Find("2Line" + CreateRect.rectCounter + "Rect");
        GameObject lRend3 = GameObject.Find("3Line" + CreateRect.rectCounter + "Rect");
        GameObject lRend4 = GameObject.Find("4Line" + CreateRect.rectCounter + "Rect");
        Destroy(lRend1);
        Destroy(lRend2);
        Destroy(lRend3);
        Destroy(lRend4);

    }
}
