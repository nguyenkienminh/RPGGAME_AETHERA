using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();

       
    }

    public void SetupCraftSlot(ItemData_Equipment _data) 
    {
        if (_data == null)
        {
            Debug.LogError("ItemData_Equipment is NULL!");
            return;
        }

        if (item == null)
        {
            Debug.LogError("Item reference is NULL in UI_CraftSlot!");
            return;
        }
        item.data = _data;
        if (itemImage == null)
        {
            Debug.LogError("itemImage is NULL in UI_CraftSlot!");
            return;
        }
        itemImage.sprite = _data.icon;
        if (itemText == null)
        {
            Debug.LogError("itemText is NULL in UI_CraftSlot!");
            return;
        }
        itemText.text = _data.itemName;

        if(itemText.text.Length > 10)
        {
            itemText.fontSize = itemText.fontSize * .7f;
        }else
        {
            itemText.fontSize = 30;
        }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.instance.PlaySFX(7, null);
        ui.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
    }

}
