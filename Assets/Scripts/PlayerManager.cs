using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerManager : NetworkBehaviour
{
    private readonly string path = "Prefabs\\Cards\\ScriptableCards\\";
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
    public void CmdDealCards(int amount, bool isSenderServer)
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager playerManager = networkIdentity.GetComponent<PlayerManager>();

        //Deck deck = isSenderServer ? playerManager.player1Deck : playerManager.player2Deck;
        Deck deck = playerManager.player1Deck;

        if(deck.CardDeck.Count >= amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject cardObject = Instantiate(cardPrefab, playerArea.transform);
                NetworkServer.Spawn(cardObject, connectionToClient);
                RpcShowCard(cardObject, deck.CardDeck.Pop(), "Dealt");
            }
        }
        else
        {
            Debug.Log($"Cards in {deck} ran out");
        }
    }

    [ClientRpc]
    private void RpcShowCard(GameObject cardObject, Card card, string type)
    {
        if(type == "Dealt")
        {
            if (hasAuthority)
            {
                cardObject.transform.SetParent(playerArea.transform, false);
                CardDisplay cardDisplay = cardObject.GetComponent<CardDisplay>();
                cardDisplay.Card = card;
                cardDisplay.InitializeCard();
            }
            else
            {
                cardObject.transform.SetParent(enemyArea.transform, false);
                cardObject.GetComponent<CardFlipper>().Flip();
            }
        }
        else if(type == "Played")
        {
            if (hasAuthority)
            {
                cardObject.transform.SetParent(playerDropZone.transform, false);
            }
            else 
            {
                cardObject.transform.SetParent(enemyDropZone.transform, false);
                CardDisplay cardDisplay = cardObject.GetComponent<CardDisplay>();
                cardDisplay.Card = card;
                cardDisplay.InitializeCard();
                cardObject.GetComponent<CardFlipper>().Flip();
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
        RpcShowCard(card, card.GetComponent<CardDisplay>().Card, "Played");
    }
}
