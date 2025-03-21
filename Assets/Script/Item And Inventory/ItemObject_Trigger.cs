using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    private itemObject myItemObject => GetComponentInParent<itemObject>();
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        if (collision.GetComponent<Player>() != null)
        {
            if(collision.GetComponent<CharacterStats>().isDead)
            {
                return;
            }
            Debug.Log("Pickup item success");
            PlayerManager.instance.player.fx.CreatePopupText($"Pickup {myItemObject.itemData.itemName}");
            myItemObject.PickupItem();
        }
    }
}
