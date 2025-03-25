using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotTypes;


    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotTypes.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null) return;

        //unequipment
        AudioManager.instance.PlaySFX(37, null);
        Inventory.instance.UnequipItem(item.data as ItemData_Equipment);
        Inventory.instance.AddItem(item.data as ItemData_Equipment);

        ui.itemToolTip.HideToolTip();
        CleanUpSlot();
    }
}
