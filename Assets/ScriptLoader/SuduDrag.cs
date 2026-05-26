using UnityEngine;
using UnityEngine.EventSystems;

public class SuduDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private Vector3 posisiAsal; 
    [HideInInspector] public bool berjayaDiletakkan = false; 

    // We need these two to calculate the correct UI scale
    private RectTransform rectTransform;
    private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>(); // Automatically finds your Canvas

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        posisiAsal = transform.position; 
        
        transform.SetAsLastSibling(); 
        canvasGroup.blocksRaycasts = false;
        berjayaDiletakkan = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        // THE FIX: This moves the spoon exactly with your mouse, respecting the Canvas scale!
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (!berjayaDiletakkan)
        {
            transform.position = posisiAsal;
        }
    }
}