using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Mirror;

public class CreateRectSinglePlayer : MonoBehaviour
{
    public int size, randWidth, randHeight, height, width;
    public static int rectCounter=-1;
    public bool createRect;
    [SerializeField] private GameObject singlePixelPrefab;
    [SerializeField] private GameObject gameManager; // это сам обьект GameManager, который на сцене
    [SerializeField] private GameManagerSinglePlayer gameManagerScript; // а это его скрипт
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerSinglePlayer>();
        size = gameManagerScript.size;
        height = gameManagerScript.height;
        width = gameManagerScript.width;
        createRect = gameManagerScript.createRect;
        singlePixelPrefab.transform.localScale = new Vector3(size, size, 1);
    }

    void Update()
    {
        createRect = gameManagerScript.createRect;
        if (createRect == true){
            WellCreateRect();
    
        }
    }



    void WellCreateRect(){
        rectCounter++;
        randWidth = size * UnityEngine.Random.Range(1, 6);
        randHeight = size * UnityEngine.Random.Range(1, 6);
        singlePixelPrefab.GetComponent<SpriteRenderer>().color = 
                gameManagerScript.playersColorsArray[Array.IndexOf(gameManagerScript.colorArray, gameManagerScript.nowToMove)];
            GameObject newRandRect = Instantiate(singlePixelPrefab);
            newRandRect.transform.localScale = new Vector3(randWidth, randHeight, 1);
            newRandRect.transform.name = rectCounter + "Rect";
        gameManagerScript.createRect = false;

    }

}