using UnityEngine;
using System.IO;

public class AreaBattle : MonoBehaviour
{
    [SerializeField] private GameObject boss1Prefab;
    [SerializeField] private GameObject boss2Prefab;
    [SerializeField] private GameObject telePortPrefabs;
    [SerializeField] private GameObject telePortNewMap;

    private bool boss1Defeated = false;
    private bool battleStarted = false;
    private bool battleCompleted = false;
    private GameObject currentBoss;

    private string saveFilePath;

    private void Start()
    {
        saveFilePath = Application.persistentDataPath + "/AreaBattle.json";
        LoadBattleState(); // Load trạng thái từ JSON

        if (telePortNewMap != null)
        {
            telePortNewMap.SetActive(false);
        }
    }

    private void StartBattle()
    {
        if (boss1Defeated)
        {
            SpawnBoss2();
        }
        else
        {
            SpawnBoss1();
        }
    }

    private void SpawnBoss1()
    {
        if (boss1Prefab != null)
        {
            currentBoss = Instantiate(boss1Prefab, transform.position, Quaternion.identity);
            currentBoss.GetComponent<Enemy>().OnDeath += Boss1Defeated;
        }
    }

    private void SpawnBoss2()
    {
        if (boss2Prefab != null)
        {
            currentBoss = Instantiate(boss2Prefab, transform.position, Quaternion.identity);
            currentBoss.GetComponent<Enemy>().OnDeath += Boss2Defeated;
        }
    }

    private void Boss1Defeated()
    {
        boss1Defeated = true;
        SaveBattleState(); // Lưu trạng thái vào JSON
        Destroy(currentBoss);
        Invoke(nameof(SpawnBoss2), 1f);
    }

    private void Boss2Defeated()
    {
        battleCompleted = true;
        Destroy(currentBoss);

        if (telePortPrefabs != null)
        {
            telePortPrefabs.SetActive(true);
        }
        if (telePortNewMap != null)
        {
            telePortNewMap.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!battleStarted && !battleCompleted && collision.CompareTag("Player"))
        {
            if (telePortPrefabs != null)
            {
                telePortPrefabs.SetActive(false);
            }
            battleStarted = true;
            StartBattle();
        }
    }

    private void SaveBattleState()
    {
        BattleState state = new BattleState { boss1Defeated = this.boss1Defeated };
        string json = JsonUtility.ToJson(state);
        File.WriteAllText(saveFilePath, json);
    }

    private void LoadBattleState()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            BattleState state = JsonUtility.FromJson<BattleState>(json);
            this.boss1Defeated = state.boss1Defeated;
        }
    }
}

[System.Serializable]
public class BattleState
{
    public bool boss1Defeated;
}
