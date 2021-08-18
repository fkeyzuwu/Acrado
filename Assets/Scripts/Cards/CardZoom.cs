using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardZoom : NetworkBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private float timer = 0;
    private float timeTillZoom = 0.15f;
    private bool isHovering = false;
    private bool isZooming = false;

    void Update()
    {
        if (isHovering && !isZooming)
        {
            timer += Time.deltaTime;

            if(timer >= timeTillZoom)
            {
                
                Zoom();
                isZooming = true;
                timer = 0;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hasAuthority) return; //add and if card is not played
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!hasAuthority) return; //add and if card is not played

        if (isHovering || isZooming)
        {
            UnZoom();
        }
    }

    private void Zoom()
    {
        transform.localScale = new Vector2(1.4f, 1.4f);
        transform.localPosition = new Vector2(transform.localPosition.x, 30);
    }

    private void UnZoom()
    {
        transform.localScale = new Vector2(1f, 1f);
        transform.localPosition = new Vector2(transform.localPosition.x, 0);
        isHovering = false;
        isZooming = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UnZoom();
    }
}
