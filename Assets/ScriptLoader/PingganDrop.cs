using UnityEngine;
using UnityEngine.EventSystems;

public class PingganDrop : MonoBehaviour, IDropHandler
{
    private ModulManager manager;

    void Start()
    {
        // Cari ModulManager di dalam scene secara automatik
        manager = FindObjectOfType<ModulManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject objekYangDilepas = eventData.pointerDrag;

        if (objekYangDilepas != null)
        {
            SuduDrag sudu = objekYangDilepas.GetComponent<SuduDrag>();

            if (sudu != null)
            {
                sudu.berjayaDiletakkan = true;
                sudu.transform.position = transform.position; 
                
                Debug.Log("Berjaya! Memanggil Modul Manager untuk tukar panel...");

                // Beritahu manager bahawa game ini sudah selesai!
                if (manager != null)
                {
                    manager.SelesaiGameDrag();
                }
            }
        }
    }
}