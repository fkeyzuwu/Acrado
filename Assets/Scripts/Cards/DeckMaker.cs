using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DeckMaker : NetworkBehaviour
{
    PlayerManager playerManager;
    void Start()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        playerManager = networkIdentity.GetComponent<PlayerManager>();
        playerManager.CmdCreatePlayerDeck(networkIdentity);
    }
}
