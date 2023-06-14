using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Mirror;

public class CreateRect : NetworkBehaviour
{
    [SyncVar] public int size, randWidth, randHeight, height, width;
    [SyncVar(hook = nameof(IfServerForCreateRect))] public int rectCounter=-1;
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
        if (isServer && gameManagerScript.readyToCreate==true){
            createRect = gameManagerScript.createRect;
            if (createRect == true){ //222222222222222
                
                if (isServer){
                    CreateValuesForRect();
                    //CreateRectOnClient();
                    //111111111111111
                }
                //CreateRectOnClient();
                gameManagerScript.createRect = false;
            }
        }
    }
    
    [Server]
    void CreateValuesForRect(){
        randWidth = size * UnityEngine.Random.Range(1, 6);
        randHeight = size * UnityEngine.Random.Range(1, 6);
        rectCounter++;        
        Debug.Log("called CreateValuesForRect on server");
    }


    void IfServerForCreateRect(int a, int b){ // оболочка(тепер рудимент).
        CreateRectOnClient();
    }

    void CreateRectOnClient(){ 
        // перефарбовую префаб і роблю клона з заданими параметрами
        singlePixelPrefab.GetComponent<SpriteRenderer>().color = 
                gameManagerScript.playersColorsArray[Array.IndexOf(gameManagerScript.colorArray, gameManagerScript.nowToMove)];
            GameObject newRandRect = Instantiate(singlePixelPrefab);
            newRandRect.transform.localScale = new Vector3(randWidth, randHeight, 1);
            newRandRect.transform.name = rectCounter + "Rect";
            Debug.Log("called CreateRectOnClient on client");
    }
    

}
