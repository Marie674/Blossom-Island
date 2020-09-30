using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemSystem;
using TMPro;

public class ToolbarSlotUI : MonoBehaviour
{

    public TextMeshProUGUI AmountText;
    private Image ItemIcon;

    [SerializeField]
    public InventoryItemStack ReferencedItemStack = null;
    public bool IsActiveSlot = false;
    public GameObject ActiveFrame;
    public bool IsInit = false;

    private bool IsQuitting;

    public List<ItemType> AcceptedTypes;

    public KeyCode key = KeyCode.None;

    public string buttonName = string.Empty;

    void OnApplicationQuit()
    {
        IsQuitting = true;
    }

    // Get the components and update current item info and visuals
    public void Init()
    {
        ItemIcon = transform.Find("ItemIcon").GetComponent<Image>();
        ActiveFrame = transform.Find("ActiveFrame").gameObject;
        if (ReferencedItemStack != null && ReferencedItemStack.ContainedItem != null)
        {
            ChangeItem(ReferencedItemStack);
        }
        else
        {
            ChangeItem(null);
        }

        IsInit = true;
    }

    void OnEnable()
    {
        if (IsInit == false)
        {
            Init();
        }
    }

    void OnDisable()
    {
        if (!IsQuitting)
        {
            if (ReferencedItemStack != null)
            {
                ReferencedItemStack.OnItemChanged -= UpdateItem;

            }
            IsInit = false;
        }

    }

    // Event broadcast upon changing item

    public delegate void SlotItemChange();
    public event SlotItemChange OnSlotItemChanged;

    public void ChangeItem(InventoryItemStack pItemStack)
    {
        //	print (pItemStack);
        //If an item stack is specified

        if (pItemStack != null && !AcceptedTypes.Contains(pItemStack.ContainedItem.itemType))
        {
            return;
        }
        if (pItemStack != null && pItemStack.Amount > 0)
        {

            // If the slot already referenced an item stack, unsubscribe from its item change
            if (ReferencedItemStack != null)
            {
                ReferencedItemStack.OnItemChanged -= UpdateItem;
            }
            // Set the reference stack to the specified stack and subscribe to its item change event
            ReferencedItemStack = pItemStack;
            ReferencedItemStack.OnItemChanged += UpdateItem;

            UpdateVisuals();

            // If stack was referenced by another slot, remove the reference
            foreach (ToolbarSlotUI slot in Toolbar.Instance.Slots)
            {
                if (slot != this && slot.ReferencedItemStack == this.ReferencedItemStack)
                {
                    slot.ChangeItem(null);
                }
            }
        }
        else if (pItemStack == null || pItemStack.Amount <= 0)
        {
            // unsubscribe from item change event
            if (ReferencedItemStack != null)
            {
                ReferencedItemStack.OnItemChanged -= UpdateItem;
            }

            //		print(gameObject.name + " set null item stack ref");
            // remove reference and update visuals
            ReferencedItemStack = null;
            UpdateVisuals();
        }


        // Call item change event
        if (OnSlotItemChanged != null)
        {
            OnSlotItemChanged();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(key) || (!string.IsNullOrEmpty(buttonName) && Input.GetButtonDown(buttonName)))
        {
            Toolbar.Instance.ChangeSelectedSlot(this);
        }
    }

    // If an item stack is referenced, set the visual info to match, if not, erase visuals
    void UpdateVisuals()
    {
        if (ReferencedItemStack != null)
        {
            ItemIcon.color = Color.white;
            ItemIcon.sprite = ReferencedItemStack.ContainedItem.itemIcon;
            if (ReferencedItemStack.Amount > 1)
            {
                AmountText.text = ReferencedItemStack.Amount.ToString();
            }
            else
            {
                AmountText.text = string.Empty;
            }
        }
        else
        {
            ItemIcon.sprite = null;
            ItemIcon.color = new Color(1, 1, 1, 0);
            AmountText.text = string.Empty;
        }
    }

    void UpdateItem()
    {
        ChangeItem(ReferencedItemStack);
    }

    // Set is active to the toggle and show/hide the frame
    public void Activate(bool pToggle)
    {
        IsActiveSlot = pToggle;
        if (ActiveFrame != null)
        {
            ActiveFrame.SetActive(pToggle);
        }
    }


}
