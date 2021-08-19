using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class CardDisplay : NetworkBehaviour
{
    [SerializeField] private Card card;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private Image image;

    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI healthText;

    public void InitializeCard()
    {
        gameObject.name = card.name;

        nameText.text = card.name;
        descriptionText.text = card.description;
        //image.sprite = card.sprite;
        manaText.text = card.manaCost.ToString();
        attackText.text = card.attack.ToString();
        healthText.text = card.health.ToString();
    }

    public Card Card
    {
        get { return card; }
        set { card = value; }
    }

    public void PrintCard()
    {
        Debug.Log(card.name);
    }
}
