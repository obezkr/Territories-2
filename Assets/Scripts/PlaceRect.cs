using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceRect : NetworkBehaviour
{
    [SerializeField] private GameObject gameManager; // это сам обьект GameManager, который на сцене
    [SerializeField] private GameManager gameManagerScript; // а это его скрипт
    [SerializeField] private GameObject passMove;
    [SerializeField] private PassMove passMoveScript;
    public GameObject myself;
    private int size, height, width;
    public int rectWidth, rectHeight;
    private float x, y;
    public float layer1 = -1, layer2 = -2, layer3 = -3, layer4 = -4;
    private bool rectHold = false, isEmptySquare = true, isSurrounded=false, placed=false;
    private string[,] allSquares, allPossibleSquares;
    private string[] colorArray;
    private string nowToMove;
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        passMove = GameObject.Find("PassMove");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        passMoveScript = passMove.GetComponent<PassMove>();
        size = gameManagerScript.size;
        height = gameManagerScript.height;
        width = gameManagerScript.width;
        rectWidth = CreateRect.randWidth;
        rectHeight = CreateRect.randHeight;
        colorArray = gameManagerScript.colorArray;
        placed = false;
        if (!(myself.name.Contains("SinglePixel"))){
            DrawLines.toDrawOutlineForRect = true;
        }
        else
        {
            placed = true;
        }
        
        

    }
    private float RoundingCoord(float coord) // функция выравнивания по сетке
    {
        if (coord > size) {
            if (coord % size > size / 2)
            {
                coord = coord + size - coord % size;
            }
            else
            {
                coord = coord - coord % size;
            }
        }
        else if ((0<coord) && (coord<size))
        {
            if (coord < size / 2)
            {
                coord = coord - coord % size;
            }
            else
            {
                coord = coord + size - coord % size;
            }
        }
        else
        {
            if (Math.Abs(coord % size) > size / 2)
            {
                coord = coord - coord % size - size;
            }
            else
            {
                coord = coord - coord % size;   
            }
        }
        return coord;
    }

    void Update()
    {
        allSquares = gameManagerScript.allSquares;
        allPossibleSquares = gameManagerScript.allPossibleSquares;
        nowToMove = gameManagerScript.nowToMove;
        if ((Input.GetMouseButtonDown(0)) && placed==false)
        {
            x = myself.transform.position.x - rectWidth / 2;
            y = myself.transform.position.y + rectHeight / 2;
            Vector2 screenPositionClick = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPositionClick = Camera.main.ScreenToWorldPoint(screenPositionClick);
            if ((x - size * 2 < worldPositionClick.x) && (worldPositionClick.x < x + rectWidth + size * 2) &&
                (y + size * 2 > worldPositionClick.y) && (worldPositionClick.y > y - rectHeight - size * 2) && (rectHold == false))
            {
                rectHold = true;
            }
        }
        if ((Input.GetMouseButtonUp(0)) && placed==false)

        {
            if ((rectHold == true) && (myself.transform.position.x - rectWidth / 2 + size/2 > 0) && (myself.transform.position.x + rectWidth / 2 - size/2 < width)
                && (myself.transform.position.y + rectHeight / 2 - size/2 < height) && (myself.transform.position.y - rectHeight / 2 + size/2) > 0)
            {
                //выравнивание по сетке
                rectHold = false; 
                float leftUpCornerX = myself.transform.position.x - rectWidth / 2;
                float leftUpCornerY = myself.transform.position.y + rectHeight / 2;
                leftUpCornerX = RoundingCoord(leftUpCornerX);
                leftUpCornerY = RoundingCoord(leftUpCornerY);
                x = leftUpCornerX + rectWidth / 2;
                y = leftUpCornerY - rectHeight / 2;
                //проверяем, можно ли поставить
                isEmptySquare = true;
                isSurrounded = false;
                for (int i = 0; i < rectWidth/size; i++)
                {
                    for (int j = 0; j < rectHeight/size; j++)
                    {
                        
                        if (allSquares[(int)(leftUpCornerX/size) + i,((int)((height-leftUpCornerY)/size) + j)] != "empty" )
                        {
                            
                            isEmptySquare = false;
                            break;
                        }
                    }
                }
                //проверяем, или поблизости есть клетки того же цвета
                for (int i = 0; i < rectWidth / size; i++)
                {
                    for (int j = 0; j < rectHeight / size; j++)
                    {
                        if (allPossibleSquares[(int)(leftUpCornerX / size) + i, ((int)((height - leftUpCornerY) / size) + j)].Contains(nowToMove))
                        {
                            isSurrounded = true;
                            break;
                        }
                    }
                }
                //ставим прямоугольник
                if ((isSurrounded == true) && (isEmptySquare == true))
                {
                    myself.transform.position = new Vector3(x, y, layer1);
                    placed = true;
                    x = myself.transform.position.x - rectWidth / 2;
                    y = myself.transform.position.y + rectHeight / 2;
                    // двигаем обводку за прямоугольником
                    LineRenderer lRend1 = GameObject.Find("1Line" + CreateRect.rectCounter + "Rect").GetComponent<LineRenderer>();
                    lRend1.SetPosition(0, new Vector3((x), (y), layer2));
                    lRend1.SetPosition(1, new Vector3((x + rectWidth), (y), layer2));

                    LineRenderer lRend2 = GameObject.Find("2Line" + CreateRect.rectCounter + "Rect").GetComponent<LineRenderer>();
                    lRend2.SetPosition(0, new Vector3((x + rectWidth), (y), layer2));
                    lRend2.SetPosition(1, new Vector3((x + rectWidth), (y - rectHeight), layer2));

                    LineRenderer lRend3 = GameObject.Find("3Line" + CreateRect.rectCounter + "Rect").GetComponent<LineRenderer>();
                    lRend3.SetPosition(0, new Vector3((x + rectWidth), (y - rectHeight), layer2));
                    lRend3.SetPosition(1, new Vector3((x), (y - rectHeight), layer2));

                    LineRenderer lRend4 = GameObject.Find("4Line" + CreateRect.rectCounter + "Rect").GetComponent<LineRenderer>();
                    lRend4.SetPosition(0, new Vector3((x), (y - rectHeight), layer2));
                    lRend4.SetPosition(1, new Vector3((x), (y), layer2));
                    //отмечаем клетки
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
                    gameManagerScript.playersScores[Array.IndexOf(colorArray, nowToMove)] =
                        gameManagerScript.playersScores[Array.IndexOf(colorArray, nowToMove)] + 
                        rectHeight / size * rectWidth / size;
                    //синхронизируем массивы и передаем очередность хода (синхрон? по моєму тут не треба нічого синхронізовувати)
                    gameManagerScript.allSquares = allSquares;
                    gameManagerScript.allPossibleSquares = allPossibleSquares;
                    passMoveScript.Pass();
                }
            }
            else
            {
                rectHold = false;
            }
        }
        if ((rectHold == true) && (placed==false))
        {
            x = myself.transform.position.x - rectWidth / 2;
            y = myself.transform.position.y + rectHeight / 2;
            Vector2 screenPositionClick = new Vector2(Input.mousePosition.x, Input.mousePosition.y+size*7);
            Vector2 worldPositionClick = Camera.main.ScreenToWorldPoint(screenPositionClick);
            myself.transform.position = worldPositionClick;
            myself.transform.position = new Vector3(myself.transform.position.x, myself.transform.position.y, layer3);
            x = myself.transform.position.x - rectWidth / 2;
            y = myself.transform.position.y + rectHeight / 2;
            // двигаем обводку за прямоугольником
            LineRenderer lRend1 = GameObject.Find("1Line" + CreateRect.rectCounter + "Rect").GetComponent<LineRenderer>();
            lRend1.SetPosition(0, new Vector3((x), (y), layer4));
            lRend1.SetPosition(1, new Vector3((x + rectWidth), (y), layer4));

            LineRenderer lRend2 = GameObject.Find("2Line" + CreateRect.rectCounter + "Rect").GetComponent<LineRenderer>();
            lRend2.SetPosition(0, new Vector3((x + rectWidth), (y), layer4));
            lRend2.SetPosition(1, new Vector3((x + rectWidth), (y - rectHeight), layer4));

            LineRenderer lRend3 = GameObject.Find("3Line" + CreateRect.rectCounter + "Rect").GetComponent<LineRenderer>();
            lRend3.SetPosition(0, new Vector3((x + rectWidth), (y - rectHeight), layer4));
            lRend3.SetPosition(1, new Vector3((x), (y - rectHeight), layer4));

            LineRenderer lRend4 = GameObject.Find("4Line" + CreateRect.rectCounter + "Rect").GetComponent<LineRenderer>();
            lRend4.SetPosition(0, new Vector3((x), (y - rectHeight), layer4));
            lRend4.SetPosition(1, new Vector3((x), (y), layer4));

        }
    }
}
