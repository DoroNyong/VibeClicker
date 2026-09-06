using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    [Header("Data Reference")]
    [SerializeField] private UpgradeData data;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button buyButton;

    // 외부 노출 프로퍼티
    public UpgradeData Data => data;
    public int CurrentLevel { get; private set; } = 0;

    private void Start()
    {
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyClicked);
        }
        UpdateUI();
    }

    public void SetData(UpgradeData newData)
    {
        data = newData;
        UpdateUI();
    }

    public void OnBuyClicked()
    {
        if (data == null || CurrencyManager.Instance == null) return;

        double cost = data.GetCost(CurrentLevel);
        if (CurrencyManager.Instance.TrySpendGold(cost))
        {
            CurrentLevel++;
            CurrencyManager.Instance.GoldPerClick += data.additionalGoldPerClick;
            CurrencyManager.Instance.GoldPerSecond += data.additionalGoldPerSecond;
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        if (data == null) return;

        if (nameText != null) nameText.text = data.upgradeName;
        if (levelText != null) levelText.text = $"Lv.{CurrentLevel}";
        if (costText != null) costText.text = $"{data.GetCost(CurrentLevel):N0} G";
    }

    // SaveManager에서 로드할 때 호출
    public void LoadLevel(int level)
    {
        CurrentLevel = level;

        // 로드된 레벨만큼 CurrencyManager 스탯 복원
        if (data != null && CurrencyManager.Instance != null && level > 0)
        {
            CurrencyManager.Instance.GoldPerClick += data.additionalGoldPerClick * level;
            CurrencyManager.Instance.GoldPerSecond += data.additionalGoldPerSecond * level;
        }

        UpdateUI();
    }
}