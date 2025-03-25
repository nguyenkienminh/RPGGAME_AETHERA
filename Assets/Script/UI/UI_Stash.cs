using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Stash : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform stashGrid;
    [SerializeField] private TextMeshProUGUI stashTitle;

    private List<UI_ItemSlot> stashSlots = new List<UI_ItemSlot>();

    public void InitializeStash(int slotCount)
    {
        ClearStash();
        for (int i = 0; i < slotCount; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, stashGrid);
            UI_ItemSlot itemSlot = newSlot.GetComponent<UI_ItemSlot>();
            stashSlots.Add(itemSlot);
        }
    }

    public void UpdateStash(List<InventoryItem> items)
    {
        for (int i = 0; i < stashSlots.Count; i++)
        {
            if (i < items.Count)
            {
                stashSlots[i].UpdateSlot(items[i]);
            }
            else
            {
                stashSlots[i].CleanUpSlot();
            }
        }
    }

    public void ClearStash()
    {
        foreach (Transform child in stashGrid)
        {
            Destroy(child.gameObject);
        }
        stashSlots.Clear();
    }

    public void ShowStash(bool show)
    {
        gameObject.SetActive(show);
    }
}
