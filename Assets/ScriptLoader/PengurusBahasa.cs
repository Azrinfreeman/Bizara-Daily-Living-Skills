using UnityEngine;
using System;

public class PengurusBahasa : MonoBehaviour
{
    public static PengurusBahasa Instance { get; private set; }

    public static event Action OnBahasaDitukar;
    
    private DataTerjemahan dataTerjemahan;

    // "EN" for English, "MS" for Melayu
    public string BahasaSemasa
    {
        get { return PlayerPrefs.GetString("BahasaPermainan", "EN"); }
        set 
        { 
            PlayerPrefs.SetString("BahasaPermainan", value);
            PlayerPrefs.Save();
            OnBahasaDitukar?.Invoke();
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            MuatDataTerjemahan();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void MuatDataTerjemahan()
    {
        dataTerjemahan = Resources.Load<DataTerjemahan>("DataTerjemahan");
        if (dataTerjemahan != null)
        {
            dataTerjemahan.InisialisasiKamus();
        }
        else
        {
            Debug.LogError("DataTerjemahan.asset tidak dijumpai di folder Resources!");
        }
    }

    public string Terjemah(string kunci)
    {
        if (dataTerjemahan != null)
        {
            return dataTerjemahan.DapatkanTeks(kunci, BahasaSemasa);
        }
        return kunci;
    }
    
    public void TukarBahasa()
    {
        BahasaSemasa = (BahasaSemasa == "EN") ? "MS" : "EN";
    }

    // Auto-create in scene if not exists
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void AutoCreate()
    {
        if (FindObjectsByType<PengurusBahasa>(FindObjectsSortMode.None).Length == 0)
        {
            GameObject go = new GameObject("PengurusBahasa");
            go.AddComponent<PengurusBahasa>();
        }
    }
}
