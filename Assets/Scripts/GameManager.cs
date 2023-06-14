using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;
using System;
using Mirror;

public class GameManager : NetworkBehaviour
{
    // Start is called before the first frame update
    [SyncVar] public int fps = 30, numberOfPlayers = 4, size, penalty, height, width;
    [SyncVar] public float currentxToSync=0, currentyToSync=0;
    [SyncVar(hook = "AssignAuthorityToChosenClient")] public string nowToMove = "1";
    [SyncVar] public string winnerMessage = "", currentRect, rectToDestroy;
    [SyncVar] public bool createRect = true, readyToCreate = false, currentrectHoldToSync=false, currentplacedToSync=false;
    [SyncVar] public Vector3 currentRotToSync;
    public AudioSource soundPlaceRect;
    public AudioSource soundDestroyRect;
    public AudioSource music015Territories;
    public AudioSource music011MetalTheme;
    public String lastMusicPlayed;
    public string[,] allSquares, allPossibleSquares;
    public string[] colorArray = {"1", "2", "3", "4"};
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
    [Command]
    public void ChangeCurrentRectVars(float x, float y, bool b1, bool b2, Vector3 rotation){
        currentxToSync=x;
        currentyToSync=y;
        currentrectHoldToSync=b1;
        currentplacedToSync=b2;
        currentRotToSync=rotation;
        ValuesUpdateOnClients(x, y, b1, b2, rotation);
    }
    [Command]
    public void AssigncreateRect(bool flag, string c){
        AssigncreateRectAndnowToMoveOnAllClients(flag, c);

    }
    [Command]
    public void SayToFinallyPlaceRectOnClients(float x, float y, bool b1, bool b2, string name, int rectWidth, int rectHeight, float layer1, float layer2, float leftUpCornerX, float leftUpCornerY){
        FinalPlacement(x, y, b1, b2,name, rectWidth, rectHeight, layer1, layer2, leftUpCornerX, leftUpCornerY);
    }
    [Command]
    public void SayToDestroyRectOnClients(string name){
        WipeOutThisRectFromExistence(name);
    }
    [Command]
    public void TuskAct4RotateThisRect(string name){
        RotateRectInAllDimensions(name);
    }

    public void AssignAuthorityToChosenClient(string a, string b){ 
        Debug.Log("assign is at least working");
        if (isServer){
            if (numberOfPlayers == (int)NetworkServer.connections.Count){
                if (true){ 
                    this.GetComponent<NetworkIdentity>().RemoveClientAuthority();
                }
                NetworkConnectionToClient conn = NetworkServer.connections[((Array.IndexOf(colorArray, nowToMove)) % numberOfPlayers)];
                this.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
            }
            if (numberOfPlayers == (int)(NetworkServer.connections.Count*2)){
                Debug.Log("it went to second if");
                if (true){ 
                    this.GetComponent<NetworkIdentity>().RemoveClientAuthority();
                    Debug.Log("it removed authority? i guess");
                }
                NetworkConnectionToClient conn = NetworkServer.connections[((Array.IndexOf(colorArray, nowToMove)) % (int)(numberOfPlayers/2))];
                this.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
                Debug.Log("and gave authority to connection number: ");
                Debug.Log(((Array.IndexOf(colorArray, nowToMove)) % (int)(numberOfPlayers/2)));
            }
        }
        
    }
    [ClientRpc]
    public void RotateRectInAllDimensions(string name){
        if (isOwned == false){
            GameObject objectToRotate = GameObject.Find(name);
            PlaceRect placeRectScript = objectToRotate.GetComponent<PlaceRect>();
            float layerOfOutline = placeRectScript.layer4;
            int temp1 = placeRectScript.rectWidth;
            placeRectScript.rectWidth = placeRectScript.rectHeight;
            placeRectScript.rectHeight = temp1;
            int rectWidth = placeRectScript.rectWidth;
            int rectHeight = placeRectScript.rectHeight;
            float x = placeRectScript.myself.transform.position.x - rectWidth / 2;
            float y = placeRectScript.myself.transform.position.y + rectHeight / 2;
            LineRenderer lRend1 = GameObject.Find("1Line" + name).GetComponent<LineRenderer>();
            lRend1.SetPosition(0, new Vector3((x), (y), layerOfOutline));
            lRend1.SetPosition(1, new Vector3((x + rectWidth), (y), layerOfOutline));

            LineRenderer lRend2 = GameObject.Find("2Line" + name).GetComponent<LineRenderer>();
            lRend2.SetPosition(0, new Vector3((x + rectWidth), (y), layerOfOutline));
            lRend2.SetPosition(1, new Vector3((x + rectWidth), (y - rectHeight), layerOfOutline));

            LineRenderer lRend3 = GameObject.Find("3Line" + name).GetComponent<LineRenderer>();
            lRend3.SetPosition(0, new Vector3((x + rectWidth), (y - rectHeight), layerOfOutline));
            lRend3.SetPosition(1, new Vector3((x), (y - rectHeight), layerOfOutline));

            LineRenderer lRend4 = GameObject.Find("4Line" + name).GetComponent<LineRenderer>();
            lRend4.SetPosition(0, new Vector3((x), (y - rectHeight), layerOfOutline));
            lRend4.SetPosition(1, new Vector3((x), (y), layerOfOutline));
        }
    }

    [ClientRpc]
    public void WipeOutThisRectFromExistence(string name){
        // отнимаем очки
        soundDestroyRect.Play();
        if (isOwned==false){
            playersScores[Array.IndexOf(colorArray, nowToMove)] =
                playersScores[Array.IndexOf(colorArray, nowToMove)] - penalty;
            // уничтожаем прямоугольник и его обводку
            GameObject objectToDestroy = GameObject.Find(name);
            Destroy(objectToDestroy);
            GameObject lRend1 = GameObject.Find("1Line" + name);
            GameObject lRend2 = GameObject.Find("2Line" + name);
            GameObject lRend3 = GameObject.Find("3Line" + name);
            GameObject lRend4 = GameObject.Find("4Line" + name);
            Destroy(lRend1);
            Destroy(lRend2);
            Destroy(lRend3);
            Destroy(lRend4);
        }
    }
    [ClientRpc]
    public void AssigncreateRectAndnowToMoveOnAllClients(bool flag, string c){
        Debug.Log("assign createRect");
        nowToMove=c;
        createRect=flag;
    }
    [ClientRpc]
    public void ValuesUpdateOnClients(float x, float y, bool b1, bool b2, Vector3 rotation){
        currentxToSync=x;
        currentyToSync=y;
        currentrectHoldToSync=b1;
        currentplacedToSync=b2;
        currentRotToSync=rotation;
    }
    [ClientRpc]
    public void FinalPlacement(float x, float y, bool b1, bool b2, string name, int rectWidth, int rectHeight, float layer1, float layer2, float leftUpCornerX, float leftUpCornerY){ 
        GameObject objectToPlace = GameObject.Find(name);
        objectToPlace.transform.position = new Vector3(x, y, layer1);
        objectToPlace.GetComponent<PlaceRect>().placed = true;
        x = objectToPlace.transform.position.x - rectWidth / 2;
        y = objectToPlace.transform.position.y + rectHeight / 2;
        // двигаем обводку за прямоугольником
        LineRenderer lRend1 = GameObject.Find("1Line" + name).GetComponent<LineRenderer>();
        lRend1.SetPosition(0, new Vector3((x), (y), layer2));
        lRend1.SetPosition(1, new Vector3((x + rectWidth), (y), layer2));

        LineRenderer lRend2 = GameObject.Find("2Line" + name).GetComponent<LineRenderer>();
        lRend2.SetPosition(0, new Vector3((x + rectWidth), (y), layer2));
        lRend2.SetPosition(1, new Vector3((x + rectWidth), (y - rectHeight), layer2));

        LineRenderer lRend3 = GameObject.Find("3Line" + name).GetComponent<LineRenderer>();
        lRend3.SetPosition(0, new Vector3((x + rectWidth), (y - rectHeight), layer2));
        lRend3.SetPosition(1, new Vector3((x), (y - rectHeight), layer2));

        LineRenderer lRend4 = GameObject.Find("4Line" + name).GetComponent<LineRenderer>();
        lRend4.SetPosition(0, new Vector3((x), (y - rectHeight), layer2));
        lRend4.SetPosition(1, new Vector3((x), (y), layer2));
        soundPlaceRect.Play();
        //отмечаем клетки
        if (isOwned==false){
            for (int i = 0; i < rectWidth / size; i++)
            {
                for (int j = 0; j < rectHeight / size; j++)
                {
                    allSquares[(int)(leftUpCornerX / size) + i, ((int)((height - leftUpCornerY) / size) + j)] = nowToMove; 
                }
            }
            //отмечаем возможные клетки
            for (int i = 0; i < rectWidth / size + 2; i++)
            {
                for (int j = 0; j < rectHeight / size + 2; j++)
                {
                    if (((int)((leftUpCornerX / size) + i - 1) < width/size)
                        && (0 <= (int)((leftUpCornerX / size) + i - 1))
                        && (((int)((height - leftUpCornerY) / size) + j - 1) < height/size)
                        && (0 <= ((int)((height - leftUpCornerY) / size) + j - 1)))
                    {
                        if (!(((i==0) && (j==0)) || ((i == rectWidth / size + 1) && (j == rectHeight / size + 1)) || ((i == 0) && (j == rectHeight / size + 1)) || ((i == rectWidth / size + 1) && (j == 0))))
                        {
                            allPossibleSquares[(int)((leftUpCornerX / size) + i - 1), ((int)((height - leftUpCornerY) / size) + j - 1)] = 
                            allPossibleSquares[(int)((leftUpCornerX / size) + i - 1), ((int)((height - leftUpCornerY) / size) + j - 1)] + nowToMove;  
                        }
                    }  
                }
            }
            //добавляем очки
            playersScores[Array.IndexOf(colorArray, nowToMove)] =
                playersScores[Array.IndexOf(colorArray, nowToMove)] + 
                rectHeight / size * rectWidth / size; 
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

    public override void OnStartAuthority()
    {
        // Custom logic to handle authority assignment on the client
        Debug.Log("Authority assigned to client");

        // Perform any necessary actions when the client receives authority
        // For example, enable player controls or interact with the object
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
