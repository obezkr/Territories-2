using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int fps = 30, size = 20, penalty = 10, numberOfPlayers = 4, height = 600, width = 600;
    public string nowToMove = "blue", winnerMessage = "";
    public bool createRect = true;
    public string[,] allSquares, allPossibleSquares;
    public string[] colorArray = { "blue", "red", "green", "yellow" };
    public int[] playersScores;
    private int n, m, i, j, max=0;

    public void FinishGame() // ось це мені не подобалось, воно має бути не в GameManager, а окремо, но ладно (чи ні?)
    {
        max = -penalty;
        for (i=0; i<numberOfPlayers; i++)
        {
            if (playersScores[i] > max)
            {
                max = i;
            }
        }
        winnerMessage = (colorArray[max] + " won!");
        //Debug.Log("finish-game-function");
    }

    void Awake()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = fps;
        //создаем массив всех клеток поля - кому какие клекти принадлежат
        n = ((width / size));
        m = ((height / size));
        allSquares = new string[n, m];
        allPossibleSquares = new string[n, m];
        playersScores = new int[numberOfPlayers];
        for (i=0; i<numberOfPlayers; i++)
        {
            playersScores[i] = 0;
            
        }
        for (i=0; i<n; i++)
        {
            for (j=0; j<m; j++)
            {
                allSquares[i, j] = "empty";
                allPossibleSquares[i, j] = "empty";
            }
        }
        allPossibleSquares[0, 0] = "blue";
        allPossibleSquares[0, m - 1] = "red";
        allPossibleSquares[n - 1, m - 1] = "green";
        allPossibleSquares[n-1, 0] = "yellow";


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
