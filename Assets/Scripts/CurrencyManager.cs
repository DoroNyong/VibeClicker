using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public double CurrentGold { get; private set; } = 0;
    public double GoldPerClick { get; set; } = 1;
    public double GoldPerSecond { get; set; } = 0;

    public event Action<double> OnGoldChanged;

    private long lastDisplayedGold = 0;

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
        NotifyIfDisplayChanged();
    }

    public bool TrySpendGold(double amount)
    {
        if (CurrentGold >= amount)
        {
            CurrentGold -= amount;
            NotifyIfDisplayChanged();
            return true;
        }
        return false;
    }

    public void SetGold(double gold)
    {
        CurrentGold = gold;
        NotifyIfDisplayChanged();
    }

    public void ResetState()
    {
        CurrentGold = 0;
        GoldPerClick = 1;
        GoldPerSecond = 0;
        lastDisplayedGold = (long)Math.Floor(CurrentGold);
        OnGoldChanged?.Invoke(CurrentGold);
    }

    private void NotifyIfDisplayChanged()
    {
        long floored = (long)Math.Floor(CurrentGold);
        if (floored != lastDisplayedGold)
        {
            lastDisplayedGold = floored;
            OnGoldChanged?.Invoke(CurrentGold);
        }
    }
}
