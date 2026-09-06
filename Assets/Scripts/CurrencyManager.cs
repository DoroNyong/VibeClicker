using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public double CurrentGold { get; private set; } = 0;
    public double GoldPerClick { get; set; } = 1;
    public double GoldPerSecond { get; set; } = 0;

    // UI가 값 변경을 감지할 수 있도록 발행하는 이벤트
    public event Action<double> OnGoldChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (GoldPerSecond > 0)
        {
            AddGold(GoldPerSecond * Time.deltaTime);
        }
    }

    public void Click()
    {
        AddGold(GoldPerClick);
    }

    public void AddGold(double amount)
    {
        CurrentGold += amount;
        OnGoldChanged?.Invoke(CurrentGold);
    }

    public bool TrySpendGold(double amount)
    {
        if (CurrentGold >= amount)
        {
            CurrentGold -= amount;
            OnGoldChanged?.Invoke(CurrentGold);
            return true;
        }
        return false;
    }
}
