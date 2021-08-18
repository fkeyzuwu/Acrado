using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DrawCards : MonoBehaviour
{
    private Transform playerArea;
    private PlayerManager playerManager;

    void Start()
    {
        playerArea = GameObject.Find("Player Area").transform;
    }

    public void OnClick()
    {
        if(playerArea.childCount < 8)
        {
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            playerManager = networkIdentity.GetComponent<PlayerManager>();
            playerManager.CmdDealCards(1);
        }
    }
}
