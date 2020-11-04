using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
using CreativeSpore.SuperTilemapEditor;
using PixelCrushers.DialogueSystem;
using Game.NPCs.Blossoms;
public class PlaceObjectManager : Singleton<PlaceObjectManager>
{

    public ItemProp CurrentItem;
    GameObject ReferencedObject;

    public float TimeBeforeHold = 0.15f;
    public float HoldDelayTimer = 0f;
    public bool HoldOnDelay = false;
    public string UseButton = "Use Tool";

    public PlaceObjectCursor Cursor;

    public int ObjectIndex = 0;
    PlayerInventory PlayerInventory;

    void Start()
    {
        PlayerInventory = FindObjectOfType<PlayerInventory>();
    }
    public void OnEnable()
    {
        Toolbar.Instance.OnSelectedSlotItemChanged += ChangeItem;
    }

    void OnDisable()
    {
        if (Toolbar.Instance != null)
            Toolbar.Instance.OnSelectedSlotItemChanged -= ChangeItem;
    }

    public delegate void SelectedItemChange();
    public event SelectedItemChange OnSelectedItemChanged;
    void ChangeItem()
    {

        if (Toolbar.Instance.SelectedSlot.ReferencedItemStack != null)
        {

            ItemBase selectedItem = Toolbar.Instance.SelectedSlot.ReferencedItemStack.ContainedItem;

            if (selectedItem as ItemProp == null)
            {
                CurrentItem = null;
            }
            else
            {
                print("ID: " + selectedItem.ID);
                CurrentItem = ItemSystem.Instance.GetItemClone(selectedItem.ID) as ItemProp;
            }

        }

        else
        {
            CurrentItem = null;
        }

        if (CurrentItem != null)
        {
            ReferencedObject = CurrentItem.Props[0];
            ObjectIndex = 0;
            Cursor.gameObject.SetActive(true);
            Cursor.Set(CurrentItem);
        }
        else
        {
            ReferencedObject = null;
            if (Cursor != null)
                Cursor.gameObject.SetActive(false);
        }


        if (OnSelectedItemChanged != null)
        {
            OnSelectedItemChanged();
        }
    }
    void Update()
    {

        if (Input.GetButtonDown("Cycle Cursor"))
        {
            CycleObject();
        }
        if (HoldOnDelay)
        {
            HoldDelayTimer += Time.deltaTime;
            if (HoldDelayTimer >= TimeBeforeHold)
            {
                HoldDelayTimer = 0f;
                HoldOnDelay = false;
            }
        }

        if (IsUseButton())
        {

            if (HoldOnDelay)
            {
                return;
            }

            if (ReferencedObject != null)
            {
                Place();
            }


            HoldOnDelay = true;

        }

        if (IsUseButtonUp())
        {
            HoldDelayTimer = HoldDelayTimer;
        }

    }


    void CycleObject()
    {
        if (CurrentItem == null)
        {
            return;
        }
        if (CurrentItem.Type != ItemSystem.ItemTypes.Prop)
        {
            return;
        }
        ObjectIndex = ((ObjectIndex + 1) % (CurrentItem.Props.Count));
        print(ObjectIndex);
        ReferencedObject = CurrentItem.Props[ObjectIndex];
        Cursor.Set(CurrentItem);
    }

    private bool IsUseButton()
    {
        if (DialogueManager.IsDialogueSystemInputDisabled()) return false;
        return (!string.IsNullOrEmpty(UseButton) && Input.GetButton("Use Item"));
    }

    private bool IsUseButtonUp()
    {
        if (DialogueManager.IsDialogueSystemInputDisabled()) return false;
        return (!string.IsNullOrEmpty(UseButton) && Input.GetButtonUp("Use Item"));
    }

    void Place()
    {
        if (Cursor.CheckPlacement() == false)
        {
            return;
        }
        PixelCrushers.MessageSystem.SendMessage(this, "PropPlaced", CurrentItem.Name);

        GameObject spawnObj = Instantiate(ReferencedObject, Cursor.transform.position, Cursor.transform.rotation);
        spawnObj.transform.position = Cursor.transform.position;
        spawnObj.transform.rotation = Cursor.transform.rotation;
        spawnObj.GetComponent<ObjectPosition>().AdjustPositions();
        if (spawnObj.GetComponent<OccupySpace>() != null)
        {
            if (spawnObj.GetComponent<OccupySpace>().isActiveAndEnabled)
                spawnObj.GetComponent<OccupySpace>().OccupyTiles();
        }
        PlayerInventory.RemoveItem(CurrentItem);
        if (spawnObj.GetComponent<Hut>() != null)
        {
            Hut hut = spawnObj.GetComponent<Hut>();
            hut.Init();
            BlossomManager.Instance.AddHut(hut.Name);
        }

        if (spawnObj.GetComponent<PixelCrushers.SpawnedObject>() != null)
        {
            PixelCrushers.SpawnedObject spawnedObject = spawnObj.GetComponent<PixelCrushers.SpawnedObject>();
            spawnedObject.key += " PlayerPlaced";
        }
        AstarPath.active.Scan();
    }

}



