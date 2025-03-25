using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ChestEntry
{
    public string chestID;
    public bool isOpened;
    public bool isDropped;

    public ChestEntry(string id, bool opened, bool dropped)
    {
        chestID = id;
        isOpened = opened;
        isDropped = dropped;
    }
}

[System.Serializable]
public class ChestData
{
    public List<ChestEntry> defeatedChests = new List<ChestEntry>();

    public bool IsChestOpened(string id)
    {
        return defeatedChests.Exists(entry => entry.chestID == id && entry.isOpened);
    }

    public bool HasDropped(string id)
    {
        var chest = defeatedChests.Find(entry => entry.chestID == id);
        return chest != null && chest.isDropped;
    }

    public void AddChest(string id, bool isOpened, bool isDropped)
    {
        var chest = defeatedChests.Find(entry => entry.chestID == id);
        if (chest != null)
        {
            chest.isOpened = isOpened;
            chest.isDropped = isDropped;
        }
        else
        {
            defeatedChests.Add(new ChestEntry(id, isOpened, isDropped));
        }
    }
}

public class Treasure : MonoBehaviour
{
    private Animator animator;
    private ItemDrop itemDrop;
    [SerializeField] private string treasureId;
    private bool isDropped = false;
    private bool isOpened = false;

    private static string savePath;
    private static ChestData chestData;

    [ContextMenu("Generate chest Id")]
    private void GenerateId()
    {
        treasureId = System.Guid.NewGuid().ToString();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        itemDrop = GetComponent<ItemDrop>();

        LoadChestData();

        if (chestData.IsChestOpened(treasureId))
        {
            isOpened = true;
            isDropped = chestData.HasDropped(treasureId);
            animator.SetBool("Open", true);
            animator.SetBool("Idle", false);
        }
    }

    private void RegisterChest(string chestID, bool isOpened, bool isDropped)
    {
        chestData.AddChest(chestID, isOpened, isDropped);
        SaveChestData();
    }

    private void LoadChestData()
    {
        savePath = Application.persistentDataPath + "/ChestData.json";

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            chestData = JsonUtility.FromJson<ChestData>(json);
        }
        else
        {
            chestData = new ChestData();
        }
    }

    private void SaveChestData()
    {
        string json = JsonUtility.ToJson(chestData, true);
        File.WriteAllText(savePath, json);
    }

    private void Update()
    {
        if (!isOpened)
        {
            animator.SetBool("Idle", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isDropped)
            {
                AudioManager.instance.PlaySFX(22, transform);
                itemDrop.GenerateDrop();
                isDropped = true;
            }

            if (isOpened)
            {
                return;
            }

            animator.SetBool("Move", true);
            isOpened = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RegisterChest(treasureId, isOpened, isDropped);
            animator.SetBool("Open", true);
            animator.SetBool("Idle", false); 
        }
    }
}
