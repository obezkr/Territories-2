using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRect : MonoBehaviour
{
    public static int size, randWidth, randHeight, height, width;
    public static int rectCounter=-1;
    public static bool createRect;
    
    [SerializeField] private GameObject singlePixelPrefab;
    [SerializeField] private GameObject gameManager; // это сам обьект GameManager, который на сцене
    [SerializeField] private GameManager gameManagerScript; // а это его скрипт
    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
        size = gameManagerScript.size;
        height = gameManagerScript.height;
        width = gameManagerScript.width;
        createRect = gameManagerScript.createRect;
        singlePixelPrefab.transform.localScale = new Vector3(size, size, 1);
    }

    void Update()
    {
        if (createRect == true)
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
        }
    }
}
