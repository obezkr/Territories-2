using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CreateRect : NetworkBehaviour
{
    public static int size, randWidth, randHeight, height, width;
    public static int rectCounter=-1;
    public static bool createRect;
    [SyncVar] public bool isPlayerAHost=false;
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
        if (!isPlayerAHost && isLocalPlayer && isServer)
        {
            isPlayerAHost=true;
            //Instantiate(singlePixelPrefab);
        }
    }

    void Update()
    {
        if (createRect == true /*&& isPlayerAHost==true && isLocalPlayer==true*/)
        {
            rectCounter++;
            createRect = false;
            randWidth = size * UnityEngine.Random.Range(1, 6);
            randHeight = size * UnityEngine.Random.Range(1, 6);
            // перефарбовую префаб і роблю клона з заданими параметрами
            singlePixelPrefab.GetComponent<SpriteRenderer>().color = 
                gameManagerScript.playersColorsArray[Array.IndexOf(gameManagerScript.colorArray, gameManagerScript.nowToMove)];
            GameObject newRandRect = Instantiate(singlePixelPrefab);
            newRandRect.transform.localScale = new Vector3(randWidth, randHeight, 1);
            newRandRect.transform.name = rectCounter + "Rect";
            NetworkServer.Spawn(newRandRect); //give authority to client    111111111111111111111111111111111111111111111
            
        }
    }

}
