using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ReadyButtonScript : NetworkBehaviour
{

    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameManager gameManagerScript;

    void Start(){
        if (!isServer){
            this.gameObject.SetActive(false);
        }
    }

    [Server]
    public void changeReadyStatus()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
        gameManagerScript.readyToCreate=true;
        this.gameObject.SetActive(false);
    }
}
