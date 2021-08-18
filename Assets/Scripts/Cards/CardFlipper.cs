using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CardFlipper : MonoBehaviour
{
    [SerializeField] private Sprite cardFront;
    [SerializeField] private Sprite cardBack;
    [SerializeField] private Image background;
    [SerializeField] private GameObject cardData;

    public void Flip()
    {
        Sprite currentSprite = background.sprite;

        if(currentSprite == cardFront)
        {
            background.sprite = cardBack;
            cardData.SetActive(false);
        }
        else
        {
            background.sprite = cardFront;
            if (!cardData.activeInHierarchy)
            {
                cardData.SetActive(true);
            }
        }
    }
}
