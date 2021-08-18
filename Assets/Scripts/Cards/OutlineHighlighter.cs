using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutlineHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHighlting = false;
    [SerializeField] private Color startColor;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Image highlight;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHighlting = true;
        StopAllCoroutines();
        StartCoroutine(HighlightOverTime());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHighlting = false;
        StopAllCoroutines();
        StartCoroutine(DeHighlightOverTime());
    }

    IEnumerator HighlightOverTime()
    {
        Color changingColor = startColor;

        while (changingColor.a <= highlightColor.a)
        {
            if (isHighlting)
            {
                float newA = changingColor.a + 0.01f;
                changingColor = new Color(changingColor.r, changingColor.g, changingColor.b, newA);
                highlight.color = changingColor;
                yield return new WaitForSeconds(0.005f);
            }
            else
            {
                StopAllCoroutines();
            }
        }   
    }

    IEnumerator DeHighlightOverTime()
    {
        Color changingColor = highlight.color;

        while (changingColor.a > 0)
        {
            if (!isHighlting)
            {
                float newA = changingColor.a -= 0.01f;
                changingColor = new Color(changingColor.r, changingColor.g, changingColor.b, newA);
                highlight.color = changingColor;
                yield return new WaitForSeconds(0.005f);
            }
            else
            {
                StopAllCoroutines();
            }
        }
    }
}
