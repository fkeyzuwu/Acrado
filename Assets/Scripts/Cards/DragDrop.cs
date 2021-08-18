using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;


public class DragDrop : NetworkBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GameObject canvas;
    private PlayerManager playerManager;

    private bool isDragging = false;
    private bool isDraggable = true;

    private GameObject startParent;
    private Vector2 startPosition;
    private GameObject dropZone;
    private bool isOverDropZone = false;

    void Start()
    {
        canvas = GameObject.Find("Main Canvas");

        if (!hasAuthority)
        {
            isDraggable = false;
        }
    }
    void Update()
    {
        if (isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player Drop Zone")
        {
            isOverDropZone = true;
            dropZone = collision.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player Drop Zone")
        {
            isOverDropZone = false;
            dropZone = null;
        }  
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDraggable) return;

        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;

        transform.SetParent(canvas.transform, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDraggable) return;

        isDragging = false;

        if (isOverDropZone && dropZone.transform.childCount < 8)
        {
            transform.SetParent(dropZone.transform, false);
            isDraggable = false;
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            playerManager = networkIdentity.GetComponent<PlayerManager>();
            playerManager.PlayCard(gameObject);
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }
    }
}
