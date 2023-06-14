using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLinesSinglePlayer : MonoBehaviour
{
    private int size, width, height;
    public static bool toDrawOutlineForRect = false;
    [SerializeField] private GameObject lineGeneratorPrefab;
    [SerializeField] private GameObject gameManager; // это сам обьект GameManager, который на сцене
    [SerializeField] private GameManagerSinglePlayer gameManagerScript; // а это его скрипт
    private PlaceRectSinglePlayer placeRectScript;

    private void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManagerSinglePlayer>(); // с обьекта_берем_наш-скрипт
        width = gameManagerScript.width;
        height = gameManagerScript.height;
        size = gameManagerScript.size;
        SpawnLineGenerator();
        
    }

    private void SpawnLineGenerator()
    {
        for (int j = 0; j <= height; j = j + size)
        {
            GameObject newLineGen = Instantiate(lineGeneratorPrefab);
            LineRenderer lRend = newLineGen.GetComponent<LineRenderer>();
            lRend.SetPosition(0, new Vector3(0, j, 10));
            lRend.SetPosition(1, new Vector3(width, j, 10));

        }
        for (int i = 0; i <= width; i = i + size)
        {
            GameObject newLineGen = Instantiate(lineGeneratorPrefab);
            LineRenderer lRend = newLineGen.GetComponent<LineRenderer>();
            lRend.SetPosition(0, new Vector3(i, 0, 10));
            lRend.SetPosition(1, new Vector3(i, height, 10));

        }

    }
    private void DrawLinesForRect()
    {
        GameObject currentRect = GameObject.Find(CreateRectSinglePlayer.rectCounter + "Rect"); //находим текущий прямоугольник
        placeRectScript = currentRect.GetComponent<PlaceRectSinglePlayer>();
        float rectHeight = placeRectScript.rectHeight;
        float rectWidth = placeRectScript.rectWidth;

        GameObject newLineGen1 = Instantiate(lineGeneratorPrefab); // верхняя линия обводки
        LineRenderer lRend1 = newLineGen1.GetComponent<LineRenderer>();
        lRend1.name = "1Line" + currentRect.name;
        lRend1.SetPosition(0, new Vector3((int)(placeRectScript.transform.position.x - rectWidth / 2), (int)(placeRectScript.transform.position.y + rectHeight / 2), (float)(0)));
        lRend1.SetPosition(1, new Vector3((int)(placeRectScript.transform.position.x + rectWidth / 2), (int)(placeRectScript.transform.position.y + rectHeight / 2), (float)(0)));

        GameObject newLineGen2 = Instantiate(lineGeneratorPrefab); // правая линия обводки
        LineRenderer lRend2 = newLineGen2.GetComponent<LineRenderer>();
        lRend2.name = "2Line" + currentRect.name;
        lRend2.SetPosition(0, new Vector3((int)(placeRectScript.transform.position.x + rectWidth / 2), (int)(placeRectScript.transform.position.y + rectHeight / 2), (float)(0)));
        lRend2.SetPosition(1, new Vector3((int)(placeRectScript.transform.position.x + rectWidth / 2), (int)(placeRectScript.transform.position.y - rectHeight / 2), (float)(0)));

        GameObject newLineGen3 = Instantiate(lineGeneratorPrefab); // нижняя линия обводки 
        LineRenderer lRend3 = newLineGen3.GetComponent<LineRenderer>();
        lRend3.name = "3Line" + currentRect.name;
        lRend3.SetPosition(0, new Vector3((int)(placeRectScript.transform.position.x + rectWidth / 2), (int)(placeRectScript.transform.position.y - rectHeight / 2), (float)(0)));
        lRend3.SetPosition(1, new Vector3((int)(placeRectScript.transform.position.x - rectWidth / 2), (int)(placeRectScript.transform.position.y - rectHeight / 2), (float)(0)));

        GameObject newLineGen4 = Instantiate(lineGeneratorPrefab); // левая линия обводки 
        LineRenderer lRend4 = newLineGen4.GetComponent<LineRenderer>();
        lRend4.name = "4Line" + currentRect.name;
        lRend4.SetPosition(0, new Vector3((int)(placeRectScript.transform.position.x - rectWidth / 2), (int)(placeRectScript.transform.position.y - rectHeight / 2), (float)(0)));
        lRend4.SetPosition(1, new Vector3((int)(placeRectScript.transform.position.x - rectWidth / 2), (int)(placeRectScript.transform.position.y + rectHeight / 2), (float)(0)));


    }

    private void Update()
    {
        if (toDrawOutlineForRect == true)
        {
            toDrawOutlineForRect = false;
            DrawLinesForRect();
        }

    }
}
