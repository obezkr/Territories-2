using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Mirror;

public class CreateRect : NetworkBehaviour
{
    [SyncVar] public int size, randWidth, randHeight, height, width;
    public static int rectCounter=-1;
    [SyncVar] public bool createRect;
    [SerializeField] private GameObject singlePixelPrefab;
    [SerializeField] private GameObject gameManager; // это сам обьект GameManager, который на сцене
    [SerializeField] private GameManager gameManagerScript; // а это его скрипт
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        size = gameManagerScript.size;
        height = gameManagerScript.height;
        width = gameManagerScript.width;
        createRect = gameManagerScript.createRect;
        singlePixelPrefab.transform.localScale = new Vector3(size, size, 1);
    }

    void Update()
    {
        if (createRect == true){
            createRect = false;
            if (isServer){
                CreateValuesForRect();
                AssignAuthorityToChosenClient(); //111111111111111
            }
            CreateRectOnClient();
        }
    }
    [Server]
    void CreateValuesForRect(){
        rectCounter++;
        randWidth = size * UnityEngine.Random.Range(1, 6);
        randHeight = size * UnityEngine.Random.Range(1, 6);
        Debug.Log("called CreateValuesForRect on server");
    }

    [Server]
    void AssignAuthorityToChosenClient(){ //11111111111111111  ^  
        
        NetworkConnectionToClient conn = NetworkServer.connections[0];
        this.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
    }


    [Client]
    void CreateRectOnClient(){
        // перефарбовую префаб і роблю клона з заданими параметрами
        singlePixelPrefab.GetComponent<SpriteRenderer>().color = 
                gameManagerScript.playersColorsArray[Array.IndexOf(gameManagerScript.colorArray, gameManagerScript.nowToMove)];
            GameObject newRandRect = Instantiate(singlePixelPrefab);
            newRandRect.transform.localScale = new Vector3(randWidth, randHeight, 1);
            newRandRect.transform.name = rectCounter + "Rect";
            Debug.Log("called CreateValuesForRect clientRPC");
    }
    [Command]
    public void AssigncreateRect(bool flag){
        Debug.Log("assign createRect");
        createRect=flag;
    }



}
