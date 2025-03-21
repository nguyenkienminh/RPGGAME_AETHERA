using UnityEngine;

public class itemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public ItemData itemData;


    private void SetupVisuals()
    {
        if (itemData == null)
        {
            return;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && itemData != null)
        {
            sr.sprite = itemData.icon;
            gameObject.name = "Item object - " + itemData.name;
        }
    }


    public void SetupItem(ItemData _itemData,Vector2 _velocity)
    {
        itemData = _itemData;
        rb.linearVelocity = _velocity;
        SetupVisuals();
    }
  



    public void PickupItem()
    {
        if (itemData == null) return;

        if (itemData.itemName.Equals("Coin"))
        {
            PlayerManager.instance.currency += 1;
            Destroy(gameObject);
            return;
        }

        if (!Inventory.instance.CanAddItem() && itemData.ItemType == ItemType.Equipment)
        {
            rb.linearVelocity = new Vector2(0, 7);
            PlayerManager.instance.player.fx.CreatePopupText("Inventory is full");
            return; 
        }

        AudioManager.instance.PlaySFX(18, transform);
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
