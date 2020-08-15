using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
using UnityEngine.SceneManagement;
using Game.NPCs.Blossoms;
public class PlaceObjectCursor : MonoBehaviour
{
    protected SpriteRenderer Sprite;
    protected ItemPlaceable Item;
    public float MaxDistance;

    public float Units = 1f;

    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
    }

    public void Set(ItemPlaceable pItem)
    {
        Item = pItem;
        Sprite.sprite = pItem.ObjectPrefabs[PlaceObjectManager.Instance.ObjectIndex].GetComponent<SpriteRenderer>().sprite;
    }

    protected virtual void LateUpdate()
    {
        Position();
        UpdateVisuals();
    }

    protected void Position()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 pos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 SpriteTopLeft;

        pos.x = Units * Mathf.Round(pos.x / Units);
        pos.y = Units * Mathf.Round(pos.y / Units);

        Sprite.transform.position = pos;
        SpriteTopLeft = new Vector3(Sprite.bounds.min.x, Sprite.bounds.max.y, 0);

        Vector2 spritePivot = Sprite.transform.position;
        Vector2 spriteOffset = new Vector2(spritePivot.x - SpriteTopLeft.x, spritePivot.y - SpriteTopLeft.y);


        transform.position = new Vector3(pos.x - spriteOffset.x, pos.y + spriteOffset.y, -9f);
    }


    public bool CheckPlacement()
    {
        if (Item == null)
        {
            return false;
        }
        if (Item.itemType != ItemType.PlaceableItem)
        {
            return false;
        }

        Vector3 spriteTopLeft = new Vector3(Sprite.bounds.min.x, Sprite.bounds.max.y, 0);

        GameObject referencedObject = Item.ObjectPrefabs[PlaceObjectManager.Instance.ObjectIndex];
        bool validPlacement = true;

        //Check if collisions are clear
        if (referencedObject.GetComponent<OccupySpace>().CheckTiles(spriteTopLeft) == false)
        {
            validPlacement = false;
        }
        if (Item.ValidLevels.Count > 0 && !Item.ValidLevels.Contains(SceneManager.GetActiveScene().name))
        {
            validPlacement = false;
        }
        if (referencedObject.GetComponent<Hut>() != null)
        {
            if (BlossomManager.Instance.HutAmount >= BlossomManager.Instance.MaxHuts)
            {
                validPlacement = false;
            }
        }

        return validPlacement;

    }

    void UpdateVisuals()
    {
        if (CheckPlacement())
        {
            Sprite.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0.5f);
        }
        else
        {
            Sprite.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
        }
    }


    // public void SetObjectOffset(){
    // 	if (DisplaySprite.sprite == null) {
    // 		return;
    // 	}

    // 	if (GetComponent<SpriteRenderer> ().sprite != null) {
    // 		//transform.localPosition = Vector3.zero;

    // 		Vector3 objPivot = GetComponent<SpriteRenderer> ().sprite.pivot;
    // 		objPivot = new Vector3(objPivot.x,objPivot.y,0);
    // 		Vector3 objTopLeft = new Vector3 (0, (SpriteExtents.y*2f)*100f, 0);

    // 		Vector3 difference = objPivot - objTopLeft;
    // 		Vector3 offsetPos = transform.position;

    // 		if (transform.localRotation.eulerAngles.z == 0 || transform.localRotation.eulerAngles.z == 180) {
    // 			offsetPos.x += difference.x / 100f;
    // 			offsetPos.y += difference.y / 100f;
    // 		} else {
    // 			offsetPos.x += difference.y / 100f;
    // 			offsetPos.y += difference.x / 100f;
    // 		}
    // 		transform.position = offsetPos;
    // 	}
    // }



}
