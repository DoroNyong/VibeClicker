using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable Objects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string upgradeId;
    public string upgradeName;
    [TextArea] public string description;

    public double baseCost = 10;
    public double costMultiplier = 1.15;

    public double additionalGoldPerClick = 0;
    public double additionalGoldPerSecond = 0;

    public double GetCost(int currentLevel)
    {
        return Math.Round(baseCost * Math.Pow(costMultiplier, currentLevel));
    }
}
