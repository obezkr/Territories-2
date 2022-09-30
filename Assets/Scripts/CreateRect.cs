using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRect : MonoBehaviour
{
    public static int size, randWidth, randHeight, height, width;
    public static int rectCounter=-1;
    public static bool createRect;
    [SerializeField] private GameObject singlePixelBluePrefab, singlePixelRedPrefab, singlePixelGreenPrefab, singlePixelYellowPrefab;
    [SerializeField] private GameObject gameManager; // это сам обьект GameManager, который на сцене
    [SerializeField] private GameManager gameManagerScript; // а это его скрипт
    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
        size = gameManagerScript.size;
        height = gameManagerScript.height;
        width = gameManagerScript.width;
        createRect = gameManagerScript.createRect;
        singlePixelBluePrefab.transform.localScale = new Vector3(size, size, 1);
        singlePixelRedPrefab.transform.localScale = new Vector3(size, size, 1);
        singlePixelGreenPrefab.transform.localScale = new Vector3(size, size, 1);
        singlePixelYellowPrefab.transform.localScale = new Vector3(size, size, 1);
    }

    void Update()
    {
        if (createRect == true)
        {
            rectCounter++;
            createRect = false;
            randWidth = size * Random.Range(1, 6);
            randHeight = size * Random.Range(1, 6);
            /* тут можно заменить префаб каждого цвета на один глобальный префаб,
             * который просто потом перекрашивать в цвет nowToMove 
             */
            if (gameManagerScript.nowToMove == "blue") 
            {
                GameObject newRandRect = Instantiate(singlePixelBluePrefab);
                newRandRect.transform.localScale = new Vector3(randWidth, randHeight, 1);
                newRandRect.transform.name = rectCounter + "Rect";
            }
            else if (gameManagerScript.nowToMove == "red")
            {
                GameObject newRandRect = Instantiate(singlePixelRedPrefab);
                newRandRect.transform.localScale = new Vector3(randWidth, randHeight, 1);
                newRandRect.transform.name = rectCounter + "Rect";
            }
            else if (gameManagerScript.nowToMove == "green")
            {
                GameObject newRandRect = Instantiate(singlePixelGreenPrefab);
                newRandRect.transform.localScale = new Vector3(randWidth, randHeight, 1);
                newRandRect.transform.name = rectCounter + "Rect";
            }
            else if (gameManagerScript.nowToMove == "yellow")
            {
                GameObject newRandRect = Instantiate(singlePixelYellowPrefab);
                newRandRect.transform.localScale = new Vector3(randWidth, randHeight, 1);
                newRandRect.transform.name = rectCounter + "Rect";  
            }
        }
    }
}
