using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;
using System;
using Mirror;

public class GameManagerSinglePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public int fps = 30, numberOfPlayers = 4, size, penalty, height, width;
    public string nowToMove = "1", winnerMessage = "", lastMusicPlayed;
    public bool createRect = true;
    public string[,] allSquares, allPossibleSquares;
    public string[] colorArray = {"1", "2", "3", "4"};

    public AudioSource music015Territories;
    public AudioSource music011MetalTheme;
    [SerializeField] public Color[] playersColorsArray={};
    public int[] playersScores;
    private int n, m, i, j, max = 0;

    static string path = "settings.ini";
    static char[] separators = new char[] { ' ', '=', ';' };

    public int Size 
    {
        get { return size; } 
        set { size = value; }
    }

    public void FinishGame() // ось це мені не подобалось, воно має бути не в GameManager, а окремо, но ладно (чи ні?)
    {
        max = -penalty;
        for (i = 0; i < numberOfPlayers; i++)
        {
            if (playersScores[i] > max)
            {
                max = i;
            }
        }
        if (colorArray[max]=="1"){
            winnerMessage = ("Blue win!");
        }
        else if (colorArray[max]=="2"){
            winnerMessage = ("Red win!");
        }
        else if (colorArray[max]=="3"){
            winnerMessage = ("Green win!");
        }
        else if (colorArray[max]=="4"){
            winnerMessage = ("Yellow win!");
        }
    }

    async Task ApplySettingsFromFile()
    {       
        // асинхронное чтение
        using (StreamReader reader = new StreamReader(path))
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                string[] splittedLine = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                /*foreach(string s in splittedLine)
                {
                    Console.WriteLine(s);
                }*/

                if (splittedLine[0] == "size")
                    size = Convert.ToInt32(splittedLine[1]);               
                if (splittedLine[0] == "penalty")
                    penalty = Convert.ToInt32(splittedLine[1]);
                if (splittedLine[0] == "height")
                    height = Convert.ToInt32(splittedLine[1]);
                if (splittedLine[0] == "width")
                    width = Convert.ToInt32(splittedLine[1]);
                
            }
        }
        Debug.Log(size);
        Debug.Log(penalty);
        Debug.Log(height);
        Debug.Log(width);
    }

    void Awake()
    {
        //(1)Applying settings from file by Miron

        ApplySettingsFromFile();

        //(1)


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
        allPossibleSquares[0, 0] = colorArray[0];
        allPossibleSquares[0, m - 1] = colorArray[1];
        allPossibleSquares[n - 1, m - 1] = colorArray[2];
        allPossibleSquares[n-1, 0] = colorArray[3];


    }
    void Start(){
        music015Territories.Play();
        lastMusicPlayed="music015Territories";
    }

    // Update is called once per frame
    void Update()
    {
        if (music011MetalTheme.isPlaying == false && music015Territories.isPlaying == false){
            if (lastMusicPlayed=="music015Territories"){
                Debug.Log("play 011");
                music011MetalTheme.Play();
                lastMusicPlayed="music011MetalTheme";
            }
            else if (lastMusicPlayed=="music011MetalTheme"){
                Debug.Log("play 015");
                music015Territories.Play();
                lastMusicPlayed="music015Territories";
            }
        }
    }
}