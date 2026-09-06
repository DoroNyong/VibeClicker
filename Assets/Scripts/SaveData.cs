using System;
using System.Collections.Generic;

[Serializable]
public class UpgradeSaveEntry
{
    public string upgradeId;
    public int level;
}

[Serializable]
public class SaveData
{
    public double currentGold;
    public List<UpgradeSaveEntry> upgrades = new List<UpgradeSaveEntry>();
}