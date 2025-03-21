using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;


    public List<ItemData> startingEquipment;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentItemSlot;
    private UI_StatSlot[] statSlot;

    [Header("Items cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;


    public float flaskCooldown { get; private set; }
    private float armorCooldown;

    [Header("Data base")]
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadEquipment;


    [SerializeField] private GameObject craftUI;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    //private void Awake()
    //{
    //    if (instance != null && instance != this)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    instance = this;

    //    DontDestroyOnLoad(gameObject); // Giữ SaveManager khi chuyển scene
    //}

    private void Start()
    {
        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();


        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipmentItemSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();

        if (statSlotParent == null)
        {
            Debug.LogError("statSlotParent is null! Make sure it is assigned in the Inspector.");
            return;
        }
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();
        AddStartingItem();



    }

    private void AddStartingItem()
    {
        foreach (ItemData_Equipment item in loadEquipment)
        {
            Debug.Log("AddStartingItem: " + item.itemName);
            EquipItem(item);
        }

        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    Debug.Log("AddStartingItem: " + item.data.itemName);
                    AddItem(item.data);
                }
            }
            return;
        }


        for (int i = 0; i < startingEquipment.Count; i++)
        {
            if (startingEquipment[i] != null)
            {
                Debug.Log("AddStartingItem: " + startingEquipment[i].itemName);
                AddItem(startingEquipment[i]);
            }
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);

        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item);
        UpdateSlotUI();
    }
    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (itemToRemove == null)
        {
            Debug.LogWarning("Item to remove is null!");
            return;
        }

        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModififers();
        }
    }
    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentItemSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentItemSlot[i].slotTypes)
                {
                    equipmentItemSlot[i].UpdateSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {

        if (statSlot == null || statSlot.Length == 0)
        {
            return;
        }

        for (int i = 0; i < statSlot.Length; i++)
        {
            if (statSlot[i] != null)
            {
                statSlot[i].UpdateStatValueUI();
            }
            else
            {
                Debug.LogWarning($"statSlot[{i}] is null!");
            }
        }

    }

    public bool CanAddItem()
    {
        if (inventory.Count >= inventoryItemSlot.Length)
        {
            Debug.Log("het slot");
            return false;
        }

        return true;
    }
    public void AddItem(ItemData _item)
    {
        if (_item == null)
        {
            return;
        }

        if (_item.ItemType == ItemType.Equipment && CanAddItem())
        {
            AddToInventory(_item);
        }
        else if (_item.ItemType == ItemType.Material)
        {
            AddToStash(_item);
        }

        UpdateSlotUI();
    }
    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }
    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }
    //public void RemoveItem(ItemData _item)
    //{
    //    if (_item == null)
    //    {
    //        Debug.LogWarning("Item to remove is null!");
    //        return;
    //    }


    //    if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
    //    {
    //        if (value.stackSize <= 1)
    //        {
    //            inventory.Remove(value);
    //            inventoryDictionary.Remove(_item);
    //        }
    //        else
    //        {
    //            value.RemoveStack();
    //        }
    //    }
    //    if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
    //    {
    //        if (stashValue.stackSize <= 1)
    //        {
    //            stash.Remove(stashValue);
    //            stashDictionary.Remove(_item);
    //        }
    //        else
    //        {
    //            stashValue.RemoveStack();
    //        }
    //    }

    //    //if (equipmentDictionary.TryGetValue(_item as ItemData_Equipment, out InventoryItem equipValue))
    //    //{
    //    //    if (value.stackSize <= 1)
    //    //    {
    //    //        equipment.Remove(value);
    //    //        equipmentDictionary.Remove(_item as ItemData_Equipment);
    //    //    }
    //    //    else
    //    //    {
    //    //        value.RemoveStack();
    //    //    }
    //    //}
    //    UpdateSlotUI();

    //}
    public void RemoveItem2(ItemData _item)
    {
        if (_item == null)
        {
            Debug.LogWarning("Item to remove is null!");
            return;
        }

        // Xóa tất cả số lượng item trong inventory
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            inventory.Remove(value);
            inventoryDictionary.Remove(_item);
        }

        // Xóa tất cả số lượng item trong stash
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            stash.Remove(stashValue);
            stashDictionary.Remove(_item);
        }

        UpdateSlotUI();
    }

    public void RemoveItem(ItemData _item, bool removeAll = false)
    {
        if (_item == null)
        {
            Debug.LogWarning("Item to remove is null!");
            return;
        }

        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (removeAll || value.stackSize <= 1) // Nếu removeAll = true, xóa luôn
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (removeAll || stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        UpdateSlotUI();
    }

    public bool CanCraft(ItemData_Equipment _itemToCraftm, List<InventoryItem> _requireMaterials)
    {

        if (!PlayerManager.instance.player.IsNearNPC())
        {
            PopupCraft("You need to be near an NPC to craft", _itemToCraftm);
            return false;
        }

        //check if all required materials are avalible with te required quantity

        foreach (var requiredItem in _requireMaterials)
        {
            if (stashDictionary.TryGetValue(requiredItem.data, out InventoryItem stashItem))
            {
                if (stashItem.stackSize < requiredItem.stackSize)
                {
                    PopupCraft("Not enough materials", _itemToCraftm);
                    //Debug.Log("Not enough materials" + requiredItem.data.name);
                    return false;
                }
            }
            else
            {
                PopupCraft("Not enough materials", _itemToCraftm);
                //Debug.Log("Materials not found in stash" + requiredItem.data.name);
                return false;
            }
        }

        // if all materials are avalible, remove them from stash

        foreach (var requiredItem in _requireMaterials)
        {
            for (int i = 0; i < requiredItem.stackSize; i++)
            {
                RemoveItem(requiredItem.data);
            }
        }

        // success and then add item

        AddItem(_itemToCraftm);
        PopupCraft("Pickup", _itemToCraftm);
        return true;
    }

    private void PopupCraft(string message, ItemData_Equipment _itemToCraftm)
    {
        craftUI.SetActive(false);
        Time.timeScale = 1;
        PlayerManager.instance.player.fx.CreatePopupText($"{message} {_itemToCraftm.name}");
        StartCoroutine(delayCanCraft());
    }

    private IEnumerator delayCanCraft()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
        craftUI.SetActive(true);

    }

    //public void RemoveItem2(ItemData _item)
    //{
    //    if (_item == null)
    //    {
    //        Debug.LogWarning("Item to remove is null!");
    //        return;
    //    }

    //    // Xóa tất cả số lượng item trong inventory
    //    if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
    //    {
    //        inventory.Remove(value);
    //        inventoryDictionary.Remove(_item);
    //    }

    //    // Xóa tất cả số lượng item trong stash
    //    if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
    //    {
    //        stash.Remove(stashValue);
    //        stashDictionary.Remove(_item);
    //    }

    //    UpdateSlotUI();
    //}




    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;


    public List<InventoryItem> GetInventory() => inventory;


    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipmentData = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                equipmentData = item.Key;
            }
        }
        return equipmentData;
    }
    public void UseFlaskHeal()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
        {
            return;
        }

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

        if (canUseFlask)
        {
            AudioManager.instance.PlaySFX(50, null);
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
        {
            Debug.Log("Flask on cooldown");
        }
    }

    public bool CanUserArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.Armor);

        if (Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }

        Debug.Log("Armor on cooldown");
        return false;
    }

    public void LoadData(GameData _data)
    {
        loadedItems.Clear();
        loadEquipment.Clear();

        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);

                    itemToLoad.stackSize = pair.Value;

                    Debug.Log("Item to load: " + itemToLoad.data.itemName + " stack size: " + itemToLoad.stackSize);
                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string loadedItemId in _data.equipmentId)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemId == loadedItemId)
                {
                    Debug.Log("Equipment to load: " + item.itemName);
                    loadEquipment.Add(item as ItemData_Equipment);
                }
            }

        }
        AddStartingItem();
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();
        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Fill up item data base")]
    private void FillUpItemDataBase() => itemDataBase = new List<ItemData>(GetItemDataBase());
   
    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
#endif
}
