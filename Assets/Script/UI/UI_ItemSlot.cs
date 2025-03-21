using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    public InventoryItem item;
    protected  UI ui;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }
    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        if (item == null || item.data == null)
        {
            return;
        }



        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        //itemImage.gameObject.SetActive(false);
        itemText.text = "";
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ui == null)
        {
            Debug.LogError("UI reference is missing in UI_ItemSlot!");
            return;
        }

        if (item == null || item.data == null)
        {
            return;
        }


        //AudioManager.instance.PlaySFX(38, null);
        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            AudioManager.instance.PlaySFX(39, null);
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if (item == null)
        {
            return;
        }

        if (item.data != null && item.data.ItemType == ItemType.Equipment)
        {
            AudioManager.instance.PlaySFX(36, null);
            Inventory.instance.EquipItem(item.data);
        }

        ui.itemToolTip.HideToolTip();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(ui == null)
        {
            Debug.LogError("UI reference is missing in UI_ItemSlot!");
            return;
        }
        ui.itemToolTip.HideToolTip();
    }
}
