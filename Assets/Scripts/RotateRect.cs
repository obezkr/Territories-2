using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateRect : MonoBehaviour
{
    private PlaceRect placeRectScript;
    [SerializeField] private GameObject createRect;
    [SerializeField] private CreateRect createRectScript;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameManager gameManagerScript;
    private int temp1;
    private float x, y, layerOfOutline;
    private int rectWidth, rectHeight, rectCounter;
    public void Rotate()
    {
        createRectScript = createRect.GetComponent<CreateRect>();
        gameManagerScript = gameManager.GetComponent<GameManager>();
        rectCounter = createRectScript.rectCounter;
        GameObject objectToRotate = GameObject.Find(rectCounter + "Rect");
        placeRectScript = objectToRotate.GetComponent<PlaceRect>();
        layerOfOutline = placeRectScript.layer4;
        temp1 = placeRectScript.rectWidth;
        placeRectScript.rectWidth = placeRectScript.rectHeight;
        placeRectScript.rectHeight = temp1;
        if (placeRectScript.myself.transform.rotation.z == 0)
        {
            placeRectScript.myself.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            placeRectScript.myself.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        rectWidth = placeRectScript.rectWidth;
        rectHeight = placeRectScript.rectHeight;
        x = placeRectScript.myself.transform.position.x - rectWidth / 2;
        y = placeRectScript.myself.transform.position.y + rectHeight / 2;
        // поворачиваем обводку за прямоугольником
        LineRenderer lRend1 = GameObject.Find("1Line" + rectCounter + "Rect").GetComponent<LineRenderer>();
        lRend1.SetPosition(0, new Vector3((x), (y), layerOfOutline));
        lRend1.SetPosition(1, new Vector3((x + rectWidth), (y), layerOfOutline));

        LineRenderer lRend2 = GameObject.Find("2Line" + rectCounter + "Rect").GetComponent<LineRenderer>();
        lRend2.SetPosition(0, new Vector3((x + rectWidth), (y), layerOfOutline));
        lRend2.SetPosition(1, new Vector3((x + rectWidth), (y - rectHeight), layerOfOutline));

        LineRenderer lRend3 = GameObject.Find("3Line" + rectCounter + "Rect").GetComponent<LineRenderer>();
        lRend3.SetPosition(0, new Vector3((x + rectWidth), (y - rectHeight), layerOfOutline));
        lRend3.SetPosition(1, new Vector3((x), (y - rectHeight), layerOfOutline));

        LineRenderer lRend4 = GameObject.Find("4Line" + rectCounter + "Rect").GetComponent<LineRenderer>();
        lRend4.SetPosition(0, new Vector3((x), (y - rectHeight), layerOfOutline));
        lRend4.SetPosition(1, new Vector3((x), (y), layerOfOutline));
        gameManagerScript.TuskAct4RotateThisRect(rectCounter + "Rect");
        

    }
}
