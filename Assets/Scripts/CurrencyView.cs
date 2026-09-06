using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Button clickButton;

    private void Start()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnGoldChanged += UpdateGoldUI;
            UpdateGoldUI(CurrencyManager.Instance.CurrentGold);
        }

        if (clickButton != null)
        {
            clickButton.onClick.AddListener(OnClickButton);
        }
    }

    private void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnGoldChanged -= UpdateGoldUI;
        }
    }

    private void OnClickButton()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.Click();
        }
    }

    private void UpdateGoldUI(double currentGold)
    {
        if (goldText != null)
        {
            goldText.text = $"{currentGold:N0} G";
        }
    }
}