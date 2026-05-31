using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class KunciTerjemahan
{
    public string kunci;
    [TextArea(2, 5)] public string teksInggeris;
    [TextArea(2, 5)] public string teksMelayu;
}

[CreateAssetMenu(fileName = "DataTerjemahan", menuName = "Antigravity/Data Terjemahan")]
public class DataTerjemahan : ScriptableObject
{
    public List<KunciTerjemahan> senaraiTerjemahan = new List<KunciTerjemahan>();

    private Dictionary<string, KunciTerjemahan> kamus;

    public void InisialisasiKamus()
    {
        kamus = new Dictionary<string, KunciTerjemahan>();
        foreach (var item in senaraiTerjemahan)
        {
            if (!kamus.ContainsKey(item.kunci))
            {
                kamus.Add(item.kunci, item);
            }
        }
    }

    public string DapatkanTeks(string kunci, string bahasa)
    {
        if (kamus == null || kamus.Count == 0) InisialisasiKamus();

        if (kamus.TryGetValue(kunci, out KunciTerjemahan data))
        {
            return bahasa == "EN" ? data.teksInggeris : data.teksMelayu;
        }

        return $"[{kunci}]"; // Fallback to show missing key
    }
}
