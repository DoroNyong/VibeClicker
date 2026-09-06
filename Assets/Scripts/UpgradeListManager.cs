using System.Collections.Generic;
using UnityEngine;

public class UpgradeListManager : MonoBehaviour
{
    [Header("Prefabs & Container")]
    [SerializeField] private GameObject upgradeItemPrefab;
    [SerializeField] private Transform contentContainer;

    [Header("Data")]
    [SerializeField] private List<UpgradeData> upgradeDataList = new List<UpgradeData>();

    private void Start()
    {
        SpawnUpgradeItems();

        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.LoadGame();
        }
    }

    private void SpawnUpgradeItems()
    {
        if (upgradeItemPrefab == null || contentContainer == null) return;

        foreach (var data in upgradeDataList)
        {
            if (data == null) continue;

            GameObject itemObj = Instantiate(upgradeItemPrefab, contentContainer);
            if (itemObj.TryGetComponent<UpgradeItem>(out var item))
            {
                item.SetData(data);
            }
        }
    }
}
