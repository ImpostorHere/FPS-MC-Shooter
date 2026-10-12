using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI hpIndicatorTmp;
    public TextMeshProUGUI ammoIndicatorTmp;

    public static UIManager Instance { get; set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null && Instance != this)
            Destroy(gameObject);
    }

    public void UpdateAmmoIndicator(int current, int max)
    {
        ammoIndicatorTmp.text = $"{current}/{max}";
    }

    public void UpdateHpIndicator(int current, int max)
    {
        hpIndicatorTmp.text = $"{current}/{max}";
    }
}
