using UnityEngine;
using System.Collections;

public class ButangSalah : MonoBehaviour
{
    private bool sedangBergoyang = false;
    private Vector3 posisiAsal;

    void Start()
    {
        // Simpan kedudukan asal butang apabila game bermula
        posisiAsal = transform.localPosition;
    }

    public void Goyang()
    {
        // Pastikan animasi tidak bertindih jika pemain tekan banyak kali dengan cepat
        if (!sedangBergoyang)
        {
            StartCoroutine(AnimasiGoyang());
        }
    }

    private IEnumerator AnimasiGoyang()
    {
        sedangBergoyang = true;
        
        // Buat pergerakan ke kiri dan kanan sebanyak 3 kali (wobble effect)
        for (int i = 0; i < 3; i++)
        {
            transform.localPosition = posisiAsal + new Vector3(15f, 0, 0); // Gerak ke kanan sedikit
            yield return new WaitForSeconds(0.05f);
            transform.localPosition = posisiAsal - new Vector3(15f, 0, 0); // Gerak ke kiri sedikit
            yield return new WaitForSeconds(0.05f);
        }
        
        // Pastikan butang kembali tepat ke posisi asalnya
        transform.localPosition = posisiAsal;
        sedangBergoyang = false;
    }
}