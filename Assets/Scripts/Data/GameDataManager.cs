using UnityEngine;
using System.IO;

public class GameDataManager : MonoBehaviour
{
    private string saveFilePath;

    private void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "playerData.json");

        Player player = FindObjectOfType<Player>();

        if (player != null && player.Data != null)
        {
            LoadPlayerData(player.Data);
        }
        else
        {
            Debug.LogError("Player or Player data is missing.");
        }
    }

    public void SavePlayerData(PlayerSO playerSO)
    {
        string json = playerSO.ToJson();
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Player data saved.");
    }

    public void LoadPlayerData(PlayerSO playerSO)
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            playerSO.LoadFromJson(json);
            Debug.Log("Player data loaded.");
        }
        else
        {
            Debug.LogError("No save file found.");
        }
    }

    // 게임 종료 시 데이터를 저장합니다.
    private void OnApplicationQuit()
    {
        Player player = FindObjectOfType<Player>();

        if (player != null && player.Data != null)
        {
            SavePlayerData(player.Data);
        }
    }
}
