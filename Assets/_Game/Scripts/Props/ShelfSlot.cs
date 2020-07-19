// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class ShelfSlot : MonoBehaviour
// {

//     public Sprite SoldOutSprite;
//     public ItemContainer ItemSlot;
//     public int CurrentAmount = 0;
//     public int MaxAmount = 99;
//     public SpriteRenderer Sprite;
//     public TextMeshProUGUI AmountText;
//     public bool CanInteract = false;

//     public void UpdateVisuals()
//     {
//         if (CurrentAmount > 0)
//         {
//             Sprite.color = Color.white;
//             if (AmountText != null)
//             {
//                 AmountText.text = CurrentAmount.ToString();

//             }
//             Sprite.sprite = ItemSlot.item.itemIcon;
//         }
//         else
//         {
//             Sprite.sprite = SoldOutSprite;
//             if (AmountText != null)
//                 AmountText.text = string.Empty;
//         }
//     }

//     public void Interact()
//     {

//         if (CanInteract == false)
//         {
//             return;
//         }


//         BuyUI ui = FindObjectOfType<BuyUI>();

//         float maxAmount = FindObjectOfType<PlayerInventory>().Gold / ItemSlot.item.value;
//         maxAmount = Mathf.Floor(maxAmount);

//         if (maxAmount > CurrentAmount)
//         {
//             maxAmount = CurrentAmount;
//         }
//         // if (maxAmount <= 0)
//         // {
//         //     return;
//         // }
//        // ui.Open(this, (int)maxAmount);

//     }

//     public void SoldOut()
//     {
//         CanInteract = false;
//     }

//     public void Buy(int pAmount)
//     {
//         ItemSpawner.Instance.SpawnItems(ItemSlot.item, GameManager.Instance.Player.transform.position, (uint)pAmount);
//         CurrentAmount -= pAmount;
//         FindObjectOfType<PlayerInventory>().ChangeGold(-(int)(ItemSlot.item.value * pAmount));
//         if (CurrentAmount == 0)
//         {
//             SoldOut();
//         }
//         UpdateVisuals();
//     }


// }
