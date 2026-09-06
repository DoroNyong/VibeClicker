using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string saveFilePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        saveFilePath = Path.Combine(Application.persistentDataPath, "gamesave.json");
    }

    public void SaveGame()
    {
        if (CurrencyManager.Instance == null) return;

        SaveData data = new SaveData
        {
            currentGold = CurrencyManager.Instance.CurrentGold
        };

        UpgradeItem[] items = FindObjectsByType<UpgradeItem>(FindObjectsSortMode.None);
        foreach (var item in items)
        {
            if (item.Data != null)
            {
                data.upgrades.Add(new UpgradeSaveEntry
                {
                    upgradeId = item.Data.upgradeId,
                    level = item.CurrentLevel
                });
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("[SaveManager] 데이터가 성공적으로 저장되었습니다.");
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath)) return;

        try
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if (data == null) return;

            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.SetGold(data.currentGold);
            }

            UpgradeItem[] items = FindObjectsByType<UpgradeItem>(FindObjectsSortMode.None);
            foreach (var item in items)
            {
                if (item.Data == null) continue;

                var entry = data.upgrades.Find(u => u.upgradeId == item.Data.upgradeId);
                if (entry != null)
                {
                    item.LoadLevel(entry.level);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveManager] 데이터 로드 실패: {e.Message}");
        }
    }

    public void ResetSaveData()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("[SaveManager] 세이브 데이터가 삭제되었습니다.");
        }

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.ResetState();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
