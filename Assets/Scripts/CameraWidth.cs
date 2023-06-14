using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWidth : MonoBehaviour
{
    public bool maintainWidth=true;
    float defaultWidth;
    // Start is called before the first frame update
    void Start()
    {
        defaultWidth=Camera.main.orthographicSize*Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if (maintainWidth){
            Camera.main.orthographicSize=defaultWidth/Camera.main.aspect;
        }
    }
}
