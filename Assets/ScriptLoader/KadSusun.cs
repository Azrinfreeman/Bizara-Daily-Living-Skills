using UnityEngine;
using UnityEngine.EventSystems;

public class KadSusun : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [HideInInspector] public Vector3 posisiAsal;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Canvas canvas;
    
    [Header("Identiti Kad")]
    public int idKad; 
    
    private ModulManager manager;

    // --- PEMBOLEH UBAH BAHARU UNTUK KUNCI KAD ---
    [HideInInspector] public bool sudahDikunci = false; 

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        manager = FindObjectOfType<ModulManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Jika sudah dikunci, berhenti di sini dan abaikan arahan drag
        if (sudahDikunci) return; 

        posisiAsal = transform.position; 
        transform.SetAsLastSibling(); 
        canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (sudahDikunci) return; // Jangan benarkan ia bergerak jika dikunci

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // KEMAS KINI: Pastikan kad sentiasa kembali ke posisi sepatutnya DAHULU
        transform.position = posisiAsal; 

        if (sudahDikunci) return; // Baru periksa jika ia dikunci

        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (sudahDikunci) return; 

        GameObject objekYangDilepas = eventData.pointerDrag;

        if (objekYangDilepas != null)
        {
            KadSusun kadYangDitarik = objekYangDilepas.GetComponent<KadSusun>();

            if (kadYangDitarik != null && kadYangDitarik != this && !kadYangDitarik.sudahDikunci)
            {
                Vector3 posisiKadIni = transform.position;
                
                // 1. Kad ini (yang kena tindih) bertukar ke tempat asal kad yang ditarik
                transform.position = kadYangDitarik.posisiAsal; 
                this.posisiAsal = transform.position; 
                
                // 2. Kad yang ditarik bertukar ke tempat asal kad ini
                kadYangDitarik.posisiAsal = posisiKadIni; 
                
                // --- KEMAS KINI: Paksa kad yang ditarik terus masuk ke dalam kotak sekarang juga! ---
                kadYangDitarik.transform.position = posisiKadIni; 
                
                Debug.Log("Kad bertukar tempat!");

                if (manager != null)
                {
                    manager.SemakSusunanKad();
                }
            }
        }
    }
}