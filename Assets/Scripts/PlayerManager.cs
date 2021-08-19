using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] private GameObject playerDeckPrefab;
    [SerializeField] private GameObject cardPrefab;

    private GameObject playerArea;
    private GameObject playerDropZone;
    private GameObject enemyArea;
    private GameObject enemyDropZone;

    private Deck player1Deck; //player 1 is server(host) for now
    private Deck player2Deck; //player 2 is client for now

    public override void OnStartClient()
    {
        base.OnStartClient();

        playerArea = GameObject.Find("Player Area");
        playerDropZone = GameObject.Find("Player Drop Zone");
        enemyArea = GameObject.Find("Enemy Area");
        enemyDropZone = GameObject.Find("Enemy Drop Zone");
    }

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
        CmdCreatePlayerDeck();
    }

    [Server]
    public void CmdCreatePlayerDeck()
    {
        if (NetworkServer.connections.Count < 2)
        {
            GameObject deckGameObject = Instantiate(playerDeckPrefab);
            player1Deck = deckGameObject.GetComponent<Deck>();
        }
        else
        {
            GameObject deckGameObject = Instantiate(playerDeckPrefab);
            player2Deck = deckGameObject.GetComponent<Deck>();
        }
    }

    [Command]
    public void CmdDealCards(int amount)
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager playerManager = networkIdentity.GetComponent<PlayerManager>();

        Deck deck;

        if (isServer)
        {
            deck = playerManager.player1Deck;
        }
        else
        {
            deck = playerManager.player2Deck;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject card = Instantiate(cardPrefab, playerArea.transform);
            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
            cardDisplay.Card = deck.CardDeck.Pop();
            cardDisplay.InitializeCard();
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, "Dealt");
        }
    }

    [ClientRpc]
    private void RpcShowCard(GameObject card, string type)
    {
        if(type == "Dealt")
        {
            if (hasAuthority)
            {
                card.transform.SetParent(playerArea.transform, false);
            }
            else
            {
                card.transform.SetParent(enemyArea.transform, false);
                card.GetComponent<CardFlipper>().Flip();
            }
        }
        else if(type == "Played")
        {
            if (hasAuthority)
            {
                card.transform.SetParent(playerDropZone.transform, false);
            }
            else 
            {
                card.transform.SetParent(enemyDropZone.transform, false);
                card.GetComponent<CardFlipper>().Flip();
            }
        }
    }

    public void PlayCard(GameObject card)
    {
        CmdPlayCard(card);
    }

    [Command]
    private void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card, "Played");
    }
}
