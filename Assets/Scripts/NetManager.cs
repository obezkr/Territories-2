using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetManager : NetworkBehaviour
{

    [SerializeField] private GameObject gameManager; // это сам обьект GameManager, который на сцене
    [SerializeField] private GameManager gameManagerScript; // а это его скрипт
    // Start is called before the first frame update
    void Start()
    {
        if (isServer){
            NetworkConnectionToClient conn = NetworkServer.connections[0];
            gameManagerScript.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (isServer){
            if (NetworkServer.connections.Count == gameManagerScript.numberOfPlayers){
                int a = (((Array.IndexOf(colorArray, gameManagerScript.nowToMove) + 1) % numberOfPlayers));
            }
            AssignAuthorityToChosenClient();
        }*/
        //Debug.Log(gameManagerScript.GetComponent<NetworkIdentity>().isOwned);
    }

    

}
