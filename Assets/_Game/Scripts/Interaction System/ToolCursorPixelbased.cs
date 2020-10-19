using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCursorPixelbased : ToolCursorBase
{

    Sprite CursorSprite;
    ToolControllerBase CurrentToolController;
    public List<GameObject> AffectedObjects = new List<GameObject>();

    public bool CanUse;

    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        ToolManager.Instance.OnSelectedToolChanged += ToolChange;
        ToolChange();
    }

    void Disable()
    {
        ToolManager.Instance.OnSelectedToolChanged += ToolChange;
    }

    void ToolChange()
    {
        if (ToolManager.Instance.CurrentTool == null)
        {
            CurrentToolController = null;
            return;
        }
        CurrentToolController = ToolManager.Instance.CurrentToolController as ToolControllerBase;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        CheckValidity();
    }

    void CheckValidity()
    {
        if (CurrentToolController == null)
        {
            return;
        }
        bool isValid = true;
        if (AffectedObjects.Count < 1)
        {
            isValid = false;
            SetColor(isValid);

            return;
        }


        bool semiValid = false;
        int validCount = 0;
        int invalidCount = 0;


        foreach (GameObject obj in AffectedObjects)
        {
            HarvestObject harvestObject = obj.GetComponent<HarvestObject>();

            if (harvestObject != null)
            {
                if (harvestObject.RequiredToolLevel <= CurrentToolController.CurrentTool.Level)
                {
                    validCount += 1;
                }
                else
                {
                    invalidCount += 1;
                }
            }
        }

        if (invalidCount > 0)
        {
            if (validCount > 0)
            {
                isValid = true;
                semiValid = true;
            }
            else
            {
                isValid = false;
            }
        }

        if (CurrentToolController.CheckUseValidity() == false)
        {
            isValid = false;
        }

        if (isValid)
        {
            CanUse = true;
        }
        else
        {
            CanUse = false;
        }

        SetColor(isValid, semiValid);

    }


    void SetColor(bool pIsValid, bool pIsSemiValid = false)
    {

        if (pIsValid)
        {
            if (pIsSemiValid == true)
            {
                Sprite.color = Color.yellow;
            }
            else
            {
                Sprite.color = Color.cyan;
            }

        }
        else
        {
            Sprite.color = Color.red;
        }
    }

    void OnTriggerStay2D(Collider2D pOther)
    {
        if (pOther.isTrigger == false)
        {
            return;
        }
        if (AffectedObjects.Contains(pOther.gameObject))
        {
            return;
        }

        List<string> allowedTags = CurrentToolController.AllowedTags;
        if (allowedTags.Contains(pOther.tag))
        {
            AffectedObjects.Add(pOther.gameObject);

        }

    }

    void OnTriggerExit2D(Collider2D pOther)
    {
        if (AffectedObjects.Contains(pOther.gameObject))
        {
            AffectedObjects.Remove(pOther.gameObject);
        }
    }

}
